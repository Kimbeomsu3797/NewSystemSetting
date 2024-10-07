using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGM
{
    lobby,
    COUNT
}
public enum SFX
{
    chapter_clear,
    stage_clear,
    ui_button_click,
    ui_get, // 재화 획득 효과음
    ui_increase, //재화 텍스트 증가 효과음
    COUNT
}
public class AudioManager : SingletonBehaviour<AudioManager>
{
    //두 오브젝트에 변동할 변수
    public Transform BGMTrs;
    public Transform SFXTrs;
    //오디오 파일을 로드할 경로
    private const string AUDIO_PATH = "Audio";

    //모든 BGM 오디오 리소스를 저장할 컨테이너
    private Dictionary<BGM, AudioSource> m_BGMPlayer = new Dictionary<BGM, AudioSource>();
    //BGM관련해서 현재 재생하고 있는 오디오소스 컴포넌트
    private AudioSource m_CurrBGMSource;
    //모든 SFX 오디오 리소스를 저장할 컨테이너
    private Dictionary<SFX, AudioSource> m_SFXPlayer = new Dictionary<SFX, AudioSource>();

    protected override void Init()
    {
        base.Init();

        LoadBGMPlayer(); // 얘는 BGM이니까 루프될꺼다
        LoadSFXPlayer(); // 얘는 내용을 보니 1회성 음악이 많을거다
    }

    private void LoadBGMPlayer()
    {
        for(int i = 0; i< (int)BGM.COUNT; i++)
        {
            var audioName = ((BGM)i).ToString();
            var pathStr = $"{AUDIO_PATH}/{audioName}";
            var audioClip = Resources.Load(pathStr, typeof(AudioClip)) as AudioClip;
            if (!audioClip)
            {
                Logger.LogError($"{audioName} clip does not exist.");
                continue;
            }
            var newGO = new GameObject(audioName);
            var newAudioSource = newGO.AddComponent<AudioSource>();
            newAudioSource.clip = audioClip;
            newAudioSource.loop = true;
            newAudioSource.playOnAwake = false;
            newGO.transform.parent = BGMTrs;
            //딕셔너리에 해당 이넘키값으로 생성한 오디오 소스를 대입
            m_BGMPlayer[(BGM)i] = newAudioSource;
        }

        
    }

    private void LoadSFXPlayer()
    {
        for (int i = 0; i < (int)SFX.COUNT; i++)
        {
            var audioName = ((SFX)i).ToString();
            var pathStr = $"{AUDIO_PATH}/{audioName}";
            var audioClip = Resources.Load(pathStr, typeof(AudioClip)) as AudioClip;
            if (!audioClip)
            {
                Logger.LogError($"{audioName} clip does not exist.");
                continue;
            }
            var newGO = new GameObject(audioName);
            var newAudioSource = newGO.AddComponent<AudioSource>();
            newAudioSource.clip = audioClip;
            newAudioSource.loop = true;
            newAudioSource.playOnAwake = false;
            newGO.transform.parent = SFXTrs;
            //딕셔너리에 해당 이넘키값으로 생성한 오디오 소스를 대입
            m_SFXPlayer[(SFX)i] = newAudioSource;
        }

        
    }
    public void OnLoadUserData()
    {
        var userSettingsData = UserDataManager.Instance.GetUserData<UserSettingData>();
        if (userSettingsData != null) // 유효성 검사
        {
            //flase이면 mute
            if (!userSettingsData.Sound)
            {
                Mute();
            }
        }
    }
    //BGM 플레이 함수
    public void PlayBGM(BGM bgm)
    {
        //만약 재생되고 있는 BGM소스가 있다면
        //재생을 멈추고 null값으로 초기화
        if (m_CurrBGMSource)
        {
            m_CurrBGMSource.Stop();
            m_CurrBGMSource = null;
        }
        //재생하고 싶은 BGM이 존재하는지 확인
        //존재하지 않으면 에러메세지 생성
        if (!m_BGMPlayer.ContainsKey(bgm))
        {
            Logger.LogError($"Invalid clip name.{bgm}");
            return;
        }
        //존재한다면 해당 오디오소스 컴퍼넌트를 참조시켜주고 재생
        m_CurrBGMSource = m_BGMPlayer[bgm];
        m_CurrBGMSource.Play();
    }

    //BGM 일시정지
    public void PauseBGM()
    {
        if (m_CurrBGMSource) m_CurrBGMSource.Pause();
    }
    //BGM 재실행
    public void ResumeBGM()
    {
        if (m_CurrBGMSource) m_CurrBGMSource.UnPause();
    }
    //BGM 정지
    public void StopBGM()
    {
        if (m_CurrBGMSource) m_CurrBGMSource.Stop();
    }
    //SFX 플레이(BGM과 같은 원리)
    public void PlaySFX(SFX sfx)
    {
        if (!m_SFXPlayer.ContainsKey(sfx))
        {
            Logger.LogError($"Invalid clip name.{sfx}");
            return;
        }
        m_SFXPlayer[sfx].Play();
    }

    //효과음은 짧은 시간에 재생되고 끝나므로 따로 일시정지 등은 필요없음.

    //Mute
    public void Mute()
    {
        foreach(var audioSourceItem in m_BGMPlayer)
        {
            audioSourceItem.Value.volume = 0f;
        }
        foreach (var audioSourceItem in m_SFXPlayer)
        {
            audioSourceItem.Value.volume = 0f;
        }
    }
    //UnMute
    public void UnMute()
    {
        foreach (var audioSourceItem in m_BGMPlayer)
        {
            audioSourceItem.Value.volume = 1f;
        }
        foreach (var audioSourceItem in m_SFXPlayer)
        {
            audioSourceItem.Value.volume = 1f;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
