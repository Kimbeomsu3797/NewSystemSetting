using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;
using SuperMaxim.Messaging;
using System;

public class AchievementUI : BaseUI
{
    public InfiniteScroll AchievementScrollList; // ��ũ�� �� ����

    private void OnEnable()
    {
        //������ ���� �Ǿ��� �� �߻��ϴ¸޼���<AchievementProgressMsg>����, �޼����� �޾��� �� ������ �Լ�(OnAchievementProgressed)
        Messenger.Default.Subscribe<AchievementProgressMsg>(OnAchievementProgressed);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<AchievementProgressMsg>(OnAchievementProgressed);//���� ����
    }

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
        SetAchievementList(); // ���� ����� �����ϴ� �Լ�
        SortAchievementList(); //������ ���� ����� �����ϴ� �Լ�

    }
    private void SetAchievementList() 
    {
        //���� ��ũ�� �� ������ ������ �������� ���� �� ������ ��ũ�� ����Ʈ�� û�� ó������.
        AchievementScrollList.Clear();

        //���� ��� ����
        //���� ���� �����Ϳ� ���� ���� ���� �����͸� ��� ����������.
        var achievementDataList = DataTableManager.Instance.GetAchievementDataList();
        var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();

        if(achievementDataList != null && userAchievementData != null)
        {
            //�����Ͱ� ��� �����̶�� ���� ������ ����� ��ȸ�ϸ鼭 ���� ������ UI�� �ʿ��� �����͸� �������ְ���
            foreach(var achievement in achievementDataList)
            {
                var achievementItemData = new AchievementItemData();
                //������ achivementItemData�� �������� ������ �ְ���.
                achievementItemData.AchievementType = achievement.AchievementType;

                //���� ���� ���� ���൥���Ϳ��� �ش� ���� �����Ͱ� �ִٸ�
                //������ �󸶳� ����Ǿ����� ������ ���ְ� ���� �޼� ���ο� ���� ���� ���� ���ε� ���� ������ ��
                var userAchieveData = userAchievementData.GetUserAchievementProgressData(achievement.AchievementType);
                if(userAchieveData != null)
                {
                    achievementItemData.AchieveAmount = userAchieveData.AchievementAmount;
                    achievementItemData.IsAchieved = userAchieveData.IsAchieved;
                    achievementItemData.IsRewardClaimed = userAchieveData.IsRewardClaimed;
                }
                //�׸��� ��ũ�� �信 �ش� �����͸� �߰��Ͽ� ���� ��Ͽ� ǥ�õǵ��� ����.
                AchievementScrollList.InsertData(achievementItemData);
            }
        }
    }
    private void SortAchievementList()
    {
        //��ũ�Ѻ信 SortDataList()�� ȣ���ϰ� �� �ȿ� ���ٽ����� ���� ������ �ۼ�.
        //�� ����� a,b���� �� ���� ������ �����͸� �޾ƿ�����.
        AchievementScrollList.SortDataList((a, b) =>
        {
            var achievementA = a.data as AchievementItemData;
            var achievementB = b.data as AchievementItemData;
            //�켱 ����
            //������ ���� �޼������� ������ ���� ���� ������ ���� ������ ����
            var AComp = achievementA.IsAchieved && !achievementA.IsRewardClaimed;
            var BComp = achievementB.IsAchieved && !achievementB.IsRewardClaimed;

            int compareResult = BComp.CompareTo(AComp);//CompareTo�� ���ؼ� ��ġ�� ������ 0, (-)�� ��, (+)�� �ڷ� ��ġ
            //���� ������ �����ϴٸ� �޼����� ���� ������ �޼��� ���� ���� �� ������ ������ �ְ���
            if (compareResult == 0)
            {
                compareResult = achievementA.IsAchieved.CompareTo(achievementB.IsAchieved);

                if (compareResult == 0)//�� ���Ǹ��� ���ٸ� �׳� ���� Ÿ�Կ� ���� ����
                {
                    compareResult = (achievementA.AchievementType).CompareTo(achievementB.AchievementType);
                }
            }
            return compareResult;
        });
    }
    private void OnAchievementProgressed(AchievementProgressMsg msg) //�׳� �ٽ� ������ ���ε�
    {
        SetAchievementList(); // ���� ����� �����ϴ� �Լ�
        SortAchievementList(); //������ ���� ����� �����ϴ� �Լ�
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
