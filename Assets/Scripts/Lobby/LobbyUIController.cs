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

                //컨펌 UI를 열어달라고 UI매니저에 요청
                UIManager.Instance.OpenUI<ConfirmUI>(uiData);
            }
        }
        
    }
    //프로필 버튼을 클릭했을 시 이벤트 처리 함수
    public void OnClickProfileBtn()
    {
        Logger.Log($"{GetType()}::OnClickProfileBtn");

        //데이터 인스턴스를 BaseUIData클래스로 만들어주고
        var uiData = new BaseUIData();
        //UI매니저를 통해서 인벤토리UI를 열도록 함.
        UIManager.Instance.OpenUI<InventoryUI>(uiData);
    }
}
