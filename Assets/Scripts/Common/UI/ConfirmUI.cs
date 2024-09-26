using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public enum ConfirmType
{
    OK,
    //단순히 알림성 팝업으로 특정 내용과 함께 확인 버튼만 보여지며
    //버튼을 누르면 닫히는 팝업
    OK_CANCEL,
    //유저가 어떤 행위를 하려는 것이 맞는지 재차 물어보며
    //그렇다면 확인 버튼을 눌러 그 행위를 실행하고
    //아니라면 취소 버튼을 눌러 취소하는 팝업
}

public class confirmUIData : BaseUIData
{
    public ConfirmType ConfirmType; // 팝업 유형을 구분하는 변수
    public string Titletxt; // 화면 제목에 들어갈 텍스트
    public string DescTxt; // 화면 중앙에 표시할 텍스트
    public string OKBtnTxt; //화면 버튼에 보여질 텍스트
    public Action OnClickOKBtn; // 확인 버튼을 누를 시에 처리
    public string CancleBtnTxt; //취소 버튼에 보여질 텍스트
    public Action OnClickCancelBtn; //취소 버튼을 눌렀을 시에 행위
}


public class ConfirmUI : BaseUI
{
    public TextMeshProUGUI TitleTxt = null; //화면 제목 텍스트 선언
    public TextMeshProUGUI DescTxt = null; // 화면 중앙에 설명 텍스트 선언
    public Button OKBtn = null; // 확인 버튼 선언
    public Button CancleBtn = null; // 취소 버튼 선언
    public TextMeshProUGUI OKBtnTxt = null; // 확인 버튼 텍스트 선언
    public TextMeshProUGUI CancleBtnTxt = null; // 취소 버튼 텍스트 선언

    //화면을 열 때 매개변수로 받은 UIData를 저장할 변수 선언
    private confirmUIData m_ConfirmUIData = null;
    //확인 버튼을 눌렀을 시 액션을 선언
    private Action m_OnClickOKBtn = null;
    //취소 버튼을 눌렀을 시 액션을 선언
    private Action m_OnclickCancelBtn = null;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        //매개변수로 받은 ui데이터를 저장
        m_ConfirmUIData = uiData as confirmUIData;

        TitleTxt.text = m_ConfirmUIData.Titletxt;
        DescTxt.text = m_ConfirmUIData.DescTxt;
        OKBtnTxt.text = m_ConfirmUIData.OKBtnTxt;
        m_OnClickOKBtn = m_ConfirmUIData.OnClickOKBtn;
        CancleBtnTxt.text = m_ConfirmUIData.CancleBtnTxt;
        m_OnclickCancelBtn = m_ConfirmUIData.OnClickCancelBtn;

        //ok버튼과 cancle버튼을 활성화
        //ConfirmType이 ok이면 ok버튼만, cancle이면 ok, cancle 버튼 둘다 활성화
        OKBtn.gameObject.SetActive(true);
        CancleBtn.gameObject.SetActive(m_ConfirmUIData.ConfirmType == ConfirmType.OK_CANCEL);
    }
    //확인 버튼 클릭 시 처리를 위한 함수
    public void OnClickOKbtn()
    {
        m_OnClickOKBtn?.Invoke(); // 널이아니면 액션 실행
        CloseUI();
    }
    //취소 버튼 클릭시 처리를 위한 함수
    public void OnClickCancleBtn()
    {
        m_OnclickCancelBtn?.Invoke();
        CloseUI();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
         
    }
}
