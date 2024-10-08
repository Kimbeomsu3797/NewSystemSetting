using SuperMaxim.Messaging;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterClearUIData : BaseUIData
{
    public int chapter; // 어떤 챕터를 클리어 했는지
    public bool earnReward; // 보상을 받아야 하는지 여부를 저장하는 변수
    //이 변수는 이 챕터를 처음으로 클리어해서 보상을 지급해야 하는지
    // 아니면 한번 클리어 후 다시 클리어한 것이기 때문에 보상을 지급하지 말아야 하는지 판단하기 위한 변수
}
public class ChapterClearUI : BaseUI
{
    //보상 관련 UI요소들의 최상위 오브젝트 변수를 선언
    public GameObject Rewards;
    //보상으로 받을 보석 수량을 표시해줄 Text 컴퍼넌트
    public TextMeshProUGUI GemRewardAmountTxt;
    //보상으로 받을 골드 수량을 표시해줄 텍스트 컴퍼넌트
    public TextMeshProUGUI GoldRewardAmountTxt;
    //홈버튼
    public Button HomeBtn;
    //이펙트 오브젝트를 담을 배열
    public ParticleSystem[] ClearFX;
    //전용 데이터 클래스를 담을 변수
    private ChapterClearUIData m_ChapterClearUIData;

    //SetInfo 함수 오버라이딩
    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        m_ChapterClearUIData = uiData as ChapterClearUIData; // 매개변수로 받은 uiData를 ChapterClearUIData로 받아줌
        if(m_ChapterClearUIData == null)
        {
            Logger.LogError("ChapterClearUIData is invalid");
            return;
        }
        //보상 정보를 표시해주기 위해 해당 챕터에 해당하는 챕터 데이터를 가져온다.
        var chapterData = DataTableManager.Instance.GetChapterData(m_ChapterClearUIData.chapter);

        if(chapterData == null)
        {
            Logger.LogError($"ChapterData is invalid. Chapter:{m_ChapterClearUIData.chapter}");
            return;
        }

        //보상을 받을지 여부에 따라 리워드 오브젝트를 활성화 또는 비활성화 처리해줌.
        Rewards.SetActive(m_ChapterClearUIData.earnReward);
        //만약 보상을 받게 된다면 얼마큼 보상을 받게 될지 수량을 표시해 주겠음
        if (m_ChapterClearUIData.earnReward)
        {
            //챕터 데이터에 보상 정보가 명시되어 있음
            //따라서 그 챕터 데이터에서 보석과 골드 보상 정보를 가져와 표시
            GemRewardAmountTxt.text = chapterData.ChapterRewardGem.ToString("N0");
            GoldRewardAmountTxt.text = chapterData.ChapterRewardGold.ToString("N0");

            //실제로 보상을 지급해 줘야하는데 다음 파트에서 지금은 미 구현
            //TODO : Earn reward

            var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>(); // 유저데이터를 가져온다.
            if(userGoodsData == null)
            {
                Logger.LogError("UserGoodsData does not exist.");
                return;
            }
            //지급될 보상만큼 유저 데이터의 골드와 보석 값을 증가시킨다.
            userGoodsData.Gold += chapterData.ChapterRewardGold;
            userGoodsData.Gem += chapterData.ChapterRewardGem;
            userGoodsData.SaveData();

            //이제 유저가 보유한 재화가 변동되었다는 메세지를 발행
            var goldUpdateMsg = new GoldUpdateMsg();//골드 변동 메세지 인스턴스 생성
            goldUpdateMsg.isAdd = true;//변수값을 설정해주고
            Messenger.Default.Publish(goldUpdateMsg);//메세지 발행
            var userAchievementData = UserDataManager.Instance.GetUserData<UserAchievementData>();
            if(userAchievementData != null)
            {
                //업적 진행 처리, 업적 타입은 골드 획득으로 지정해 주고 업적 달성 수치는 획득한 골드양으로 해주겠음.
                userAchievementData.ProgressAchievement(AchievementType.CollectGold, chapterData.ChapterRewardGold);
            }
            //보석도 동일하게 처리
            var gemUpdateMsg = new GemUpdateMsg();
            gemUpdateMsg.isAdd = true;
            Messenger.Default.Publish(gemUpdateMsg);
        }
        //보상 여부에 따라 보상 UI가 활성화 되거나 비활성화 되기 때문에 그에 맞춰 홈 버튼 위치도 조정
        HomeBtn.GetComponent<RectTransform>().localPosition = new Vector3(0f, m_ChapterClearUIData.earnReward ? -250f : 50f, 0f);

        //이펙트 재생
        for(int i = 0; i < ClearFX.Length; i++)
        {
            ClearFX[i].Play();
        }

        
    }
    public void OnClickHomeBtn()
    {
        SceneLoader.Instance.LoadScene(SceneType.Lobby); // 로비로 씬 전환
        CloseUI(); // 화면 닫기
    }
}
