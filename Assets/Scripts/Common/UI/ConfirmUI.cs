using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public enum ConfirmType
{
    OK,
    //�ܼ��� �˸��� �˾����� Ư�� ����� �Բ� Ȯ�� ��ư�� ��������
    //��ư�� ������ ������ �˾�
    OK_CANCEL,
    //������ � ������ �Ϸ��� ���� �´��� ���� �����
    //�׷��ٸ� Ȯ�� ��ư�� ���� �� ������ �����ϰ�
    //�ƴ϶�� ��� ��ư�� ���� ����ϴ� �˾�
}

public class confirmUIData : BaseUIData
{
    public ConfirmType ConfirmType; // �˾� ������ �����ϴ� ����
    public string Titletxt; // ȭ�� ���� �� �ؽ�Ʈ
    public string DescTxt; // ȭ�� �߾ӿ� ǥ���� �ؽ�Ʈ
    public string OKBtnTxt; //ȭ�� ��ư�� ������ �ؽ�Ʈ
    public Action OnClickOKBtn; // Ȯ�� ��ư�� ���� �ÿ� ó��
    public string CancleBtnTxt; //��� ��ư�� ������ �ؽ�Ʈ
    public Action OnClickCancelBtn; //��� ��ư�� ������ �ÿ� ����
}


public class ConfirmUI : BaseUI
{
    public TextMeshProUGUI TitleTxt = null; //ȭ�� ���� �ؽ�Ʈ ����
    public TextMeshProUGUI DescTxt = null; // ȭ�� �߾ӿ� ���� �ؽ�Ʈ ����
    public Button OKBtn = null; // Ȯ�� ��ư ����
    public Button CancleBtn = null; // ��� ��ư ����
    public TextMeshProUGUI OKBtnTxt = null; // Ȯ�� ��ư �ؽ�Ʈ ����
    public TextMeshProUGUI CancleBtnTxt = null; // ��� ��ư �ؽ�Ʈ ����

    //ȭ���� �� �� �Ű������� ���� UIData�� ������ ���� ����
    private confirmUIData m_ConfirmUIData = null;
    //Ȯ�� ��ư�� ������ �� �׼��� ����
    private Action m_OnClickOKBtn = null;
    //��� ��ư�� ������ �� �׼��� ����
    private Action m_OnclickCancelBtn = null;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        //�Ű������� ���� ui�����͸� ����
        m_ConfirmUIData = uiData as confirmUIData;

        TitleTxt.text = m_ConfirmUIData.Titletxt;
        DescTxt.text = m_ConfirmUIData.DescTxt;
        OKBtnTxt.text = m_ConfirmUIData.OKBtnTxt;
        m_OnClickOKBtn = m_ConfirmUIData.OnClickOKBtn;
        CancleBtnTxt.text = m_ConfirmUIData.CancleBtnTxt;
        m_OnclickCancelBtn = m_ConfirmUIData.OnClickCancelBtn;

        //ok��ư�� cancle��ư�� Ȱ��ȭ
        //ConfirmType�� ok�̸� ok��ư��, cancle�̸� ok, cancle ��ư �Ѵ� Ȱ��ȭ
        OKBtn.gameObject.SetActive(true);
        CancleBtn.gameObject.SetActive(m_ConfirmUIData.ConfirmType == ConfirmType.OK_CANCEL);
    }
    //Ȯ�� ��ư Ŭ�� �� ó���� ���� �Լ�
    public void OnClickOKbtn()
    {
        m_OnClickOKBtn?.Invoke(); // ���̾ƴϸ� �׼� ����
        CloseUI();
    }
    //��� ��ư Ŭ���� ó���� ���� �Լ�
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
