using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SuperMaxim.Messaging;

//�� ���� ������ ���� ��Ȳ�� �����ϴ� Ŭ������ ����
[Serializable]
public class UserAchievementProgressData
{
    public AchievementType AchievementType; // ���� Ÿ�� ����
    public int AchievementAmount; //���� �޼� ��ġ�� ����
    public bool IsAchieved; //���� �޼� ���θ� ����
    public bool IsRewardClaimed; // ���� ���� ���� ���� ����
}
public class AchievementProgressMsg
{

}
//�������� ���� ���� ��Ȳ �����͸� ����Ʈ�� ���·� �÷��̾��������� �����ϰ�
//�ε��� ���̱� ������ �����κ��丮������ �ε� ����ÿ� �ߴ� ��ó��
//���� Ŭ������ �ϳ� ����� �ְ���.
[Serializable]
public class UserAchievementProgressDataListWrapper
{
    //UserAchievementProgressData�� ��� ����Ʈ �ڷ� ������ ����
    public List<UserAchievementProgressData> AchievementProgressDataList;
}
//���� ���� ������ ������ �� ���� �޼����� ������ �� ����
//������ �� �޼��� Ŭ�������ϳ� ����

public class UserAchievementData : IUserData
{
    //UserAchievementProgressData�� ��� ����Ʈ �ڷ� ������ ����
    public List<UserAchievementProgressData> AchievementProgressDataList { get; set; } = new List<UserAchievementProgressData>();

    //������ ó�� ����Ǿ��� �� �׶� �����͸� ������ �� ����
    public bool LoadData()
    {
        Logger.Log($"{ GetType()}::LoadData");

        bool result = false;

        try
        {
            //�÷��̾� �������� �ִ� json��Ʈ�� �����͸� �ҷ���
            string achievementProgressDataListJson = PlayerPrefs.GetString("AchievementProgressDataList");

            if (!string.IsNullOrEmpty(achievementProgressDataListJson)) //�����Ͱ� ����� ���� �ִٸ�
            {
                //JsonUtillityŬ������ ���� WrapperŬ������ �ǽ��� ������
                UserAchievementProgressDataListWrapper achievementProgressDataListWrapper = JsonUtility.FromJson<UserAchievementProgressDataListWrapper>(achievementProgressDataListJson);
                //���� Ŭ������ ��� �����͸� �ٽ� UserAchievementDataŬ������ AchievementProgressDataList ����
                AchievementProgressDataList = achievementProgressDataListWrapper.AchievementProgressDataList;
                Logger.Log("AchievementProgressDataList"); //�ε��� �������� �α� ǥ��
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
            //����Ǿ� �ִ� �����͸� ���� Ŭ������ �Ű��ش�.
            achievementProgressDataListWrapper.AchievementProgressDataList = AchievementProgressDataList;
            //����Ŭ������ Json��Ʈ������ ��ȯ���ְ�

            //�÷��̾� �������� �ִ� json��Ʈ�� �����͸� �ҷ���
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
    //Ư�� ���� ���� �����͸� ã�Ƽ� ��ȯ�ϴ� �Լ�
    public UserAchievementProgressData GetUserAchievementProgressData(AchievementType achievementType)
    {
        return AchievementProgressDataList.Where(Item => Item.AchievementType == achievementType).FirstOrDefault();
    }

    //���� ������ ó���ϴ� �Լ� (�Ű�����: ����Ÿ��, �޼��� ���� ��ġ)
    public void ProgressAchievement(AchievementType achievementType, int achieveAmount)
    {
        //���������͸� ������
        var achievementData = DataTableManager.Instance.GetAchievementData(achievementType);
        if(achievementData == null)
        {
            Logger.LogError("AchievementData does not exist.");
            return;
        }
        //�����Ϸ��� ���� Ÿ�Կ� ���� ���� �����͵� �����´�.
        UserAchievementProgressData userAchievementProgressData = GetUserAchievementProgressData(achievementType);
        //���࿡ ����� ���� �����Ͱ� ���ٸ� ���� ������ �ֵ��� �ϰ���
        if(userAchievementProgressData == null)
        {
            //���� Ÿ���� �Ű������� ���� ���� Ÿ������ �������ְ�
            //����Ʈ �ڷ� ������ �߰��� ��
            userAchievementProgressData = new UserAchievementProgressData();
            userAchievementProgressData.AchievementType = achievementType;
            AchievementProgressDataList.Add(userAchievementProgressData);
        }
        //�޼� ���θ� Ȯ���ϰ� �޼��� ���� �ʾ����� ���� ���� ��ġ�� ����
        if (!userAchievementProgressData.IsAchieved)
        {
            userAchievementProgressData.AchievementAmount += achieveAmount; //�޼��� ��ġ��ŭ �޼���ġ�� ���������ش�.
            //���� ��ǥ �޼� ��ġ���� �ʰ��ؼ� �޼��ߴٸ� �޼� ��ǥġ�� ����
            if(userAchievementProgressData.AchievementAmount > achievementData.AchievementGoal)
            {
                userAchievementProgressData.AchievementAmount = achievementData.AchievementGoal;
            }
            //���� �޼� ��ġ�� ���� �޼� ��ǥ ��ġ�� �����ϴٸ�
            //������ �޼��ߴٰ� ����
            if(userAchievementProgressData.AchievementAmount == achievementData.AchievementGoal)
            {
                userAchievementProgressData.IsAchieved = true;
            }

            SaveData();//����

            //���� ���� ��Ȳ�� ���ŵǾ��ٴ� �޼��� ����
            //�� �޼����� ����UIȭ�鿡�� ����� ����.
            var achievementProgressMsg = new AchievementProgressMsg();
            Messenger.Default.Publish(achievementProgressMsg);
        }
    }
   
}

