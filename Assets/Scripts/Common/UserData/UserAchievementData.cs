using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SuperMaxim.Messaging;

//각 업적 데이터 진행 상황을 저장하는 클래스를 선언
[Serializable]
public class UserAchievementProgressData
{
    public AchievementType AchievementType; // 업적 타입 선언
    public int AchievementAmount; //업적 달성 수치를 선언
    public bool IsAchieved; //업적 달성 여부를 선언
    public bool IsRewardClaimed; // 업적 보상 수령 여부 변수
}
public class AchievementProgressMsg
{

}
//여러개의 업적 진행 상황 데이터를 리스트의 형태로 플레이어프랩스에 저장하고
//로드할 것이기 때문에 유저인벤토리데이터 로드 저장시에 했던 것처럼
//래퍼 클래스를 하나 만들어 주겠음.
[Serializable]
public class UserAchievementProgressDataListWrapper
{
    //UserAchievementProgressData를 담는 리스트 자료 구조를 선언
    public List<UserAchievementProgressData> AchievementProgressDataList;
}
//업적 진행 사항을 갱신할 때 마다 메세지를 발행해 줄 것임
//발행해 줄 메세지 클래스도하나 선언

public class UserAchievementData : IUserData
{
    //UserAchievementProgressData를 담는 리스트 자료 구조를 선언
    public List<UserAchievementProgressData> AchievementProgressDataList { get; set; } = new List<UserAchievementProgressData>();

    //업적이 처음 진행되었을 때 그때 데이터를 생성해 줄 것임
    public bool LoadData()
    {
        Logger.Log($"{ GetType()}::LoadData");

        bool result = false;

        try
        {
            //플레이어 프랩스에 있는 json스트링 데이터를 불러옴
            string achievementProgressDataListJson = PlayerPrefs.GetString("AchievementProgressDataList");

            if (!string.IsNullOrEmpty(achievementProgressDataListJson)) //데이터가 저장된 것이 있다면
            {
                //JsonUtillity클래스를 통해 Wrapper클래스로 피싱해 오겠음
                UserAchievementProgressDataListWrapper achievementProgressDataListWrapper = JsonUtility.FromJson<UserAchievementProgressDataListWrapper>(achievementProgressDataListJson);
                //래퍼 클래스에 담긴 데이터를 다시 UserAchievementData클래스의 AchievementProgressDataList 대입
                AchievementProgressDataList = achievementProgressDataListWrapper.AchievementProgressDataList;
                Logger.Log("AchievementProgressDataList"); //로드한 데이터의 로그 표시
                foreach (var item in AchievementProgressDataList)
                {
                    Logger.Log($"AchievementType:{item.AchievementType} AchievementAmount:{item.AchievementAmount} IsAchieved:{item.IsAchieved} IsRewardClaimed:{item.IsRewardClaimed}");
                }
            }
            result = true;
        }
        catch (Exception e)
        {
            Logger.Log($"Load failed. (" + e.Message + ")");
        }
        return result;
    }

    public bool SaveData()
    {
        Logger.Log($"{ GetType()}::SaveData");

        bool result = false;

        try
        {

            UserAchievementProgressDataListWrapper achievementProgressDataListWrapper = new UserAchievementProgressDataListWrapper();
            //저장되어 있는 데이터를 래퍼 클래스로 옮겨준다.
            achievementProgressDataListWrapper.AchievementProgressDataList = AchievementProgressDataList;
            //래퍼클래스를 Json스트링으로 변환해주고

            //플레이어 프랩스에 있는 json스트링 데이터를 불러옴
            string achievementProgressDataListJson = JsonUtility.ToJson("AchievementProgressDataList");
            PlayerPrefs.SetString("AchievementProgressDataList", achievementProgressDataListJson);

            Logger.Log("AchievementProgressDataList");

            foreach (var item in AchievementProgressDataList)
            {
                Logger.Log($"AchievementType:{item.AchievementType} AchievementAmount:{item.AchievementAmount} IsAchieved:{item.IsAchieved} IsRewardClaimed:{item.IsRewardClaimed}");
            }
            PlayerPrefs.Save();
            result = true;
        }
        catch (Exception e)
        {
            Logger.Log($"Load failed. (" + e.Message + ")");
        }
        return result;
    }

    public void SetDefaultData()
    {
       
    }
    //특정 업적 짆애 데이터를 찾아서 반환하는 함수
    public UserAchievementProgressData GetUserAchievementProgressData(AchievementType achievementType)
    {
        return AchievementProgressDataList.Where(Item => Item.AchievementType == achievementType).FirstOrDefault();
    }

    //업적 진행을 처리하는 함수 (매개변수: 업적타입, 달성한 업적 수치)
    public void ProgressAchievement(AchievementType achievementType, int achieveAmount)
    {
        //업적데이터를 가져옴
        var achievementData = DataTableManager.Instance.GetAchievementData(achievementType);
        if(achievementData == null)
        {
            Logger.LogError("AchievementData does not exist.");
            return;
        }
        //갱신하려는 업적 타입에 대한 진행 데이터도 가져온다.
        UserAchievementProgressData userAchievementProgressData = GetUserAchievementProgressData(achievementType);
        //만약에 저장된 진행 데이터가 없다면 새로 생성해 주도록 하겠음
        if(userAchievementProgressData == null)
        {
            //업적 타입을 매개변수로 받은 업적 타입으로 대입해주고
            //리스트 자료 구조에 추가해 줌
            userAchievementProgressData = new UserAchievementProgressData();
            userAchievementProgressData.AchievementType = achievementType;
            AchievementProgressDataList.Add(userAchievementProgressData);
        }
        //달성 여부를 확인하고 달성이 되지 않았으면 업적 진행 수치를 갱신
        if (!userAchievementProgressData.IsAchieved)
        {
            userAchievementProgressData.AchievementAmount += achieveAmount; //달성한 수치만큼 달성수치를 증가시켜준다.
            //만약 목표 달성 수치보다 초과해서 달성했다면 달성 목표치로 대입
            if(userAchievementProgressData.AchievementAmount > achievementData.AchievementGoal)
            {
                userAchievementProgressData.AchievementAmount = achievementData.AchievementGoal;
            }
            //만약 달성 수치가 업적 달성 목표 수치와 동일하다면
            //업적을 달성했다고 저장
            if(userAchievementProgressData.AchievementAmount == achievementData.AchievementGoal)
            {
                userAchievementProgressData.IsAchieved = true;
            }

            SaveData();//저장

            //업적 진행 상황이 갱신되었다는 메세지 발행
            //이 메세지는 업적UI화면에서 사용할 것임.
            var achievementProgressMsg = new AchievementProgressMsg();
            Messenger.Default.Publish(achievementProgressMsg);
        }
    }
   
}

