using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoodsUI : MonoBehaviour
{
    //������ ������ ������ ��� ������ ǥ������ �ؽ�Ʈ ������Ʈ
    public TextMeshProUGUI GoldAmountTxt;
    public TextMeshProUGUI GemAmountTxt;

    //���� ��ȭ �����͸� �ҷ��� ������ ��� ������ �������ִ� �Լ�
    public void SetValues()
    {
        //���� ��ȭ�����͸� ����������
        var userGoodData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        //null�̸� �����α�
        if(userGoodData == null)
        {
            Logger.LogError("No user goods data");
            return;
        }

        //��ȭ�����Ͱ� �����̸� ������ ��� ���� ǥ��(NO: 1000���� ��ǥ)
        GoldAmountTxt.text = userGoodData.Gold.ToString("NO");
        GemAmountTxt.text = userGoodData.Gold.ToString("NO");
    }
}
