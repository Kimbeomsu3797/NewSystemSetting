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
        SetCurrChapter(); //현재 선택 중인 챕터에 대한 UI 처리 함수 호출
    }
    //현재 선택 중인 챕터에 대한 UI 처리 함수
    public void SetCurrChapter()
    {
        //유저 플레이데이터를 가져옴
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exist");
            return;
        }
        //플레이데이터를 가져왔다면
        //현재 선택중인 챕터 번호를 가지고 해당 챕터 데이터를 가져옴
        var currChapterData = DataTableManager.Instance.GetChapterData(userPlayData.SelectedChapter);

        if(currChapterData == null)
        {
            Logger.LogError("CurrChapterData does not exits");
            return;
        }
        //해당 데이터가 정상적으로 존재한다면
        //챕터명을 표시해주고 챕터 이미지도 로드해서 세팅해준다.
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

    //챕터 이미지를 클릭했을 때 챕터 목록화면이 열리는 처리
    public void OnClickCurrChapter()
    {
        Logger.Log($"{GetType()}::OnClickCurrChapter");

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<ChapterListUI>(uiData);
    }
}
