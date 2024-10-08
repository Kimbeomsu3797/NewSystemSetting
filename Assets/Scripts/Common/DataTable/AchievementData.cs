using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;

//���� Ÿ�� ����
//���� Ÿ���� ������ �����ϴ� ����ũ�� ����.
public enum AchievementType
{
    CollectGold,
    ClearChapter1,
    ClearChapter2,
    ClearChapter3,
}

public class AchievementData
{
    public AchievementType AchievementType; //���� Ÿ�Լ���
    public string AchievementName; //���� ��
    public int AchievementGoal; // �����޼���ǥ��ġ
    public RewardType AchievementRewardType; // ���� �޼� �� ���� Ÿ��
    public int AchievementRewardAmount; //���� ����
}
