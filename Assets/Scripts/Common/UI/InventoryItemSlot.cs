using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

//InventoryItemSlot �ν��Ͻ� ������ ���� �ʿ��� ������ Ŭ����
//InfiniteScrollData ��� : ���Ǵ�Ƽ ��ũ�� ������Ʈ�� ����Ͽ� ��ũ�� �������� �����ϱ� ����(�׳� �Ẹ��)
public class InventoryItemSlotData : InfiniteScrollData
{
    //�ʿ��� �����ʹ� ���� ������ �����Ϳ� �����ϰ� �ø��� �ѹ��� ������ ���̵�
    public long SerialNumber;
    public int ItemId;
}

//InFiniteScrollItem ��� : InfiniteScrollŬ�������� ��ũ�� �����Ϳ� �����ϰ�
//��ũ�� ������ ���� ���������� InfiniteScrollItem �ν��Ͻ��� �����Ǳ� ����
public class InventoryItemSlot : InfiniteScrollItem
{

    public Image itemGradeBg; //��޿� ���� �̹����� ó������ ��׶����̹��� ������Ʈ
    public Image itemIcon; //�������� ���������� ���� ������ �̹����� ó������ �������̹���������Ʈ


    //InventoryItemSlotData�� ���� ����
    private InventoryItemSlotData m_InventoryItemSlotData;
    

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    //InfiniteScrollItem�� �ִ� UpdateData�� �������̵� �ؼ�
    //���� UI�� ���� ó���� �ϰ���
    //���Ǵ�Ƽ��ũ�ѵ����͸� �Ű������� �޾ƿͼ� ui�� ������ �� �Լ�
    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);
        //��ũ�� �����͸� �κ��丮�����۽��Ե����ͷ� ��ȯ�ؼ� �޴´�.
        m_InventoryItemSlotData = scrollData as InventoryItemSlotData;

        //�����Ͱ� null���� üũ�ϰ� null�̸� �α�
        if(m_InventoryItemSlotData == null)
        {
            Logger.LogError("m_InventoryItemSlotData is InvalidCastException.");
            return;
        }

        //������ ��޿� ���� ��׶��� �̹��� ó��
        //(�� �����̸�)������ID���� ��� ���ڸ� ���� ���� , �̰���(ItemGrade) �̳Ѱ����� ��ȯ�ؼ� �޾ƿ´�.
        var itemGrade = (ItemGrade)((m_InventoryItemSlotData.ItemId / 1000) % 10); //11001
        //�̷��� �޾ƿ� �̳Ѱ��� �״�� �̹��� ������ ����ϸ� ��.
        var gradeBgTexture = Resources.Load<Texture2D>($"Textures/{itemGrade}");

        //nullüũ �̻��� ������ ������ �׷��̵� ��׶��� �̹��� ������Ʊ�� �ش� �ؽ��ĸ� ������ ��
        if(gradeBgTexture != null)
        {
            itemGradeBg.sprite = Sprite.Create(gradeBgTexture, new Rect(0, 0, gradeBgTexture.width, gradeBgTexture.height), new Vector2(1f, 1f));
        }
        //�Ϲݵ���� ������ ID�� �̹����� ����� �ξ���. ������ID�� ��ް��� 1�� ġȯ�غ�����
        StringBuilder sb = new StringBuilder(m_InventoryItemSlotData.ItemId.ToString());
        sb[1] = '1'; // �ι�° �ڸ��� ������ 1�� �־���.
        var itemIconName = sb.ToString(); //�ٽ� ���ڿ��� ��ȯ
        //�̷��� ������ �̹��� ���� �ϼ�
        //�� �̹��� ������ �ؽ��ĸ� �ε�
        var itemIconTexture = Resources.Load<Texture2D>($"Textures/{itemIconName}");
        //nullüũ�ϰ� �̻��� ������ ������ ��� �̹����� ���������� ������ �̹��� ������Ʈ�� �ؽ�ó�� ������ ��
        if(itemIconTexture != null)
        {
            itemIcon.sprite = Sprite.Create(itemIconTexture, new Rect(0, 0, itemIconTexture.width, itemIconTexture.height), new Vector2(1f, 1f));

        }
    }
    public void OnClickInventoryItemSlot()
    {
        var uiData = new EquipmentUIData();
        uiData.SerialNumber = m_InventoryItemSlotData.SerialNumber;
        uiData.ItemId = m_InventoryItemSlotData.ItemId;
        UIManager.Instance.OpenUI<EquipmentUI>(uiData);
    }
}
