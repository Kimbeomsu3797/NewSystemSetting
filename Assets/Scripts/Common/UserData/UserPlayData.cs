using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPlayData : IUserData
{
    //변수선언
    public int MaxClearedChapter { get; set; } // 유저가 최대로 클리어한 챕터

    //현재 유저가 선택중인 챕터는 따로 플레이어프랩스에 저장해주지 않겠음.
    //게임에 진입해서 데이터를 로드할 때 유저가 플레이 가능한 최고 챕터로 자동으로 설정
    //게임 진행 중에만 이 변수들을 관리
    public int SelectedChapter { get; set; } = 1; //유저가 선택중인 챕터
                                                  //초기값 설정
    public void SetDefaultData()
    {
        Logger.Log($"{GetType()}::SetDefaultData");

        MaxClearedChapter = 0;//테스트로 2로 변경
        SelectedChapter = 1;
    }
    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");
        bool result = false;
        try
        {
            //저장된 값을 로드
            MaxClearedChapter = PlayerPrefs.GetInt("MaxClearedChapter");
            //유저가 플레이 가능한 제일 높은 챕터로 현재 선택 중인 챕터를 선택해줌
            SelectedChapter = MaxClearedChapter + 1;

            result = true;

            Logger.Log($"MaxClearedChapter:{MaxClearedChapter}");
        }
        catch (System.Exception e)
        {

            Logger.Log($"Load failed. (" + e.Message + ")");
        }
        return result;
    }

    public bool SaveData()
    {
        Logger.Log($"{GetType()}::SaveData");
        bool result = false;
        try
        {
            PlayerPrefs.SetInt("MaxClearedChapter", MaxClearedChapter);
            PlayerPrefs.Save();

            result = true;

            Logger.Log($"MaxClearedChapter:{MaxClearedChapter}");
        }
        catch (System.Exception e)
        {

            Logger.Log($"Save failed. (" + e.Message + ")");
        }
        return result;
    }
    
    

}
