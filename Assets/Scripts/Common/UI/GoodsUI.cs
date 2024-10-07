using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperMaxim.Messaging;
using System;


//재화 변동시 발생할 메세지 클래스를 선언
//골드 변동 메세지
public class GoldUpdateMsg
{
    public bool isAdd; //재화가 증가한 것인지 감소한 것인지 여부를 나타내는 변수
}
//보석 변동 메세지
public class GemUpdateMsg
{
    public bool isAdd;
}
//위의 두가지 메세지가 발행되었을 때 이 GoodsUI 클래스가
//구독자가 되어 메세지를 받고 그에 따른 처리
public class GoodsUI : MonoBehaviour
{
    //유저가 보유한 보석과 골드 수량을 표시해줄 텍스트 컴포넌트
    public TextMeshProUGUI GoldAmountTxt;
    public TextMeshProUGUI GemAmountTxt;
    //변수 추가
    public Image GoldIcon; // 골드 아이템 위치를 알기 위해 골드 아이콘 변수 선언
    public Image GemIcon; // 잼 아이템 위치를 알기 위해 골드 아이콘 변수 선언

    //골드 증가 연출을 코루틴을 통해서 실행할 건데 실행 중인
    //코루틴을 참조할 수 있는 코루틴 변수 선언
    private Coroutine m_GoldIncreaseCo;
    //이 변수를 선언하는 이유는 만약 재화 획득이 빠르게 여러번 요청되어 이미 획득 연출이 빠르게
    //진행중인데 새로운 획득 연출 요청 처리가 오게 되면 기존 획득 연출을 취소하고 새로운 획득 연출로 덮어쓰기 위함.
    private Coroutine m_GemIncreaseCo;
    //보석 연출도 동일

    //재화 텍스트 증가 연출이 몇초 안에 실행되어야 하는지 그 시간을 담고 있는 상수
    private const float GOODS_INCRASE_DURATION = 0.5f;

    //위에 두가지 메세지가 발행되었을 때 이 GoodsUI가 구독자가 되어
    //이 클래스가 활성화 될 때
    //이 클래스를 재화 변동 메세지 구독자로 등록
    //여기에 한 이유는 이 인스턴스가 활성화 되어 있을 때만
    //재화 변동 메세지를 받아 처리하길 원하기 때문
    private void OnEnable()
    {
        //메세지 타입을 명시
        //그리고 매개변수에 이 메세지를 받았을 때 실행할 함수를 넘겨준다.
        Messenger.Default.Subscribe<GoldUpdateMsg>(OnUpdateGold);
        Messenger.Default.Subscribe<GemUpdateMsg>(OnUpdateGem);
    }
    //위와 반대로 구독 해제
    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<GoldUpdateMsg>(OnUpdateGold);
        Messenger.Default.Unsubscribe<GemUpdateMsg>(OnUpdateGem);

    }
    //먼저 골드 재화가 변경 되었을 시 실행할 함수 작성 (획득 연출)
    private void OnUpdateGold(GoldUpdateMsg goldUpdateMsg) // 매개변수로 메세지를 받는다.
        //이렇게 함수를 선언해 주면 goldui인스턴스에서 goldupdate메세지를 받았을 때
        //이 함수가 자동 실행 됨.
    {
        //먼저 유저 데이터를 가져온다.
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if(userGoodsData == null)
        {
            Logger.LogError("UserGoodsData does not exist.");
            return;
        }
        AudioManager.Instance.PlaySFX(SFX.ui_get); // 재화 획득 효과음 재생
        //만약 재화가 증가되었다면 증가 연출 처리
        if (goldUpdateMsg.isAdd)
        {
            if(m_GoldIncreaseCo != null) //기존 연출이 있다면 취소
            {
                StopCoroutine(m_GoldIncreaseCo);
            }
            m_GoldIncreaseCo = StartCoroutine(IncreaseGoldCo());
            
        }
        else
        {
            GoldAmountTxt.text = userGoodsData.Gold.ToString("N0");
        }
    }
    //재화 연출을 담당하는 코루틴
    private IEnumerator IncreaseGoldCo()
    {
        //유저데이터를 가져온다.
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if(userGoodsData == null)
        {
            Logger.LogError("UserGoodsData does not exist.");
            yield break;
        }
        var amount = 10;
        for (int i =0; i <amount; i++)
        {
            //반복문으로 지정한 수 만큼 인스턴스 생성
            //캔버스 하위에 위치
            var goldObj = Instantiate(Resources.Load("UI/GoldMove", typeof(GameObject))) as GameObject;
            goldObj.transform.SetParent(UIManager.Instance.UICanvasTrs);
            //위치와 스케일 초기화
            goldObj.transform.localScale = Vector3.one;
            goldObj.transform.localPosition = Vector3.zero;
            //인스턴스에 연동된 GoodsMove 컴포넌트를 받아와 SetMove함수 호출
            goldObj.GetComponent<GoodsMove>().SetMove(i, GoldIcon.transform.position);
        }
        yield return new WaitForSeconds(1f);
        //재화 텍스트 증가 연출부터 해주고
        //그 다음에 재화 오브젝트들이 생성되어 우측 상단의 UI쪽으로 이동하는 처리를 해줌.
        AudioManager.Instance.PlaySFX(SFX.ui_increase); // 재화 증가 효과음

        var elapsedTime = 0f; // 경과 시간을 저장할 변수
        //햔재 UI에 표시된 골드 수치를 가져온다.(쉼표 제거)
        var currTextValue = Convert.ToInt64(GoldAmountTxt.text.Replace(",", ""));
        var destValue = userGoodsData.Gold; // 실제로 증가되어 표시되어야할 골드 수치

        //연출 시간 동안에 서서히 목표 수치로 도달할 수 있는 처리
        while(elapsedTime < GOODS_INCRASE_DURATION)
        {
            //매 프레임 경과 시간에 따라 연출 시간과의 비율을 계싼해서 현재 표시해야할 텍스트 값을 선출
            var currValue = Mathf.Lerp(currTextValue, destValue, elapsedTime / GOODS_INCRASE_DURATION);
            GoldAmountTxt.text = currTextValue.ToString("N0"); //산출한 수치를 ui텍스트 컴퍼넌트에 표시
            elapsedTime += Time.deltaTime; //경과 시간 증가
            yield return null;
        }
        GoldAmountTxt.text = destValue.ToString("N0"); // 연출이 끝나면 도달 수치를 텍스트 컴퍼넌트에 표시
    }
    //먼저 골드 재화가 변경 되었을 시 실행할 함수 작성 (획득 연출)
    private void OnUpdateGem(GemUpdateMsg gemUpdateMsg) // 매개변수로 메세지를 받는다.
                                                           //이렇게 함수를 선언해 주면 goldui인스턴스에서 goldupdate메세지를 받았을 때
                                                           //이 함수가 자동 실행 됨.
    {
        //먼저 유저 데이터를 가져온다.
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if (userGoodsData == null)
        {
            Logger.LogError("UserGoodsData does not exist.");
            return;
        }
        AudioManager.Instance.PlaySFX(SFX.ui_get); // 재화 획득 효과음 재생
        //만약 재화가 증가되었다면 증가 연출 처리
        if (gemUpdateMsg.isAdd)
        {
            if (m_GemIncreaseCo != null) //기존 연출이 있다면 취소
            {
                StopCoroutine(m_GemIncreaseCo);
            }
            m_GemIncreaseCo = StartCoroutine(IncreaseGemCo());

        }
        else
        {
            GemAmountTxt.text = userGoodsData.Gold.ToString("N0");
        }
    }
    private IEnumerator IncreaseGemCo()
    {
        //유저데이터를 가져온다.
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if (userGoodsData == null)
        {
            Logger.LogError("UserGoodsData does not exist.");
            yield break;
        }
        var amount = 10;
        for (int i = 0; i < amount; i++)
        {
            //반복문으로 지정한 수 만큼 인스턴스 생성
            //캔버스 하위에 위치
            var gemObj = Instantiate(Resources.Load("UI/GemMove", typeof(GameObject))) as GameObject;
            gemObj.transform.SetParent(UIManager.Instance.UICanvasTrs);
            //위치와 스케일 초기화
            gemObj.transform.localScale = Vector3.one;
            gemObj.transform.localPosition = Vector3.zero;
            //인스턴스에 연동된 GoodsMove 컴포넌트를 받아와 SetMove함수 호출
            gemObj.GetComponent<GoodsMove>().SetMove(i, GemIcon.transform.position);
        }
        yield return new WaitForSeconds(1f);
        //재화 텍스트 증가 연출부터 해주고
        //그 다음에 재화 오브젝트들이 생성되어 우측 상단의 UI쪽으로 이동하는 처리를 해줌.
        AudioManager.Instance.PlaySFX(SFX.ui_increase); // 재화 증가 효과음

        var elapsedTime = 0f; // 경과 시간을 저장할 변수
        //햔재 UI에 표시된 골드 수치를 가져온다.(쉼표 제거)
        var currTextValue = Convert.ToInt64(GemAmountTxt.text.Replace(",", ""));
        var destValue = userGoodsData.Gem; // 실제로 증가되어 표시되어야할 골드 수치

        //연출 시간 동안에 서서히 목표 수치로 도달할 수 있는 처리
        while (elapsedTime < GOODS_INCRASE_DURATION)
        {
            //매 프레임 경과 시간에 따라 연출 시간과의 비율을 계싼해서 현재 표시해야할 텍스트 값을 선출
            var currValue = Mathf.Lerp(currTextValue, destValue, elapsedTime / GOODS_INCRASE_DURATION);
            GemAmountTxt.text = currTextValue.ToString("N0"); //산출한 수치를 ui텍스트 컴퍼넌트에 표시
            elapsedTime += Time.deltaTime; //경과 시간 증가
            yield return null;
        }
        GemAmountTxt.text = destValue.ToString("N0"); // 연출이 끝나면 도달 수치를 텍스트 컴퍼넌트에 표시
    }
    //유저 재화 데이터를 불러와 보석과 골드 수량을 세팅해주는 함수
    public void SetValues()
    {
        //유저 재화데이터를 가져오겠음
        var userGoodData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        //null이면 에러로그
        if(userGoodData == null)
        {
            Logger.LogError("No user goods data");
            return;
        }

        //재화데이터가 정상이면 보석과 골드 수량 표시(NO: 1000단위 심표)
        GoldAmountTxt.text = userGoodData.Gold.ToString("N0");
        GemAmountTxt.text = userGoodData.Gold.ToString("N0");
    }
}
