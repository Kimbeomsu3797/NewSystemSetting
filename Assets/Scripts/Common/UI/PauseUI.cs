using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : BaseUI
{
    //ȭ���� ����ų� �Ͻ�������ư�� ������ ��
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
