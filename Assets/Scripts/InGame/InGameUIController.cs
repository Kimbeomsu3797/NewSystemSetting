using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIController : MonoBehaviour
{
    public void Init()
    {
       
    }
    
    
    //유저가 앱에서 이탈했을때
    //MonoBehaviour클래스를 상속한 클래스라면 호출되는 함수
    //매개변수로 bool을 받아서 true이면 게임으로 다시돌아왔다 라는뜻이며, false이면 게임을 이탈했다 라는 뜻
    //안드로이드의 경우 게임을 처음실행해도 그것도 앱을 올렸다라고 focus가 true로 한번 실행됨
    //ios는 첫 실행시 호출되지 않는다고함
    //그래서 안드로이드 같은 경우 예외 처리를 해줘야 할수도 있음
    private void OnApplicationFocus(bool focus)
    {
        if(!focus)//게임을 이탈했다(앱을 내렸다)면
        {
            if(!InGameManager.Instance.IsPause && !InGameManager.Instance.IsStageCleared)
            {
                var uiData = new BaseUIData();
                UIManager.Instance.OpenUI<PauseUI>(uiData);

                InGameManager.Instance.PauseGame();
                //예외처리 필요 오류날거같음
            }

        }
    }
    private void Update()
    {
        //인게임이 일시정지 되었는지 확인해서 일시정지 되지 않았을 때만 인풋을 처리해 주도록
        if (!InGameManager.Instance.IsPause && !InGameManager.Instance.IsStageCleared)
        {
            HandleInput();
            //예외처리 필요 오류날거같음 // pause가 열려있다면 안열리게
        }
    }

    private void HandleInput()
    {
        //키보드 ESC와 모바일 디바이스 백버튼 인풋을 받아오도록
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            AudioManager.Instance.PlaySFX(SFX.ui_button_click); //효과음 재생

            var uiData = new BaseUIData();
            UIManager.Instance.OpenUI<PauseUI>(uiData); // UI화면을 열어줌

            InGameManager.Instance.PauseGame(); // 일시 정지
        }
    }

    //일시정지 버튼을 눌렀을 때 실행할 함수
    public void OnClickPauseBtn()
    {
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<PauseUI>(uiData);

        InGameManager.Instance.PauseGame();
    }
}
