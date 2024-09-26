using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : SingletonBehaviour<LobbyManager>
{
    public LobbyUIController LobbyUIController { get; private set; } // private set; : 로비매니뭐시기

    protected override void Init()
    {
        m_IsDestroyOnLoad = true; // 로비매니저는 다른 씬으로 전환할 때 삭제해 줄 것임.
        
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
}
