using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIController : MonoBehaviour
{
    
    public void Init()
    {
        UIManager.Instance.EnalbeStatsUI(true);

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
}
