using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

//업적 타입 정의
//업적 타입은 업적을 구분하는 유니크한 값임.
public enum AchievementType
{
    CollectGold,
    ClearChapter1,
    ClearChapter2,
    ClearChapter3,
}

public class AchievementData
{
    public AchievementType AchievementType; //업적 타입선언
    public string AchievementName; //업적 명
    public int AchievementGoal; // 업적달성목표수치
    public RewardType AchievementRewardType; // 업적 달성 시 보상 타입
    public int AchievementRewardAmount; //보상 수량
}
