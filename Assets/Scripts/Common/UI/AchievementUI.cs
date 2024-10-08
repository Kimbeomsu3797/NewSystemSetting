using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gpm.Ui;
using SuperMaxim.Messaging;
using System;

public class AchievementUI : BaseUI
{
    public InfiniteScroll AchievementScrollList; // 스크롤 뷰 변수

    private void OnEnable()
    {
        //업적이 진행 되었을 때 발생하는메세지<AchievementProgressMsg>구독, 메세지를 받았을 때 실행할 함수(OnAchievementProgressed)
        Messenger.Default.Subscribe<AchievementProgressMsg>(OnAchievementProgressed);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<AchievementProgressMsg>(OnAchievementProgressed);//구독 해제
    }

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
        SetAchievementList(); // 업적 목록을 세팅하는 함수
        SortAchievementList(); //세팅한 업적 목록을 정렬하는 함수

    }
    private void SetAchievementList() 
    {
        //먼저 스크롤 뷰 이전에 생성된 아이템이 있을 수 있으니 스크롤 리스트를 청소 처리해줌.
        AchievementScrollList.Clear();

        //업적 목록 세팅
        //먼저 업적 데이터와 유저 업적 진행 데이터를 모두 가져오겠음.
        var achievementDataList = DataTableManager.Instance.GetAchievementDataList();
        var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();

        if(achievementDataList != null && userAchievementData != null)
        {
            //데이터가 모두 정상이라면 업적 데이터 목록을 순회하면서 업적 아이템 UI에 필요한 데이터를 생성해주겠음
            foreach(var achievement in achievementDataList)
            {
                var achievementItemData = new AchievementItemData();
                //생성한 achivementItemData의 변수값을 세팅해 주겠음.
                achievementItemData.AchievementType = achievement.AchievementType;

                //만약 유저 업적 진행데이터에도 해당 업적 데이터가 있다면
                //업적이 얼마나 진행되었는지 세팅을 해주고 업적 달성 여부와 업적 보상 수령 여부도 값을 대입해 줌
                var userAchieveData = userAchievementData.GetUserAchievementProgressData(achievement.AchievementType);
                if(userAchieveData != null)
                {
                    achievementItemData.AchieveAmount = userAchieveData.AchievementAmount;
                    achievementItemData.IsAchieved = userAchieveData.IsAchieved;
                    achievementItemData.IsRewardClaimed = userAchieveData.IsRewardClaimed;
                }
                //그리고 스크롤 뷰에 해당 데이터를 추가하여 업적 목록에 표시되도록 해줌.
                AchievementScrollList.InsertData(achievementItemData);
            }
        }
    }
    private void SortAchievementList()
    {
        //스크롤뷰에 SortDataList()를 호출하고 이 안에 람다식으로 정렬 로직을 작성.
        //비교 대상인 a,b에서 각 업적 아이템 데이터를 받아오겠음.
        AchievementScrollList.SortDataList((a, b) =>
        {
            var achievementA = a.data as AchievementItemData;
            var achievementB = b.data as AchievementItemData;
            //우선 순위
            //업적을 먼저 달성했지만 보상을 받지 않은 업적을 제일 상위로 정렬
            var AComp = achievementA.IsAchieved && !achievementA.IsRewardClaimed;
            var BComp = achievementB.IsAchieved && !achievementB.IsRewardClaimed;

            int compareResult = BComp.CompareTo(AComp);//CompareTo와 비교해서 위치가 같으면 0, (-)면 앞, (+)면 뒤로 배치
            //만약 조건이 동일하다면 달성하지 못한 업적을 달성한 업적 보다 더 상위에 정렬해 주겠음
            if (compareResult == 0)
            {
                compareResult = achievementA.IsAchieved.CompareTo(achievementB.IsAchieved);

                if (compareResult == 0)//이 조건마저 같다면 그냥 업적 타입에 따라 정렬
                {
                    compareResult = (achievementA.AchievementType).CompareTo(achievementB.AchievementType);
                }
            }
            return compareResult;
        });
    }
    private void OnAchievementProgressed(AchievementProgressMsg msg) //그냥 다시 정렬이 끝인듯
    {
        SetAchievementList(); // 업적 목록을 세팅하는 함수
        SortAchievementList(); //세팅한 업적 목록을 정렬하는 함수
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
