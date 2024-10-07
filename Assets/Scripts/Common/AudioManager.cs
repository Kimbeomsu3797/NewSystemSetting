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
    ui_get, // ��ȭ ȹ�� ȿ����
    ui_increase, //��ȭ �ؽ�Ʈ ���� ȿ����
    COUNT
}
public class AudioManager : SingletonBehaviour<AudioManager>
{
    //�� ������Ʈ�� ������ ����
    public Transform BGMTrs;
    public Transform SFXTrs;
    //����� ������ �ε��� ���
    private const string AUDIO_PATH = "Audio";

    //��� BGM ����� ���ҽ��� ������ �����̳�
    private Dictionary<BGM, AudioSource> m_BGMPlayer = new Dictionary<BGM, AudioSource>();
    //BGM�����ؼ� ���� ����ϰ� �ִ� ������ҽ� ������Ʈ
    private AudioSource m_CurrBGMSource;
    //��� SFX ����� ���ҽ��� ������ �����̳�
    private Dictionary<SFX, AudioSource> m_SFXPlayer = new Dictionary<SFX, AudioSource>();

    protected override void Init()
    {
        base.Init();

        LoadBGMPlayer(); // ��� BGM�̴ϱ� �����ɲ���
        LoadSFXPlayer(); // ��� ������ ���� 1ȸ�� ������ �����Ŵ�
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
            //��ųʸ��� �ش� �̳�Ű������ ������ ����� �ҽ��� ����
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
            //��ųʸ��� �ش� �̳�Ű������ ������ ����� �ҽ��� ����
            m_SFXPlayer[(SFX)i] = newAudioSource;
        }

        
    }
    public void OnLoadUserData()
    {
        var userSettingsData = UserDataManager.Instance.GetUserData<UserSettingData>();
        if (userSettingsData != null) // ��ȿ�� �˻�
        {
            //flase�̸� mute
            if (!userSettingsData.Sound)
            {
                Mute();
            }
        }
    }
    //BGM �÷��� �Լ�
    public void PlayBGM(BGM bgm)
    {
        //���� ����ǰ� �ִ� BGM�ҽ��� �ִٸ�
        //����� ���߰� null������ �ʱ�ȭ
        if (m_CurrBGMSource)
        {
            m_CurrBGMSource.Stop();
            m_CurrBGMSource = null;
        }
        //����ϰ� ���� BGM�� �����ϴ��� Ȯ��
        //�������� ������ �����޼��� ����
        if (!m_BGMPlayer.ContainsKey(bgm))
        {
            Logger.LogError($"Invalid clip name.{bgm}");
            return;
        }
        //�����Ѵٸ� �ش� ������ҽ� ���۳�Ʈ�� ���������ְ� ���
        m_CurrBGMSource = m_BGMPlayer[bgm];
        m_CurrBGMSource.Play();
    }

    //BGM �Ͻ�����
    public void PauseBGM()
    {
        if (m_CurrBGMSource) m_CurrBGMSource.Pause();
    }
    //BGM �����
    public void ResumeBGM()
    {
        if (m_CurrBGMSource) m_CurrBGMSource.UnPause();
    }
    //BGM ����
    public void StopBGM()
    {
        if (m_CurrBGMSource) m_CurrBGMSource.Stop();
    }
    //SFX �÷���(BGM�� ���� ����)
    public void PlaySFX(SFX sfx)
    {
        if (!m_SFXPlayer.ContainsKey(sfx))
        {
            Logger.LogError($"Invalid clip name.{sfx}");
            return;
        }
        m_SFXPlayer[sfx].Play();
    }

    //ȿ������ ª�� �ð��� ����ǰ� �����Ƿ� ���� �Ͻ����� ���� �ʿ����.

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
