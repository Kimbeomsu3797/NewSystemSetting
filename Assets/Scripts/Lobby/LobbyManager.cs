using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    public LobbyUIController LobbyUIController { get; private set; } // private set; : �κ�ŴϹ��ñ�

    protected override void Init()
    {
        m_IsDestroyOnLoad = true; // �κ�Ŵ����� �ٸ� ������ ��ȯ�� �� ������ �� ����.
        
        base.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        //FindObjectOfType�� ���� �����ϴ� Ÿ���� ã�� ���� ���� ã�� �ν��Ͻ��� �Ѱ���.
        //(LobbyUIController�� �κ������ �ϳ��� �����ؾ��Ѵ�)
        LobbyUIController = FindObjectOfType<LobbyUIController>();

        if(!LobbyUIController) // ���࿡ �������� ������ �����α�
        {
            Logger.LogError("LobbyUIController does not exist.");
        }

        LobbyUIController.Init();
        AudioManager.Instance.PlayBGM(BGM.lobby); //�κ�� BGM ����
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
