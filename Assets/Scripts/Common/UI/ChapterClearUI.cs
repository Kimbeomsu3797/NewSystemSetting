using SuperMaxim.Messaging;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterClearUIData : BaseUIData
{
    public int chapter; // � é�͸� Ŭ���� �ߴ���
    public bool earnReward; // ������ �޾ƾ� �ϴ��� ���θ� �����ϴ� ����
    //�� ������ �� é�͸� ó������ Ŭ�����ؼ� ������ �����ؾ� �ϴ���
    // �ƴϸ� �ѹ� Ŭ���� �� �ٽ� Ŭ������ ���̱� ������ ������ �������� ���ƾ� �ϴ��� �Ǵ��ϱ� ���� ����
}
public class ChapterClearUI : BaseUI
{
    //���� ���� UI��ҵ��� �ֻ��� ������Ʈ ������ ����
    public GameObject Rewards;
    //�������� ���� ���� ������ ǥ������ Text ���۳�Ʈ
    public TextMeshProUGUI GemRewardAmountTxt;
    //�������� ���� ��� ������ ǥ������ �ؽ�Ʈ ���۳�Ʈ
    public TextMeshProUGUI GoldRewardAmountTxt;
    //Ȩ��ư
    public Button HomeBtn;
    //����Ʈ ������Ʈ�� ���� �迭
    public ParticleSystem[] ClearFX;
    //���� ������ Ŭ������ ���� ����
    private ChapterClearUIData m_ChapterClearUIData;

    //SetInfo �Լ� �������̵�
    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        m_ChapterClearUIData = uiData as ChapterClearUIData; // �Ű������� ���� uiData�� ChapterClearUIData�� �޾���
        if(m_ChapterClearUIData == null)
        {
            Logger.LogError("ChapterClearUIData is invalid");
            return;
        }
        //���� ������ ǥ�����ֱ� ���� �ش� é�Ϳ� �ش��ϴ� é�� �����͸� �����´�.
        var chapterData = DataTableManager.Instance.GetChapterData(m_ChapterClearUIData.chapter);

        if(chapterData == null)
        {
            Logger.LogError($"ChapterData is invalid. Chapter:{m_ChapterClearUIData.chapter}");
            return;
        }

        //������ ������ ���ο� ���� ������ ������Ʈ�� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ ó������.
        Rewards.SetActive(m_ChapterClearUIData.earnReward);
        //���� ������ �ް� �ȴٸ� ��ŭ ������ �ް� ���� ������ ǥ���� �ְ���
        if (m_ChapterClearUIData.earnReward)
        {
            //é�� �����Ϳ� ���� ������ ��õǾ� ����
            //���� �� é�� �����Ϳ��� ������ ��� ���� ������ ������ ǥ��
            GemRewardAmountTxt.text = chapterData.ChapterRewardGem.ToString("N0");
            GoldRewardAmountTxt.text = chapterData.ChapterRewardGold.ToString("N0");

            //������ ������ ������ ����ϴµ� ���� ��Ʈ���� ������ �� ����
            //TODO : Earn reward

            var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>(); // ���������͸� �����´�.
            if(userGoodsData == null)
            {
                Logger.LogError("UserGoodsData does not exist.");
                return;
            }
            //���޵� ����ŭ ���� �������� ���� ���� ���� ������Ų��.
            userGoodsData.Gold += chapterData.ChapterRewardGold;
            userGoodsData.Gem += chapterData.ChapterRewardGem;
            userGoodsData.SaveData();

            //���� ������ ������ ��ȭ�� �����Ǿ��ٴ� �޼����� ����
            var goldUpdateMsg = new GoldUpdateMsg();//��� ���� �޼��� �ν��Ͻ� ����
            goldUpdateMsg.isAdd = true;//�������� �������ְ�
            Messenger.Default.Publish(goldUpdateMsg);//�޼��� ����
            var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();
            if(userAchievementData != null)
            {
                //���� ���� ó��, ���� Ÿ���� ��� ȹ������ ������ �ְ� ���� �޼� ��ġ�� ȹ���� �������� ���ְ���.
                userAchievementData.ProgressAchievement(AchievementType.CollectGold, chapterData.ChapterRewardGold);
            }
            //������ �����ϰ� ó��
            var gemUpdateMsg = new GemUpdateMsg();
            gemUpdateMsg.isAdd = true;
            Messenger.Default.Publish(gemUpdateMsg);
        }
        //���� ���ο� ���� ���� UI�� Ȱ��ȭ �ǰų� ��Ȱ��ȭ �Ǳ� ������ �׿� ���� Ȩ ��ư ��ġ�� ����
        HomeBtn.GetComponent<RectTransform>().localPosition = new Vector3(0f, m_ChapterClearUIData.earnReward ? -250f : 50f, 0f);

        //����Ʈ ���
        for(int i = 0; i < ClearFX.Length; i++)
        {
            ClearFX[i].Play();
        }

        
    }
    public void OnClickHomeBtn()
    {
        SceneLoader.Instance.LoadScene(SceneType.Lobby); // �κ�� �� ��ȯ
        CloseUI(); // ȭ�� �ݱ�
    }
}
