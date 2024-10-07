using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : SingletonBehaviour<InGameManager>
{
    public InGameUIController inGameUIController { get; private set; }
    public bool IsStageCleared { get; private set; } // 스테이지가 클리어되었는지 여부를 알 수 있는 변수
    //이 변수는 클리어 처리를 하고 있을 때 스테이지 클리어 체크를 하지 않도록 예외처리 하기 위해 참조할 변수
    public bool IsPause { get; private set; }// 일시정지 여부를 저장할 변수

    private int m_SelectedChapter; // 현재 플레이하려는 챕터 변수 선언
    private ChapterData m_CurrChapterData; //1)현재 플레이 중인 챕터의 챕터 데이터를 담을 변수
    private int m_CurrStage; //스테이지 변수 선언
    private const string STAGE_PATH = "Stages/"; //스테이지 프리팹을 로드할 디렉토리 상수를 선언
    private Transform m_StageTrs; // 스테이지오브젝트의 트랜스폼 변수를 선언
    

    private SpriteRenderer m_Bg; //백그라운드 스프라이트 랜더러 컴퍼넌트 변수를 선언

    private GameObject m_LoadedStage; // 현재 스테이지 오브젝트 인스턴스를 가지고 있을 게임오브젝트 변수

    

    protected override void Init()
    {
        m_IsDestroyOnLoad = true; // 인게임매니저는 인게임 씬을 벗어나면 삭제되어야함으로 true
        base.Init();

        InitVariables();
        LoadBg(); //기존에 LoadStage()에 합너에 처리한 배경부분을 분리하여 호출
        LoadStage();
        UIManager.Instance.Fade(Color.black, 1f, 0f, 0.5f, 0f, true);
    }

  


    // Start is called before the first frame update
    void Start()
    {
        //씬 내에서 InGameUIController 스크립트를 가지고 있는 오브젝트를 찾아서 대입
        inGameUIController = FindObjectOfType<InGameUIController>();
        if (!inGameUIController)
        {
            Logger.LogError("InGameUIController does not exits.");
            return;
        }
        inGameUIController.Init();
    }
    private void Update()
    {
        CheckStageClear();//원작자는 동전 개수를 계속 판단해야했음으로 업데이트에 적었음
    }

    private void CheckStageClear()
    {
        if (IsStageCleared)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearStage();
        }
    }

    private void ClearStage()
    {
        Logger.Log($"{GetType()}::ClearStage");
        IsStageCleared = true;
        StartCoroutine(ShowStageClearCo());
    }

    private IEnumerator ShowStageClearCo()
    {
        AudioManager.Instance.PlaySFX(SFX.stage_clear);

        var uiData = new BaseUIData();
        UIManager.Instance.OpenUI<StageClearUI>(uiData); //StageClearUI 화면을 호출

        yield return new WaitForSeconds(1f); // 1초대기

        //대기 후 stageclearui화면 닫기
        var stageClearUI = UIManager.Instance.GetActiveUI<StageClearUI>();
        if (stageClearUI)
        {
            stageClearUI.CloseUI();
        }
        //3) 현재 챕터에 존재하는 모든 스테이지를 클리어한 상황이라면 챕터 클리어 처리
        if (IsAllClear())
        {
            ClearChapter();
        }
        else
        {
            IsStageCleared = false;

            m_CurrStage++;

            LoadStage();
        }
        
    }
    //4) 현재 스테이지가 챕터 데이터의 토탈 스테이지 개수와 동일한지 비교
    private bool IsAllClear()
    {
        return m_CurrStage == m_CurrChapterData.TotalStages;
    }

    public void ClearChapter()
    {
        AudioManager.Instance.PlaySFX(SFX.chapter_clear); // 효과음 재생

        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>(); // UserPlayData를 가져옴
        if(userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exist.");
            return;
        }

        //먼저 챕터 클리어 화면을 보여줄 것
        var uiData = new ChapterClearUIData();
        uiData.chapter = m_SelectedChapter; // 챕터 값은 현재 챕터로 설정
        //보상 지급 여부는 현재 챕터가 유저 플레이 데이터의 MaxClearedChapter값보다 큰지를 비교
        uiData.earnReward = m_SelectedChapter > userPlayData.MaxClearedChapter;
        UIManager.Instance.OpenUI<ChapterClearUI>(uiData); //ChapterClearUI 오픈

        //그리고 현재 챕터가 아직 클리어하지 못한 챕터라면
        if(m_SelectedChapter > userPlayData.MaxClearedChapter)
        {
            userPlayData.MaxClearedChapter++; //1증가시켜주고
            userPlayData.SelectedChapter = userPlayData.MaxClearedChapter + 1;
            //선택한 챕터도 방금 해금한 다음 챕터로 설정해 줌
            //이는 로비로 나갔을 때 해금한 챕터가 선택한 챕터로 선택되도록 하기 위함.

            userPlayData.SaveData();
        }
    }

    private void InitVariables()
    {
        //현재 플레이할 챕터 변수를 가져와야겠고 아마도 lobbymanager에서 가져올거고 // lobbymanager가아니라 userdata였음
        //currstage가 어떤건지 확인해야할거고 이거는 여기서 처리할거고
        //stage path로 resource들어가서 로드할거고
        //stage오브젝트의 트랜스폼은 왜 쓰는거지? 얘는 잘 모르겠고
        //백그라운드 이미지는 chapter변수랑 currstage받아서 변경될거고


        //그럼 stagepath랑 백그라운드 이미지는 같이사용
        //아 스테이지 오브젝트를 치워뒀다가 불러오면서 위치 옮겨오는 용도겠네
        //그러면 initvariable은 init에서 하기 힘든 다양한 잡처리를 할거고
        //로드 스테이지가 따로있었네!
        Logger.Log($"{GetType()}::InitVariables");

        m_StageTrs = GameObject.Find("Stage").transform;
        m_Bg = GameObject.Find("Bg").GetComponent<SpriteRenderer>();
        m_CurrStage = 20; // 스테이지는 1로 초기화 인게임에 진입하면 무조건 첫번째 스테이지부터 시작하니까
        //실험용 임시 설정 다시 1로 초기화
        //유저 플레이 데이터를 가져와
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exis.");
            return;
        }
        //현재 챕터값을 대입
        m_SelectedChapter = userPlayData.SelectedChapter;

        //2)현재 선택한 챕터의 챕터 데이터를 가져와 m_CurrChapterData 변수에 대입
        m_CurrChapterData = DataTableManager.Instance.GetChapterData(m_SelectedChapter);
        if(m_CurrChapterData == null)
        {
            Logger.LogError($"ChapterData does not exist. Chapter:{m_CurrChapterData}");
            return;
        }
    }

    private void LoadBg()
    {
        var bgTexture = Resources.Load($"ChapterBG/Background_{m_SelectedChapter.ToString("D3")}") as Texture2D;
        //D3자릿수 맞추기 001 이런식
        if (bgTexture != null)
        {
            m_Bg.sprite = Sprite.Create(bgTexture, new Rect(0, 0, bgTexture.width, bgTexture.height), new Vector2(0.5f, 0.5f));
        }
    }

    private void LoadStage()
    {
        Logger.Log($"{GetType()}::LoadStage");
        //현재 챕터와 스테이지를 로그로 찍어준다
        Logger.Log($"Chapter:{m_SelectedChapter} Stage{m_CurrStage}");

        //챕터 백그라운드이미지를 로드해서 백그라운드 변수에 세팅
        if(m_LoadedStage) // 현재 로드된 스테이지 오브젝트가 있다면 삭제
        {
            Destroy(m_LoadedStage);
        }
        //스테이지 프리팹을 로드하여 GameObject 인스턴스를 생성해줌
        var stageObj = Instantiate(Resources.Load($"{STAGE_PATH}C{m_SelectedChapter}/C{m_SelectedChapter}_S{m_CurrStage}", typeof(GameObject))) as GameObject;
        //생성한 인스턴스를 스테이지 트랜스폼 하위로 위치
        stageObj.transform.SetParent(m_StageTrs);
        stageObj.transform.localScale = Vector3.one;
        stageObj.transform.localPosition = Vector3.zero;

        m_LoadedStage = stageObj;
    }
    public void PauseGame()
    {
        IsPause = true;
        //앞으로 구현할 인게임의 일시정지를 처리하는 코드
        //GameManager.Instance.Paused = true;
        //LevelManager.Instance.ToggleCharacterPause();
        Time.timeScale = 0f;
        //타임스케일을 0으로 바꾸는건 안좋음
        //유저의 인풋을 막거나 게임내의 타이머를 제어하는게 괜찮다
        //이건 유저의 데미지 받는방식에 따라 콜라이더나 캐릭터 컨트롤러의 사용을 fasle로 하여 무적판정을 만들어주고
        //1인 게임의 경우 몬스터와 유저의 움직임을 정지 이런식으로 해도될듯

    }

    public void ResumeGame()
    {
        IsPause = false;

        Time.timeScale = 1f;
    }
}
