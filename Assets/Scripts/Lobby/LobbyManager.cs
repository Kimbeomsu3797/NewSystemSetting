using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    public LobbyUIController LobbyUIController { get; private set; } // private set; : �κ�ŴϹ��ñ�

    private bool m_IsLoadingInGame; //�ΰ��� �ε� �� ���θ� �� �� �ִ� ���� ����
    //�� ������ �����ϴ� ������ �̹� �ΰ��� ���� ��û�� �ߴµ�
    //start��ư�� ��������� ������ ���� ������ �ΰ��� ���� ��û�� �ϴ� ���� �����ϱ� ����.
    protected override void Init()
    {
        m_IsDestroyOnLoad = true; // �κ�Ŵ����� �ٸ� ������ ��ȯ�� �� ������ �� ����.
        m_IsLoadingInGame = false;
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

    public void StartInGame()
    {
        if (m_IsLoadingInGame)
        {
            return;
        }
        m_IsDestroyOnLoad = true;
        //������ ȭ������ 0.5�� �ȿ� ���̵� �ƿ�ó���� �ǵ��� �ϰ� ���̵� �ƿ��� ������ �ΰ����� �ε�
        UIManager.Instance.Fade(Color.black, 0f, 1f, 0.5f, 0f, false, ()=>
        {
            UIManager.Instance.CloseAllOpenUI();
            SceneLoader.Instance.LoadScene(SceneType.InGame);
        });

    }
}
