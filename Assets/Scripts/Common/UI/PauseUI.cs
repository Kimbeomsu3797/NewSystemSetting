using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : BaseUI
{
    //화면을 벗어나거나 일시정지버튼을 눌렀을 때
    public void OnClickResume()
    {
        InGameManager.Instance.ResumeGame();

        CloseUI();
    }
    public void OnClickHome()
    {
        SceneLoader.Instance.LoadScene(SceneType.Lobby);

        CloseUI();
    }
   
}
