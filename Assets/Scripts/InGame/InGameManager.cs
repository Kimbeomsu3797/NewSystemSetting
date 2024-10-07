using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : SingletonBehaviour<InGameManager>
{
    public InGameUIController inGameUIController { get; private set; }
    public bool IsStageCleared { get; private set; } // ���������� Ŭ����Ǿ����� ���θ� �� �� �ִ� ����
    //�� ������ Ŭ���� ó���� �ϰ� ���� �� �������� Ŭ���� üũ�� ���� �ʵ��� ����ó�� �ϱ� ���� ������ ����
    public bool IsPause { get; private set; }// �Ͻ����� ���θ� ������ ����

    private int m_SelectedChapter; // ���� �÷����Ϸ��� é�� ���� ����
    private ChapterData m_CurrChapterData; //1)���� �÷��� ���� é���� é�� �����͸� ���� ����
    private int m_CurrStage; //�������� ���� ����
    private const string STAGE_PATH = "Stages/"; //�������� �������� �ε��� ���丮 ����� ����
    private Transform m_StageTrs; // ��������������Ʈ�� Ʈ������ ������ ����
    

    private SpriteRenderer m_Bg; //��׶��� ��������Ʈ ������ ���۳�Ʈ ������ ����

    private GameObject m_LoadedStage; // ���� �������� ������Ʈ �ν��Ͻ��� ������ ���� ���ӿ�����Ʈ ����

    

    protected override void Init()
    {
        m_IsDestroyOnLoad = true; // �ΰ��ӸŴ����� �ΰ��� ���� ����� �����Ǿ�������� true
        base.Init();

        InitVariables();
        LoadBg(); //������ LoadStage()�� �ճʿ� ó���� ���κ��� �и��Ͽ� ȣ��
        LoadStage();
        UIManager.Instance.Fade(Color.black, 1f, 0f, 0.5f, 0f, true);
    }

  


    // Start is called before the first frame update
    void Start()
    {
        //�� ������ InGameUIController ��ũ��Ʈ�� ������ �ִ� ������Ʈ�� ã�Ƽ� ����
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
        CheckStageClear();//�����ڴ� ���� ������ ��� �Ǵ��ؾ��������� ������Ʈ�� ������
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
        UIManager.Instance.OpenUI<StageClearUI>(uiData); //StageClearUI ȭ���� ȣ��

        yield return new WaitForSeconds(1f); // 1�ʴ��

        //��� �� stageclearuiȭ�� �ݱ�
        var stageClearUI = UIManager.Instance.GetActiveUI<StageClearUI>();
        if (stageClearUI)
        {
            stageClearUI.CloseUI();
        }
        //3) ���� é�Ϳ� �����ϴ� ��� ���������� Ŭ������ ��Ȳ�̶�� é�� Ŭ���� ó��
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
    //4) ���� ���������� é�� �������� ��Ż �������� ������ �������� ��
    private bool IsAllClear()
    {
        return m_CurrStage == m_CurrChapterData.TotalStages;
    }

    public void ClearChapter()
    {
        AudioManager.Instance.PlaySFX(SFX.chapter_clear); // ȿ���� ���

        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>(); // UserPlayData�� ������
        if(userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exist.");
            return;
        }

        //���� é�� Ŭ���� ȭ���� ������ ��
        var uiData = new ChapterClearUIData();
        uiData.chapter = m_SelectedChapter; // é�� ���� ���� é�ͷ� ����
        //���� ���� ���δ� ���� é�Ͱ� ���� �÷��� �������� MaxClearedChapter������ ū���� ��
        uiData.earnReward = m_SelectedChapter > userPlayData.MaxClearedChapter;
        UIManager.Instance.OpenUI<ChapterClearUI>(uiData); //ChapterClearUI ����

        //�׸��� ���� é�Ͱ� ���� Ŭ�������� ���� é�Ͷ��
        if(m_SelectedChapter > userPlayData.MaxClearedChapter)
        {
            userPlayData.MaxClearedChapter++; //1���������ְ�
            userPlayData.SelectedChapter = userPlayData.MaxClearedChapter + 1;
            //������ é�͵� ��� �ر��� ���� é�ͷ� ������ ��
            //�̴� �κ�� ������ �� �ر��� é�Ͱ� ������ é�ͷ� ���õǵ��� �ϱ� ����.

            userPlayData.SaveData();
        }
    }

    private void InitVariables()
    {
        //���� �÷����� é�� ������ �����;߰ڰ� �Ƹ��� lobbymanager���� �����ðŰ� // lobbymanager���ƴ϶� userdata����
        //currstage�� ����� Ȯ���ؾ��ҰŰ� �̰Ŵ� ���⼭ ó���ҰŰ�
        //stage path�� resource���� �ε��ҰŰ�
        //stage������Ʈ�� Ʈ�������� �� ���°���? ��� �� �𸣰ڰ�
        //��׶��� �̹����� chapter������ currstage�޾Ƽ� ����ɰŰ�


        //�׷� stagepath�� ��׶��� �̹����� ���̻��
        //�� �������� ������Ʈ�� ġ���״ٰ� �ҷ����鼭 ��ġ �Űܿ��� �뵵�ڳ�
        //�׷��� initvariable�� init���� �ϱ� ���� �پ��� ��ó���� �ҰŰ�
        //�ε� ���������� �����־���!
        Logger.Log($"{GetType()}::InitVariables");

        m_StageTrs = GameObject.Find("Stage").transform;
        m_Bg = GameObject.Find("Bg").GetComponent<SpriteRenderer>();
        m_CurrStage = 20; // ���������� 1�� �ʱ�ȭ �ΰ��ӿ� �����ϸ� ������ ù��° ������������ �����ϴϱ�
        //����� �ӽ� ���� �ٽ� 1�� �ʱ�ȭ
        //���� �÷��� �����͸� ������
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exis.");
            return;
        }
        //���� é�Ͱ��� ����
        m_SelectedChapter = userPlayData.SelectedChapter;

        //2)���� ������ é���� é�� �����͸� ������ m_CurrChapterData ������ ����
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
        //D3�ڸ��� ���߱� 001 �̷���
        if (bgTexture != null)
        {
            m_Bg.sprite = Sprite.Create(bgTexture, new Rect(0, 0, bgTexture.width, bgTexture.height), new Vector2(0.5f, 0.5f));
        }
    }

    private void LoadStage()
    {
        Logger.Log($"{GetType()}::LoadStage");
        //���� é�Ϳ� ���������� �α׷� ����ش�
        Logger.Log($"Chapter:{m_SelectedChapter} Stage{m_CurrStage}");

        //é�� ��׶����̹����� �ε��ؼ� ��׶��� ������ ����
        if(m_LoadedStage) // ���� �ε�� �������� ������Ʈ�� �ִٸ� ����
        {
            Destroy(m_LoadedStage);
        }
        //�������� �������� �ε��Ͽ� GameObject �ν��Ͻ��� ��������
        var stageObj = Instantiate(Resources.Load($"{STAGE_PATH}C{m_SelectedChapter}/C{m_SelectedChapter}_S{m_CurrStage}", typeof(GameObject))) as GameObject;
        //������ �ν��Ͻ��� �������� Ʈ������ ������ ��ġ
        stageObj.transform.SetParent(m_StageTrs);
        stageObj.transform.localScale = Vector3.one;
        stageObj.transform.localPosition = Vector3.zero;

        m_LoadedStage = stageObj;
    }
    public void PauseGame()
    {
        IsPause = true;
        //������ ������ �ΰ����� �Ͻ������� ó���ϴ� �ڵ�
        //GameManager.Instance.Paused = true;
        //LevelManager.Instance.ToggleCharacterPause();
        Time.timeScale = 0f;
        //Ÿ�ӽ������� 0���� �ٲٴ°� ������
        //������ ��ǲ�� ���ų� ���ӳ��� Ÿ�̸Ӹ� �����ϴ°� ������
        //�̰� ������ ������ �޴¹�Ŀ� ���� �ݶ��̴��� ĳ���� ��Ʈ�ѷ��� ����� fasle�� �Ͽ� ���������� ������ְ�
        //1�� ������ ��� ���Ϳ� ������ �������� ���� �̷������� �ص��ɵ�

    }

    public void ResumeGame()
    {
        IsPause = false;

        Time.timeScale = 1f;
    }
}
