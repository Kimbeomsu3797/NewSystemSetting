using System.Collections;
using System.Collections.Generic;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Gpm.Ui;
using static GlobalDefine;


public class AchievementItemData : InfiniteScrollData
{
    public AchievementType AchievementType;//���� Ÿ�� ����
    public int AchieveAmount; //���� �޼� ��ġ ����
    public bool IsAchieved; //���� �޼� ���θ� ��Ÿ���� ���� ����
    public bool IsRewardClaimed; //���� ���� ���� ���θ� ��Ÿ���� ����
}
//���� ������ Ŭ����
public class AchievementItem : InfiniteScrollItem
{
    public GameObject AchievedBg; //�޼��� ������ ��� �̹����� �Ǵ� ����
    public GameObject UnAchievedBg; //�޼����� ���� ������ ��� �̹����� �� ����
    public TextMeshProUGUI AchievementNameTxt; //�������� ǥ������ �ؽ�Ʈ ������Ʈ ����
    public Slider AchievementProgressSlider; //���� ���� ��Ȳ�� ǥ������ �����̴� ������Ʈ ����
    public TextMeshProUGUI AchievementProgressTxt; //���� ���� ��Ȳ�� �ؽ�Ʈ�� ǥ������ ������Ʈ ����
    public Image RewardIcon; //���� �޼� �� �����ϰ� �� ���� �̹��� ����
    public TextMeshProUGUI RewardAmountTxt; //�����ϰ� �� ������ ������ ǥ���ϴ� �ؽ�Ʈ ������Ʈ ����
    public Button ClaimBtn; //���� ���� ��ư ����
    public Image ClaimBtnImg; //������� ��ư �̹��� ����
    public TextMeshProUGUI ClaimBtnTxt; //���� ���� ��ư �ؽ�Ʈ ���۳�Ʈ ����

    //���� ������ ���� �����͸� ���� ����
    private AchievementItemData m_AchievementItemData;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);
        //�Ű� ������ ���� ���� �����͸� �޾� ��
        m_AchievementItemData = scrollData as AchievementItemData;
        if(m_AchievementItemData == null)
        {
            Logger.LogError("m_AchievementItemData is invalid.");
            return;
        }
        //�׸��� �ش� ������ ���� �����͸� ������ ������ �Ŵ������� ����������.
        var achievementData = DataTableManager.Instance.GetAchievementData(m_AchievementItemData.AchievementType);
        if(achievementData == null)
        {
            Logger.LogError("AchievementData does not exist.");
            return;
        }
        //�ʿ��� �����͸� ��� ���������� ���������� UI��Ҹ� ������ ������.
        //������ �޼� ���ο� ���� �׿� �´� ��׶��� �̹��� ���۳�Ʈ�� Ȱ��ȭ���ְ���.
        AchievedBg.SetActive(m_AchievementItemData.IsAchieved);
        UnAchievedBg.SetActive(!m_AchievementItemData.IsAchieved);
        AchievementNameTxt.text = achievementData.AchievementName;
        AchievementProgressSlider.value = (float)m_AchievementItemData.AchieveAmount / achievementData.AchievementGoal;
        AchievementProgressTxt.text = $"{m_AchievementItemData.AchieveAmount.ToString("N0")}/{achievementData.AchievementGoal.ToString("N0")}";
        RewardAmountTxt.text = achievementData.AchievementRewardAmount.ToString("N0");

        //���� �̹����� ���� Ÿ�Կ� ���� ����
        var rewardTextureName = string.Empty;
        switch (achievementData.AchievementRewardType)
        {
            case GlobalDefine.RewardType.Gem:
                rewardTextureName = "IconGolds";
                break;
            case GlobalDefine.RewardType.Gold:
                rewardTextureName = "IconGems";
                break;
            default:
                break;
        }

        var rewardTexture = Resources.Load<Texture2D>($"Textures/{rewardTextureName}"); // �̹��� �ε�
        if(rewardTexture != null)
        {
            RewardIcon.sprite = Sprite.Create(rewardTexture, new Rect(0, 0, rewardTexture.width, rewardTexture.height), new Vector2(1f, 1f));
        }
        //���� ���� ��ư�� ���ǿ� �°� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ ó��
        ClaimBtn.enabled = m_AchievementItemData.IsAchieved && !m_AchievementItemData.IsRewardClaimed;
        ClaimBtnImg.color = ClaimBtn.enabled ? Color.white : Color.grey;
        ClaimBtnTxt.color = ClaimBtn.enabled ? Color.white : Color.grey;
    }
    //���� �ޱ⸦ ������ �� ������ �̺�Ʈ �Լ��� �ۼ�
    public void OnClickClaimBtn()
    {
        //���ǿ� �°� ��ư�� ��Ȱ��ȭ ó�� ������
        //Ȥ�� �� ������ ������ ������ ������ �ƴ϶�� ����ó���� ���ְ���.
        if(!m_AchievementItemData.IsAchieved || m_AchievementItemData.IsRewardClaimed)
        {
            return;
        }
        //���� ���� ó���� �ϱ� ���� �ʿ��� �����͸� ������
        //���� ���� ���� �����͸� ������
        var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();
        if(userAchievementData == null)
        {
            Logger.LogError("UserAchievementData does not exist.");
            return;
        }
        //������ ���̺��� ���������͵� ������
        var achievementData = DataTableManager.Instance.GetAchievementData(m_AchievementItemData.AchievementType);
        if(achievementData == null)
        {
            Logger.LogError("AchievementData does not exist.");
            return;
        }
        //���� ���� Ÿ�Կ� �´� ���� ���� �����͸� ������
        var userAchievedData = userAchievementData.GetUserAchievementProgressData(m_AchievementItemData.AchievementType);
        //�ʿ��� �����͸� ��� ���������� ���� ���� ����ó���� �ϰ���.
        if(userAchievedData != null)
        {
            var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
            if(userGoodsData != null)
            {
                //���� ���� ���� �����Ϳ��� ���� ���� ���θ� true������ ����
                userAchievedData.IsRewardClaimed = true;
                userAchievementData.SaveData();//����
                //���� UI���� �����Ϳ��� ���� ���� ���θ� true������ ����
                m_AchievementItemData.IsRewardClaimed = true;
                //���� ���� Ÿ�Կ� ���� ������ ����
                switch (achievementData.AchievementRewardType)
                {
                    case GlobalDefine.RewardType.Gold: //������ �����
                        //��ȭ �������� ��� ���� ���� ���� ��ŭ ���� ����
                        userGoodsData.Gold += achievementData.AchievementRewardAmount;
                        //��� ���� �޼��� ����
                        var goldUpdateMsg = new GoldUpdateMsg();
                        goldUpdateMsg.isAdd = true;
                        Messenger.Default.Publish(goldUpdateMsg);
                        //���� �߿� ��带 ȹ���ϴ� ������ ����.
                        //�׷��� ������ �� ������ ���ؼ� ���� ���� ó���� �� �־�� ��.
                        //�Ű�����, ���� Ÿ��, ȹ���� ��� ����
                        userAchievementData.ProgressAchievement(AchievementType.CollectGold, achievementData.AchievementRewardAmount);
                        break;
                    case GlobalDefine.RewardType.Gem: //������ ���̶��
                        //��ȭ �������� ��� ���� ���� ���� ��ŭ ���� ����
                        userGoodsData.Gem += achievementData.AchievementRewardAmount;
                        //��� ���� �޼��� ����
                        var gemUpdateMsg = new GemUpdateMsg();
                        gemUpdateMsg.isAdd = true;
                        Messenger.Default.Publish(gemUpdateMsg);
                        break;
                    default:
                        break;
                }
            }
        }
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
