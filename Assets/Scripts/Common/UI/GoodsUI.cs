using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoodsUI : MonoBehaviour
{
    //유저가 보유한 보석과 골드 수량을 표시해줄 텍스트 컴포넌트
    public TextMeshProUGUI GoldAmountTxt;
    public TextMeshProUGUI GemAmountTxt;

    //유저 재화 데이터를 불러와 보석과 골드 수량을 세팅해주는 함수
    public void SetValues()
    {
        //유저 재화데이터를 가져오겠음
        var userGoodData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        //null이면 에러로그
        if(userGoodData == null)
        {
            Logger.LogError("No user goods data");
            return;
        }

        //재화데이터가 정상이면 보석과 골드 수량 표시(NO: 1000단위 심표)
        GoldAmountTxt.text = userGoodData.Gold.ToString("NO");
        GemAmountTxt.text = userGoodData.Gold.ToString("NO");
    }
}
