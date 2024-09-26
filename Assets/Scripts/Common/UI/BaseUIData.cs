using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseUIData
{
    //함수를 담을 수 있는 변수라고 생각
    //동일한 UI화면에 대해서도 어떤 상황에서는 A라는 기능을 실행해줘야하고
    //어떤 상황에서는 B라는 기능을 실행해줘야 할 때가 있음.
    //그렇기 때문에 UI화면 클래스 안에서 이런 OnShow나 OnClose를 정의하는 것보다
    //그 화면을 열겠다고 UI매니저를 호출할 때 어떤 행위를 해줘야 될지 정의해서
    //넘겨주는 것이 더 유연하게 원하는 기획 내용을 구현할 수 있다.

    public Action Onshow; //UI화면을 열었을 때 해주고 싶은 행위를 정의
    public Action OnClose; //UI화면을 닫으면서 실행해야 되는 기능 정의

    
    
}
public class BaseUI : MonoBehaviour
{
    //UI 열어줄때 재생할 애니메이션 변수
    public Animation m_UIOpenAnim;

    //화면을 열 때 실행해야할 기능
    //화면을 닫을 때 실행해야할 액션 변수 선언
    private Action m_OnShow;
    private Action m_OnClose;
    //이 변수 들은 화면을 열때 매개변수로 넘어온 UIData클래스에
    //정의된 OnShow와 OnColse 그대로 BaseUI 클래스에 있는 m_OnShow와 m_OnClose에 대하여
    //m_OnShow = uiData.OnShow; 이런식으로

    public virtual void Init(Transform anchor)
    {
        Logger.Log($"{GetType()} init.");

        m_OnShow = null;
        m_OnClose = null;

        transform.SetParent(anchor); // anchor : UI캔버스 컴포넌트의 트랜스폼

        var rectTransform = GetComponent<RectTransform>();
        if (!rectTransform)
        {
            Logger.LogError("UI does not have rectransform.");
            return;
        }

        //기본 값으로 전부 초기화
        rectTransform.localPosition = new Vector3(0f, 0f, 0f);
        rectTransform.localScale = new Vector3(1f, 1f, 1f);
        rectTransform.offsetMin = new Vector2(0, 0);
        rectTransform.offsetMax = new Vector2(0, 0);
    }

    //UI화면에 UI요소를 세팅해주는 함수
    public virtual void SetInfo(BaseUIData uiData)
    {
        Logger.Log($"{GetType()} set info.");

        m_OnShow = uiData.Onshow;
        m_OnClose = uiData.OnClose;
    }

    public virtual void ShowUI()
    {
        if (m_UIOpenAnim)
        {
            m_UIOpenAnim.Play();
        }

        m_OnShow?.Invoke(); // m_OnShow가 null이 아니라면 m_OnShow실행
        /* ? 키워드를 사용 한 예외처리
          _action?.Invoke(3); // if(_action !=null)를 ?키워드를 사용하여 null임을 체크*/
        m_OnShow = null; //실행 후 null값으로 초기화
    }
    public virtual void CloseUI(bool isCloseAll = false)
    {
        //isCloseAll : 씬을 전환하거나 할때 열려있는 화면을 전부 다 닫아줄 필요가 있을 때
        //true를 넘겨줘서 화면을 닫을 때 필요한 처리들을 다 무시하고 화면만 닫아주기 위해서 사용할것임

        if (!isCloseAll)
        {
            m_OnClose?.Invoke();
        }
        m_OnClose = null;

        UIManager.Instance.CloseUI(this); // 아직 작성하지 않았지만 
        //CloseUI에 바로 이 인스턴스를 매개변수로 넣어준다.
    }

    public virtual void OnClickCloseButton()
    {
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        CloseUI();
    }
}
