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
    public AchievementType AchievementType;//업적 타입 선언
    public int AchieveAmount; //업적 달성 수치 선언
    public bool IsAchieved; //업적 달성 여부를 나타내는 변수 선언
    public bool IsRewardClaimed; //업적 보상 수령 여부를 나타내는 변수
}
//업적 아이템 클래스
public class AchievementItem : InfiniteScrollItem
{
    public GameObject AchievedBg; //달성한 업적의 배경 이미지가 되는 변수
    public GameObject UnAchievedBg; //달성하지 못한 업적의 배경 이미지가 될 변수
    public TextMeshProUGUI AchievementNameTxt; //업적명을 표시해줄 텍스트 컴포넌트 변수
    public Slider AchievementProgressSlider; //업적 진행 상황을 표시해줄 슬라이더 컴포넌트 변수
    public TextMeshProUGUI AchievementProgressTxt; //업적 진행 상황을 텍스트로 표시해줄 컴포넌트 변수
    public Image RewardIcon; //업적 달성 시 수령하게 될 보상 이미지 변수
    public TextMeshProUGUI RewardAmountTxt; //수령하게 될 보상의 수량을 표시하는 텍스트 컴포넌트 변수
    public Button ClaimBtn; //보상 수령 버튼 변수
    public Image ClaimBtnImg; //보상수령 버튼 이미지 변수
    public TextMeshProUGUI ClaimBtnTxt; //보상 수령 버튼 텍스트 컴퍼넌트 변수

    //업적 아이템 전용 데이터를 담을 변수
    private AchievementItemData m_AchievementItemData;

    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);
        //매개 변수로 받은 전용 데이터를 받아 옴
        m_AchievementItemData = scrollData as AchievementItemData;
        if(m_AchievementItemData == null)
        {
            Logger.LogError("m_AchievementItemData is invalid.");
            return;
        }
        //그리고 해당 업적에 대한 데이터를 데이터 테이플 매니저에서 가져오겠음.
        var achievementData = DataTableManager.Instance.GetAchievementData(m_AchievementItemData.AchievementType);
        if(achievementData == null)
        {
            Logger.LogError("AchievementData does not exist.");
            return;
        }
        //필요한 데이터를 모두 가져왔으면 본격적으로 UI요소를 세팅해 보겠음.
        //업적의 달성 여부에 따라 그에 맞는 백그라운드 이미지 컴퍼넌트를 활성화해주겠음.
        AchievedBg.SetActive(m_AchievementItemData.IsAchieved);
        UnAchievedBg.SetActive(!m_AchievementItemData.IsAchieved);
        AchievementNameTxt.text = achievementData.AchievementName;
        AchievementProgressSlider.value = (float)m_AchievementItemData.AchieveAmount / achievementData.AchievementGoal;
        AchievementProgressTxt.text = $"{m_AchievementItemData.AchieveAmount.ToString("N0")}/{achievementData.AchievementGoal.ToString("N0")}";
        RewardAmountTxt.text = achievementData.AchievementRewardAmount.ToString("N0");

        //보상 이미지를 보상 타입에 따라 세팅
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

        var rewardTexture = Resources.Load<Texture2D>($"Textures/{rewardTextureName}"); // 이미지 로드
        if(rewardTexture != null)
        {
            RewardIcon.sprite = Sprite.Create(rewardTexture, new Rect(0, 0, rewardTexture.width, rewardTexture.height), new Vector2(1f, 1f));
        }
        //보상 수령 버튼을 조건에 맞게 활성화 또는 비활성화 처리
        ClaimBtn.enabled = m_AchievementItemData.IsAchieved && !m_AchievementItemData.IsRewardClaimed;
        ClaimBtnImg.color = ClaimBtn.enabled ? Color.white : Color.grey;
        ClaimBtnTxt.color = ClaimBtn.enabled ? Color.white : Color.grey;
    }
    //보상 받기를 눌렀을 때 실행할 이벤트 함수를 작성
    public void OnClickClaimBtn()
    {
        //조건에 맞게 버튼을 비활성화 처리 했지만
        //혹시 모를 이유로 보상을 수령할 조건이 아니라면 예외처리를 해주겠음.
        if(!m_AchievementItemData.IsAchieved || m_AchievementItemData.IsRewardClaimed)
        {
            return;
        }
        //보상 지급 처리를 하기 위해 필요한 데이터를 가져옴
        //유저 업적 진행 데이터를 가져옴
        var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();
        if(userAchievementData == null)
        {
            Logger.LogError("UserAchievementData does not exist.");
            return;
        }
        //데이터 테이블에서 업적데이터도 가져옴
        var achievementData = DataTableManager.Instance.GetAchievementData(m_AchievementItemData.AchievementType);
        if(achievementData == null)
        {
            Logger.LogError("AchievementData does not exist.");
            return;
        }
        //현재 업적 타입에 맞는 유저 진행 데이터를 가져옴
        var userAchievedData = userAchievementData.GetUserAchievementProgressData(m_AchievementItemData.AchievementType);
        //필요한 데이터를 모두 가져왔으니 업적 보상 지급처리를 하겠음.
        if(userAchievedData != null)
        {
            var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
            if(userGoodsData != null)
            {
                //유저 업적 진행 데이터에서 보상 수령 여부를 true값으로 대입
                userAchievedData.IsRewardClaimed = true;
                userAchievementData.SaveData();//저장
                //현재 UI전용 데이터에도 보상 수령 여부를 true값으로 갱신
                m_AchievementItemData.IsRewardClaimed = true;
                //업적 보상 타입에 따라 보상을 지급
                switch (achievementData.AchievementRewardType)
                {
                    case GlobalDefine.RewardType.Gold: //보상이 골드라면
                        //재화 데이터의 골드 값에 보상 수량 만큼 값을 증가
                        userGoodsData.Gold += achievementData.AchievementRewardAmount;
                        //골드 수령 메세지 발행
                        var goldUpdateMsg = new GoldUpdateMsg();
                        goldUpdateMsg.isAdd = true;
                        Messenger.Default.Publish(goldUpdateMsg);
                        //업적 중에 골드를 획득하는 업적이 있음.
                        //그렇기 때문에 그 업적에 대해서 업적 진행 처리를 해 주어야 함.
                        //매개변수, 업적 타입, 획득한 골드 수량
                        userAchievementData.ProgressAchievement(AchievementType.CollectGold, achievementData.AchievementRewardAmount);
                        break;
                    case GlobalDefine.RewardType.Gem: //보상이 잼이라면
                        //재화 데이터의 골드 값에 보상 수량 만큼 값을 증가
                        userGoodsData.Gem += achievementData.AchievementRewardAmount;
                        //골드 수령 메세지 발행
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
