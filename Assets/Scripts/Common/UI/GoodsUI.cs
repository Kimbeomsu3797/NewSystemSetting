using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SuperMaxim.Messaging;
using System;


//��ȭ ������ �߻��� �޼��� Ŭ������ ����
//��� ���� �޼���
public class GoldUpdateMsg
{
    public bool isAdd; //��ȭ�� ������ ������ ������ ������ ���θ� ��Ÿ���� ����
}
//���� ���� �޼���
public class GemUpdateMsg
{
    public bool isAdd;
}
//���� �ΰ��� �޼����� ����Ǿ��� �� �� GoodsUI Ŭ������
//�����ڰ� �Ǿ� �޼����� �ް� �׿� ���� ó��
public class GoodsUI : MonoBehaviour
{
    //������ ������ ������ ��� ������ ǥ������ �ؽ�Ʈ ������Ʈ
    public TextMeshProUGUI GoldAmountTxt;
    public TextMeshProUGUI GemAmountTxt;
    //���� �߰�
    public Image GoldIcon; // ��� ������ ��ġ�� �˱� ���� ��� ������ ���� ����
    public Image GemIcon; // �� ������ ��ġ�� �˱� ���� ��� ������ ���� ����

    //��� ���� ������ �ڷ�ƾ�� ���ؼ� ������ �ǵ� ���� ����
    //�ڷ�ƾ�� ������ �� �ִ� �ڷ�ƾ ���� ����
    private Coroutine m_GoldIncreaseCo;
    //�� ������ �����ϴ� ������ ���� ��ȭ ȹ���� ������ ������ ��û�Ǿ� �̹� ȹ�� ������ ������
    //�������ε� ���ο� ȹ�� ���� ��û ó���� ���� �Ǹ� ���� ȹ�� ������ ����ϰ� ���ο� ȹ�� ����� ����� ����.
    private Coroutine m_GemIncreaseCo;
    //���� ���⵵ ����

    //��ȭ �ؽ�Ʈ ���� ������ ���� �ȿ� ����Ǿ�� �ϴ��� �� �ð��� ��� �ִ� ���
    private const float GOODS_INCRASE_DURATION = 0.5f;

    //���� �ΰ��� �޼����� ����Ǿ��� �� �� GoodsUI�� �����ڰ� �Ǿ�
    //�� Ŭ������ Ȱ��ȭ �� ��
    //�� Ŭ������ ��ȭ ���� �޼��� �����ڷ� ���
    //���⿡ �� ������ �� �ν��Ͻ��� Ȱ��ȭ �Ǿ� ���� ����
    //��ȭ ���� �޼����� �޾� ó���ϱ� ���ϱ� ����
    private void OnEnable()
    {
        //�޼��� Ÿ���� ���
        //�׸��� �Ű������� �� �޼����� �޾��� �� ������ �Լ��� �Ѱ��ش�.
        Messenger.Default.Subscribe<GoldUpdateMsg>(OnUpdateGold);
        Messenger.Default.Subscribe<GemUpdateMsg>(OnUpdateGem);
    }
    //���� �ݴ�� ���� ����
    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<GoldUpdateMsg>(OnUpdateGold);
        Messenger.Default.Unsubscribe<GemUpdateMsg>(OnUpdateGem);

    }
    //���� ��� ��ȭ�� ���� �Ǿ��� �� ������ �Լ� �ۼ� (ȹ�� ����)
    private void OnUpdateGold(GoldUpdateMsg goldUpdateMsg) // �Ű������� �޼����� �޴´�.
        //�̷��� �Լ��� ������ �ָ� goldui�ν��Ͻ����� goldupdate�޼����� �޾��� ��
        //�� �Լ��� �ڵ� ���� ��.
    {
        //���� ���� �����͸� �����´�.
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if(userGoodsData == null)
        {
            Logger.LogError("UserGoodsData does not exist.");
            return;
        }
        AudioManager.Instance.PlaySFX(SFX.ui_get); // ��ȭ ȹ�� ȿ���� ���
        //���� ��ȭ�� �����Ǿ��ٸ� ���� ���� ó��
        if (goldUpdateMsg.isAdd)
        {
            if(m_GoldIncreaseCo != null) //���� ������ �ִٸ� ���
            {
                StopCoroutine(m_GoldIncreaseCo);
            }
            m_GoldIncreaseCo = StartCoroutine(IncreaseGoldCo());
            
        }
        else
        {
            GoldAmountTxt.text = userGoodsData.Gold.ToString("N0");
        }
    }
    //��ȭ ������ ����ϴ� �ڷ�ƾ
    private IEnumerator IncreaseGoldCo()
    {
        //���������͸� �����´�.
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if(userGoodsData == null)
        {
            Logger.LogError("UserGoodsData does not exist.");
            yield break;
        }
        var amount = 10;
        for (int i =0; i <amount; i++)
        {
            //�ݺ������� ������ �� ��ŭ �ν��Ͻ� ����
            //ĵ���� ������ ��ġ
            var goldObj = Instantiate(Resources.Load("UI/GoldMove", typeof(GameObject))) as GameObject;
            goldObj.transform.SetParent(UIManager.Instance.UICanvasTrs);
            //��ġ�� ������ �ʱ�ȭ
            goldObj.transform.localScale = Vector3.one;
            goldObj.transform.localPosition = Vector3.zero;
            //�ν��Ͻ��� ������ GoodsMove ������Ʈ�� �޾ƿ� SetMove�Լ� ȣ��
            goldObj.GetComponent<GoodsMove>().SetMove(i, GoldIcon.transform.position);
        }
        yield return new WaitForSeconds(1f);
        //��ȭ �ؽ�Ʈ ���� ������� ���ְ�
        //�� ������ ��ȭ ������Ʈ���� �����Ǿ� ���� ����� UI������ �̵��ϴ� ó���� ����.
        AudioManager.Instance.PlaySFX(SFX.ui_increase); // ��ȭ ���� ȿ����

        var elapsedTime = 0f; // ��� �ð��� ������ ����
        //�h�� UI�� ǥ�õ� ��� ��ġ�� �����´�.(��ǥ ����)
        var currTextValue = Convert.ToInt64(GoldAmountTxt.text.Replace(",", ""));
        var destValue = userGoodsData.Gold; // ������ �����Ǿ� ǥ�õǾ���� ��� ��ġ

        //���� �ð� ���ȿ� ������ ��ǥ ��ġ�� ������ �� �ִ� ó��
        while(elapsedTime < GOODS_INCRASE_DURATION)
        {
            //�� ������ ��� �ð��� ���� ���� �ð����� ������ ����ؼ� ���� ǥ���ؾ��� �ؽ�Ʈ ���� ����
            var currValue = Mathf.Lerp(currTextValue, destValue, elapsedTime / GOODS_INCRASE_DURATION);
            GoldAmountTxt.text = currTextValue.ToString("N0"); //������ ��ġ�� ui�ؽ�Ʈ ���۳�Ʈ�� ǥ��
            elapsedTime += Time.deltaTime; //��� �ð� ����
            yield return null;
        }
        GoldAmountTxt.text = destValue.ToString("N0"); // ������ ������ ���� ��ġ�� �ؽ�Ʈ ���۳�Ʈ�� ǥ��
    }
    //���� ��� ��ȭ�� ���� �Ǿ��� �� ������ �Լ� �ۼ� (ȹ�� ����)
    private void OnUpdateGem(GemUpdateMsg gemUpdateMsg) // �Ű������� �޼����� �޴´�.
                                                           //�̷��� �Լ��� ������ �ָ� goldui�ν��Ͻ����� goldupdate�޼����� �޾��� ��
                                                           //�� �Լ��� �ڵ� ���� ��.
    {
        //���� ���� �����͸� �����´�.
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if (userGoodsData == null)
        {
            Logger.LogError("UserGoodsData does not exist.");
            return;
        }
        AudioManager.Instance.PlaySFX(SFX.ui_get); // ��ȭ ȹ�� ȿ���� ���
        //���� ��ȭ�� �����Ǿ��ٸ� ���� ���� ó��
        if (gemUpdateMsg.isAdd)
        {
            if (m_GemIncreaseCo != null) //���� ������ �ִٸ� ���
            {
                StopCoroutine(m_GemIncreaseCo);
            }
            m_GemIncreaseCo = StartCoroutine(IncreaseGemCo());

        }
        else
        {
            GemAmountTxt.text = userGoodsData.Gold.ToString("N0");
        }
    }
    private IEnumerator IncreaseGemCo()
    {
        //���������͸� �����´�.
        var userGoodsData = UserDataManager.Instance.GetUserData<UserGoodsData>();
        if (userGoodsData == null)
        {
            Logger.LogError("UserGoodsData does not exist.");
            yield break;
        }
        var amount = 10;
        for (int i = 0; i < amount; i++)
        {
            //�ݺ������� ������ �� ��ŭ �ν��Ͻ� ����
            //ĵ���� ������ ��ġ
            var gemObj = Instantiate(Resources.Load("UI/GemMove", typeof(GameObject))) as GameObject;
            gemObj.transform.SetParent(UIManager.Instance.UICanvasTrs);
            //��ġ�� ������ �ʱ�ȭ
            gemObj.transform.localScale = Vector3.one;
            gemObj.transform.localPosition = Vector3.zero;
            //�ν��Ͻ��� ������ GoodsMove ������Ʈ�� �޾ƿ� SetMove�Լ� ȣ��
            gemObj.GetComponent<GoodsMove>().SetMove(i, GemIcon.transform.position);
        }
        yield return new WaitForSeconds(1f);
        //��ȭ �ؽ�Ʈ ���� ������� ���ְ�
        //�� ������ ��ȭ ������Ʈ���� �����Ǿ� ���� ����� UI������ �̵��ϴ� ó���� ����.
        AudioManager.Instance.PlaySFX(SFX.ui_increase); // ��ȭ ���� ȿ����

        var elapsedTime = 0f; // ��� �ð��� ������ ����
        //�h�� UI�� ǥ�õ� ��� ��ġ�� �����´�.(��ǥ ����)
        var currTextValue = Convert.ToInt64(GemAmountTxt.text.Replace(",", ""));
        var destValue = userGoodsData.Gem; // ������ �����Ǿ� ǥ�õǾ���� ��� ��ġ

        //���� �ð� ���ȿ� ������ ��ǥ ��ġ�� ������ �� �ִ� ó��
        while (elapsedTime < GOODS_INCRASE_DURATION)
        {
            //�� ������ ��� �ð��� ���� ���� �ð����� ������ ����ؼ� ���� ǥ���ؾ��� �ؽ�Ʈ ���� ����
            var currValue = Mathf.Lerp(currTextValue, destValue, elapsedTime / GOODS_INCRASE_DURATION);
            GemAmountTxt.text = currTextValue.ToString("N0"); //������ ��ġ�� ui�ؽ�Ʈ ���۳�Ʈ�� ǥ��
            elapsedTime += Time.deltaTime; //��� �ð� ����
            yield return null;
        }
        GemAmountTxt.text = destValue.ToString("N0"); // ������ ������ ���� ��ġ�� �ؽ�Ʈ ���۳�Ʈ�� ǥ��
    }
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
        GoldAmountTxt.text = userGoodData.Gold.ToString("N0");
        GemAmountTxt.text = userGoodData.Gold.ToString("N0");
    }
}
