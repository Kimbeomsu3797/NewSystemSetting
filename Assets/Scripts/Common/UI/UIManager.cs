using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonBehaviour<UIManager>
{
    public Transform UICanvasTrs; // (UI화면을 랜더링할 캔버스 컴포넌트 트랜스폼)
    //UI 화면을 이 UI 캔버스 트랜스폼 하위에 위치시켜줘야하기 때문에 필요함.

    public Transform ClosedUITrs; // UI 화면을 닫을 때 비활성화 시킨 UI화면들을 위치시켜줄 트랜스폼

    private BaseUI m_FrontUI; //UI 화면이 열려있을 때 가장 상단에 열려있는 UI

    //현재 열려있는, 즉 활성화 되어있는 UI화면을 담고 있는 변수(풀)
    private Dictionary<System.Type, GameObject> m_OpenUIPool = new Dictionary<System.Type, GameObject>();
    //닫혀있는, 즉 비활성화 되어 있는 UI 화면을 담고 있는 변수(풀)
    private Dictionary<System.Type, GameObject> m_ClosedUIPool = new Dictionary<System.Type, GameObject>();
    //UI 화면이 열려있는지 닫혀있는지 구분이 필요하기 때문에 UI풀을 위와 같이 2개의 변수로 관리

    private GoodsUI m_StatsUI;

    protected override void Init()
    {
        base.Init();
        //컴포넌트가 연동된 게임 오브젝트를 찾아서 GoodsUI컴퍼넌트를 리턴
        m_StatsUI = FindObjectOfType<GoodsUI>();

        if (!m_StatsUI)
        {
            Logger.Log("No stats ui component found.");
        }
    }

    //열기를 원하는 UI화면의 실제 인스턴스를 가져오는 함수
    //out 함수에서는 한가지 값이나 참조만 반환할 수 있기 때문에
    //여러가지 값이나 참조를 반환하고 싶을 때 이렇게 out 매개 변수를 사용
    //이함수는 BaseUI, isAlreadyOpen 두가지 값을 반활할 수 있다.
    private BaseUI GetUI<T>(out bool isAlreadyOpen)
    {
        System.Type uiType = typeof(T); // T는 열고자 하는 화면 UI 클래스 타입. 이걸 uitype으로 받아온다.

        BaseUI ui = null;
        isAlreadyOpen = false;

        //만약에 m_OpenUIPool에 열고자 하는 UI가 존재한다면
        if (m_OpenUIPool.ContainsKey(uiType))
        {
            //풀에 있는 해당 오브젝트의 BaseUI 컴포넌트를 ui변수에 대입
            ui = m_OpenUIPool[uiType].GetComponent<BaseUI>();
            isAlreadyOpen = true;
        } //그렇지 않고 m_OpenUIPool에 존재한다면
        else if (m_ClosedUIPool.ContainsKey(uiType))
        {
            //해당 풀에 있는 BaseUI 컴포넌트를 ui변수에 대입
            ui = m_ClosedUIPool[uiType].GetComponent<BaseUI>();
            m_ClosedUIPool.Remove(uiType); // 풀에서 해당 인스턴스를 삭제
        }
        else//그렇지 않고 아예 한번도 생성된 적이 없는 인스턴스라면
        {
            //생성을 해줌
            var uiObj = Instantiate(Resources.Load($"UI/{uiType}", typeof(GameObject))) as GameObject;
            //프리팹의 이름이 반드시 UI 클래스의 이름과 동일해야함.
            //왜냐하면 UI클래스의 이름으로 경로를 만들어서 리소스 폴더에서 로드해오라고 요청하기 때문

            ui = uiObj.GetComponent<BaseUI>();
        }

        return ui;
    }

    //UI화면을 여는 기능하는 함수
    public void OpenUI<T>(BaseUIData uIData)
    {
        System.Type uiType = typeof(T);

        Logger.Log($"{GetType()}::OpenUI({uiType})"); // 어떤 UI화면을 열고자하는지 로그를 찍는다.
        bool isAlreadyOpen = false; // 이미 열려있는지 알 수 있는 변수 선언

        var ui = GetUI<T>(out isAlreadyOpen);

        if (!ui) // 없으면 에러 로그
        {
            Logger.LogError($"{uiType} does not exist");
            return;
        }
        if(isAlreadyOpen) // 이미 열려있으면 이것 또한 비정상적인 요청이라고 간주하고 로그
        {
            Logger.LogError($"{uiType} is already open");
            return;
        }

        //위의 유효성 검사를 통과해서 정상적으로 UI화면이 열릴 수 있다면
        //이제 실제로 UI화면을 열고 데이터를 세팅해 준다.
        var siblingIdx = UICanvasTrs.childCount; //childCount 하위에 있는 게임 오브젝트 갯수
        //디버그 찍어보기
        ui.Init(UICanvasTrs); //UI화면 초기화(위치시켜야할 상단 트랜스폼)

        // 하이라이키 순위 변경 SetSiblingIndex : 매개변수를 넣어서 순위를 지정
        ui.transform.SetSiblingIndex(siblingIdx);
        //디버그 찍어보기
        //siblingIdx는 0부터 시작하는데 0부터 0,1,2,3
        //이렇게 정수값 1단위로 늘어난다.
        //생성하고자하는 ui화면을 이미 생성되어있는 ui화면들 보다
        //상단에 위치시켜 줘야하기 때문에
        //현재 존재하는 uicanvastrs 하위의 오브젝트들의 개수를 받아와서
        //siblingIdx 값으로 넘겨주는 것임.
        //siblingidx가 0부터 시작하기 때문에 childCount가 새로운 ui화면을 추가할 시
        //가장 큰 siblingIdx값이 되기 때문
        //예를 들어 자식이 2개 ->0, 다음에 생성되면 3개가 되는데 그게 1이되고 그다음에 생성되면 2가 되는 식으로
        //매번 캔버스 상에서 가장 아래에 위치하게 되고 화면에서는 가장 최상단에 노출된다.
        //캔버스를 여러개를 관리한다 생각하니 좀 쉽게 이해가됨
        ui.gameObject.SetActive(true); // 컴포넌트가 연동된 게임오브젝트 활성화
        ui.SetInfo(uIData); //UI화면에 보이는 UI요소의 데이터를 세팅
        ui.ShowUI();

        m_FrontUI = ui; //현재 열고자하는 화면 UI가 가장 상단에 있는 UI가 될것이기 때문에 이렇게 설정
        m_OpenUIPool[uiType] = ui.gameObject; //m_OpenUIPool에 생성한 UI인스턴스를 넣어준다.
    }

    //화면을 닫는 함수
    public void CloseUI(BaseUI ui)
    {
        System.Type uiType = ui.GetType();

        Logger.Log($"CloseUI : UI:{uiType}"); //어떤 ui를 닫아주는지 로그

        ui.gameObject.SetActive(false);

        m_OpenUIPool.Remove(uiType); // 오픈풀에서 제거
        m_ClosedUIPool[uiType] = ui.gameObject; //클로즈풀에 추가
        ui.transform.SetParent(ClosedUITrs); //ClosedUITrs 하위로 위치

        m_FrontUI = null; // 최상단 UI를 널로 초기화

        //현재 최상단에 있는 ui화면 오브젝트를 가져온다.

        var lastChild = UICanvasTrs.GetChild(UICanvasTrs.childCount - 1);

        //만약 ui가 존재한다면 이 ui화면 인스턴스를 최상단 ui로 대입
        if (lastChild)
        {
            m_FrontUI = lastChild.gameObject.GetComponent<BaseUI>();
        }
    }
    //특정 UI화면이 열려있는지 확인하고 그 열려있는 UI화면을 가져오는 함수
    public BaseUI GetActiveUI<T>() //이름 신경쓰자 이후에 이름이 달라 에러가 발생했었다. -GetActivePopupUI이런 이름이였음
    {
        var uiType = typeof(T);
        //m_OpenUIPool에 특정 화면 인스턴스가 존재한다면 그 화면 인스턴스를 리턴해 주고 그렇지 않으면 널 리턴
        return m_OpenUIPool.ContainsKey(uiType) ? m_OpenUIPool[uiType].GetComponent<BaseUI>() : null;

    }

    //UI화면이 열린것이 하나라도 있는지 확인하는 함수
    public bool ExistsOpenUI()
    {
        return m_FrontUI != null; //m_FrontUI가 null인지 아닌지 확인해서 bool값을 반환
    }

    //현재 가장 최상단에 있는 인스턴스를 리턴하는 함수

    public BaseUI GetCurrentFrontUI()
    {
        return m_FrontUI;
    }

    //가장 최상단에 있는 UI화면 인스턴스를 닫는 함수
    public void CloseCurrFrontUI()
    {
        m_FrontUI.CloseUI();
    }

    //열려있는 모든 UI화면을 닫으라는 함수

    public void CloseAllOpenUI()
    {
        while (m_FrontUI)
        {
            m_FrontUI.CloseUI(true);
        }
    }

    public void EnalbeStatsUI(bool value) // => EnableGoodsUI
    {
        m_StatsUI.gameObject.SetActive(value);

        if (value)
        {
            m_StatsUI.SetValues();//함수를 호출해서 보유한 재화 수량 표시
        }
    }

}
