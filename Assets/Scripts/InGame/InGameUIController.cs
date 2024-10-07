using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIController : MonoBehaviour
{
    public void Init()
    {
       
    }
    
    
    //������ �ۿ��� ��Ż������
    //MonoBehaviourŬ������ ����� Ŭ������� ȣ��Ǵ� �Լ�
    //�Ű������� bool�� �޾Ƽ� true�̸� �������� �ٽõ��ƿԴ� ��¶��̸�, false�̸� ������ ��Ż�ߴ� ��� ��
    //�ȵ���̵��� ��� ������ ó�������ص� �װ͵� ���� �÷ȴٶ�� focus�� true�� �ѹ� �����
    //ios�� ù ����� ȣ����� �ʴ´ٰ���
    //�׷��� �ȵ���̵� ���� ��� ���� ó���� ����� �Ҽ��� ����
    private void OnApplicationFocus(bool focus)
    {
        if(!focus)//������ ��Ż�ߴ�(���� ���ȴ�)��
        {
            if(!InGameManager.Instance.IsPause && !InGameManager.Instance.IsStageCleared)
            {
                var uiData = new BaseUIData();
                UIManager.Instance.OpenUI<PauseUI>(uiData);

                InGameManager.Instance.PauseGame();
                //����ó�� �ʿ� �������Ű���
            }

        }
    }
    private void Update()
    {
        //�ΰ����� �Ͻ����� �Ǿ����� Ȯ���ؼ� �Ͻ����� ���� �ʾ��� ���� ��ǲ�� ó���� �ֵ���
        if (!InGameManager.Instance.IsPause && !InGameManager.Instance.IsStageCleared)
        {
            HandleInput();
            //����ó�� �ʿ� �������Ű��� // pause�� �����ִٸ� �ȿ�����
        }
    }

    private void HandleInput()
    {
        //Ű���� ESC�� ����� ����̽� ���ư ��ǲ�� �޾ƿ�����
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            AudioManager.Instance.PlaySFX(SFX.ui_button_click); //ȿ���� ���

            var uiData = new BaseUIData();
            UIManager.Instance.OpenUI<PauseUI>(uiData); // UIȭ���� ������

            InGameManager.Instance.PauseGame(); // �Ͻ� ����
        }
    }

    //�Ͻ����� ��ư�� ������ �� ������ �Լ�
    public void OnClickPauseBtn()
    {
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<PauseUI>(uiData);

        InGameManager.Instance.PauseGame();
    }
}
