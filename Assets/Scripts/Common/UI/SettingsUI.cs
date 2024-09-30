using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SettingsUI : BaseUI
{
    public TextMeshProUGUI GameVersionTxt; //게임 버젼을 표시해줄 텍스트 컴퍼넌트
    public GameObject SoundOnToggle; //사운드가 On일때 활성화해줄 UI 오브젝트
    public GameObject SoundOffTogle; //사운드가 Off일때 활성화해줄 UI 오브젝트
    //보안정책 명시
    //텍스트를 누르면 해당 웹사이트 링크로 이동(없으니 그냥 아무 링크)
    private const string PRIVACY_POLICY_URL = "https://www.naver.com";

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        SetGameVersion(); // 게임 버전 표시 함수

        // UserDataManager 유저 설정 데이터를 가져온다.
        var userSettingsData = UserDataManager.Instance.GetUserData<UserSettingData>();

        if (userSettingsData != null) //유효성 검사
        {
            //sound의 on,off값에 따라 soundToggle 활성화 비활성화 처리
            SetSoundSetting(userSettingsData.Sound);
        }
    }
    //게임 버전 표시 함수
    private void SetGameVersion()
    {
        GameVersionTxt.text = $"Version:{Application.version}";
    }
    private void SetSoundSetting(bool sound)
    {
        SoundOnToggle.SetActive(sound);
        SoundOffTogle.SetActive(!sound);
    }

   //SoundOnToggle을 눌렀을 때 사운드 설정을 off //토글로 클릭 해제로 설정
   public void OnClickSoundOnToggle()
    {
        Logger.Log($"{GetType()}::OnClickSoundOnToggle");

        AudioManager.Instance.PlaySFX(SFX.ui_button_click); // UI클릭 시 사운드 플레이
        //설정 데이터 가져옴
        var userSettingData = UserDataManager.Instance.GetUserData<UserSettingData>();
        //유효성 검사
        if(userSettingData != null)
        {
            userSettingData.Sound = false;
            UserDataManager.Instance.SaveUserData();
            AudioManager.Instance.Mute();
            //다시 SetSoundSetting함수를 호출해서 사운드 UI요소를 업데이트
            SetSoundSetting(userSettingData.Sound);
        }
    }
    //SoundOffToggle을 눌렀을 때 사운드 설정을 on으로 바꿔주는 함수
    public void OnClickSoundOffToggle()
    {
        Logger.Log($"{GetType()}::OnClickSoundOnToggle");

        AudioManager.Instance.PlaySFX(SFX.ui_button_click); // UI클릭 시 사운드 플레이
        //설정 데이터 가져옴
        var userSettingData = UserDataManager.Instance.GetUserData<UserSettingData>();
        //유효성 검사
        if (userSettingData != null)
        {
            userSettingData.Sound = true;
            UserDataManager.Instance.SaveUserData();
            AudioManager.Instance.UnMute();
            //다시 SetSoundSetting함수를 호출해서 사운드 UI요소를 업데이트
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
