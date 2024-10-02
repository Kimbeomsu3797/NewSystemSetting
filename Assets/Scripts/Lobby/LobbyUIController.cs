using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour
{
    public TextMeshProUGUI CurrChapterNameTxt;
    public RawImage CurrChapterBg;
    //EnableStatsUI => EnableGoodsUI
    public void Init()
    {
       
        UIManager.Instance.EnalbeGoodsUI(true);
        SetCurrChapter(); //���� ���� ���� é�Ϳ� ���� UI ó�� �Լ� ȣ��
    }
    //���� ���� ���� é�Ϳ� ���� UI ó�� �Լ�
    public void SetCurrChapter()
    {
        //���� �÷��̵����͸� ������
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exist");
            return;
        }
        //�÷��̵����͸� �����Դٸ�
        //���� �������� é�� ��ȣ�� ������ �ش� é�� �����͸� ������
        var currChapterData = DataTableManager.Instance.GetChapterData(userPlayData.SelectedChapter);

        if(currChapterData == null)
        {
            Logger.LogError("CurrChapterData does not exits");
            return;
        }
        //�ش� �����Ͱ� ���������� �����Ѵٸ�
        //é�͸��� ǥ�����ְ� é�� �̹����� �ε��ؼ� �������ش�.
        CurrChapterNameTxt.text = currChapterData.ChapterName;
        var bgTexture = Resources.Load($"ChapterBG/Background_{userPlayData.SelectedChapter.ToString("D3")}") as Texture2D; //001,002~
        if(bgTexture != null)
        {
            CurrChapterBg.texture = bgTexture;
        }
    }

    private void Update()
    {
        HandleInpt();
    }
    public void OnClickSettingsBtn()
    {
        Logger.Log($"{ GetType()}::OnClickSeetingsBtn");

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<SettingsUI>(uiData);
    }
    private void HandleInpt()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            AudioManager.Instance.PlaySFX(SFX.ui_button_click);

            var frontUI = UIManager.Instance.GetCurrentFrontUI();
            if (frontUI)
            {
                frontUI.CloseUI();
            }
            else
            {
                var uiData = new confirmUIData();
                uiData.ConfirmType = ConfirmType.OK_CANCEL;
                uiData.Titletxt = "Quit";
                uiData.DescTxt = "Do you want to quit game?";
                uiData.OKBtnTxt = "Quit";
                uiData.CancleBtnTxt = "Cancel";
                uiData.OnClickOKBtn = () =>
                {
                    Application.Quit();
                };

                //���� UI�� ����޶�� UI�Ŵ����� ��û
                UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            }
        }
        
    }
    //������ ��ư�� Ŭ������ �� �̺�Ʈ ó�� �Լ�
    public void OnClickProfileBtn()
    {
        Logger.Log($"{GetType()}::OnClickProfileBtn");

        //������ �ν��Ͻ��� BaseUIDataŬ������ ������ְ�
        var uiData = new BaseUIData();
        //UI�Ŵ����� ���ؼ� �κ��丮UI�� ������ ��.
        UIManager.Instance.OpenUI<InventoryUI>(uiData);
    }

    //é�� �̹����� Ŭ������ �� é�� ���ȭ���� ������ ó��
    public void OnClickCurrChapter()
    {
        Logger.Log($"{GetType()}::OnClickCurrChapter");

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<ChapterListUI>(uiData);
    }
}
