using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    public LobbyUIController LobbyUIController { get; private set; } // private set; : 로비매니뭐시기

    private bool m_IsLoadingInGame; //인게임 로딩 중 여부를 알 수 있는 변수 선언
    //이 변수를 선언하는 이유는 이미 인게임 진입 요청을 했는데
    //start버튼을 계속적으로 누르는 등의 행위로 인게임 진입 요청을 하는 것을 방지하기 위함.
    protected override void Init()
    {
        m_IsDestroyOnLoad = true; // 로비매니저는 다른 씬으로 전환할 때 삭제해 줄 것임.
        m_IsLoadingInGame = false;
        base.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        //FindObjectOfType은 씬에 존재하는 타입을 찾아 가장 먼저 찾은 인스턴스를 넘겨줌.
        //(LobbyUIController는 로비씬에서 하나만 존재해야한다)
        LobbyUIController = FindObjectOfType<LobbyUIController>();

        if(!LobbyUIController) // 만약에 존재하지 않으면 에러로그
        {
            Logger.LogError("LobbyUIController does not exist.");
        }

        LobbyUIController.Init();
        AudioManager.Instance.PlayBGM(BGM.lobby); //로비씬 BGM 실행
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
        //검은색 화면으로 0.5초 안에 페이드 아웃처리가 되도록 하고 페이드 아웃이 끝나면 인게임을 로딩
        UIManager.Instance.Fade(Color.black, 0f, 1f, 0.5f, 0f, false, ()=>
        {
            UIManager.Instance.CloseAllOpenUI();
            SceneLoader.Instance.LoadScene(SceneType.InGame);
        });

    }
}
