using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SettingsUI : BaseUI
{
    public TextMeshProUGUI GameVersionTxt; //���� ������ ǥ������ �ؽ�Ʈ ���۳�Ʈ
    public GameObject SoundOnToggle; //���尡 On�϶� Ȱ��ȭ���� UI ������Ʈ
    public GameObject SoundOffTogle; //���尡 Off�϶� Ȱ��ȭ���� UI ������Ʈ
    //������å ���
    //�ؽ�Ʈ�� ������ �ش� ������Ʈ ��ũ�� �̵�(������ �׳� �ƹ� ��ũ)
    private const string PRIVACY_POLICY_URL = "https://www.naver.com";

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        SetGameVersion(); // ���� ���� ǥ�� �Լ�

        // UserDataManager ���� ���� �����͸� �����´�.
        var userSettingsData = UserDataManager.Instance.GetUserData<UserSettingData>();

        if (userSettingsData != null) //��ȿ�� �˻�
        {
            //sound�� on,off���� ���� soundToggle Ȱ��ȭ ��Ȱ��ȭ ó��
            SetSoundSetting(userSettingsData.Sound);
        }
    }
    //���� ���� ǥ�� �Լ�
    private void SetGameVersion()
    {
        GameVersionTxt.text = $"Version:{Application.version}";
    }
    private void SetSoundSetting(bool sound)
    {
        SoundOnToggle.SetActive(sound);
        SoundOffTogle.SetActive(!sound);
    }

   //SoundOnToggle�� ������ �� ���� ������ off //��۷� Ŭ�� ������ ����
   public void OnClickSoundOnToggle()
    {
        Logger.Log($"{GetType()}::OnClickSoundOnToggle");

        AudioManager.Instance.PlaySFX(SFX.ui_button_click); // UIŬ�� �� ���� �÷���
        //���� ������ ������
        var userSettingData = UserDataManager.Instance.GetUserData<UserSettingData>();
        //��ȿ�� �˻�
        if(userSettingData != null)
        {
            userSettingData.Sound = false;
            UserDataManager.Instance.SaveUserData();
            AudioManager.Instance.Mute();
            //�ٽ� SetSoundSetting�Լ��� ȣ���ؼ� ���� UI��Ҹ� ������Ʈ
            SetSoundSetting(userSettingData.Sound);
        }
    }
    //SoundOffToggle�� ������ �� ���� ������ on���� �ٲ��ִ� �Լ�
    public void OnClickSoundOffToggle()
    {
        Logger.Log($"{GetType()}::OnClickSoundOnToggle");

        AudioManager.Instance.PlaySFX(SFX.ui_button_click); // UIŬ�� �� ���� �÷���
        //���� ������ ������
        var userSettingData = UserDataManager.Instance.GetUserData<UserSettingData>();
        //��ȿ�� �˻�
        if (userSettingData != null)
        {
            userSettingData.Sound = true;
            UserDataManager.Instance.SaveUserData();
            AudioManager.Instance.UnMute();
            //�ٽ� SetSoundSetting�Լ��� ȣ���ؼ� ���� UI��Ҹ� ������Ʈ
            SetSoundSetting(userSettingData.Sound);
        }
    }
    public void OnClickPrivacyPolicyBtn()
    {
        Logger.Log($"{GetType()::OnClickPrivacyPolicyBtn}");

        AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        Application.OpenURL(PRIVACY_POLICY_URL);
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
