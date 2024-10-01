using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;
using UnityEngine.UI;


public class EquipmentUIData : BaseUIData
{
    public long SerialNumber;
    public int ItemId;
    public bool IsEquipped; // �ش� �������� �����Ǿ� �ִ��� ���θ� �����ϴ� ����
    //�� ������ ���߿� EquipmentUI(��UI)ȭ���� ������ �� �����Ǿ��ִ��� ���θ� �Ǵ��Ͽ�
    //UI��Ҹ� ó�����ִµ� ����ϰ�
    //�� �� UIȭ�鿡�� ����ó���� �ؾߵ��� Ż��ó���� �ؾ� ������ �Ǵ��ϴµ� �����ϵ��� �� ����.
}
public class EquipmentUI : BaseUI
{
    public Image ItemGradeBg;
    public Image ItemIcon;
    public TextMeshProUGUI ItemGradeTxt;
    public TextMeshProUGUI ItemNameTxt;
    //������ ������ ǥ������ �ؽ�Ʈ ���۳�Ʈ
    public TextMeshProUGUI AttackPowerAmountTxt;
    public TextMeshProUGUI DefenseAmountTxt;
    public TextMeshProUGUI EquipBtnTxt;

    //EquipmentUIData�� ���� ����
    public EquipmentUIData m_EquipmentUIData;

    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);

        m_EquipmentUIData = uiData as EquipmentUIData;
        if(m_EquipmentUIData == null)
        {
            Logger.LogError("m_EquipmentUIData is invalid.");
            return;
        }

        //�� ������ ǥ�����ֱ� ���ؼ� ������ ������ ���̺���
        //�ش� ������ ������ ������ �;� ��.
        var itemData = DataTableManager.Instance.GetItemData(m_EquipmentUIData.ItemId);
        if(itemData == null)
        {
            Logger.LogError($"Item data is invalid. ItemId{m_EquipmentUIData.ItemId}");
            return;
        }
        //������ID���� ������ ��� ����
        var itemGrade = (ItemGrade)((m_EquipmentUIData.ItemId / 1000) % 10);
        //������ ������ ��� ������ ������ ��� �̹����� �ε�
        var gradeBgTexture = Resources.Load<Texture2D>($"Textures/{itemGrade}");

        //�̹����� �� �ε�Ǿ����� ItemGradeBg ������Ʈ�� ����
        if(gradeBgTexture != null)
        {
            ItemGradeBg.sprite =
                Sprite.Create(gradeBgTexture, new Rect(0, 0, gradeBgTexture.width, gradeBgTexture.height)
                , new Vector2(1f, 1f));
        }
        //������ ����� �ؽ�Ʈ�ε� ǥ��
        ItemGradeTxt.text = itemGrade.ToString();
        //�� �ؽ�Ʈ ���۳�Ʈ�� �÷��� ������ ��� �÷��� �°� ����
        var hexColor = string.Empty;
        switch (itemGrade)
        {
            case ItemGrade.Common:
                hexColor = "#1AB3FF";
                break;
            case ItemGrade.Uncommon:
                hexColor = "#51C52C";
                break;
            case ItemGrade.Rare:
                hexColor = "#EA5AFF";
                break;
            case ItemGrade.Epic:
                hexColor = "#FF9900";
                break;
            case ItemGrade.Legendary:
                hexColor = "#F24949";
                break;
            default:
                break;
        }
        //�̷��� �÷����� ����������
        //�� �÷����� �÷� Ÿ�� ������ ��ȯ
        Color color;
        //ColorUtilityŬ������ ���ؼ� �÷����� ��ȯ
        //HTML ���� ���ڿ��� ��ȯ�Ϸ��� �õ��Ѵ�.
        //hexColor ���� �Ű������� �־��ָ� ��ȯ �������
        //out color�� ���
        //���������� ��ȯ�� �Ǿ����� true ���� ��ȯ ���۳�Ʈ�� �ش� �÷� ���� ����
        if(ColorUtility.TryParseHtmlString(hexColor, out color))
        {
            ItemGradeTxt.color = color;
        }

        //�����۾����ܸ��ҽ��� �ε��ؼ� ����
        //�̺κ��� �κ��丮UI���� ���� ó���� ������.
        StringBuilder sb = new StringBuilder(m_EquipmentUIData.ItemId.ToString());
        sb[1] = '1';
        var itemIconNmae = sb.ToString();

        var itemIconTexture = Resources.Load<Texture2D>($"Textures/{itemIconNmae}");
        if(itemIconTexture != null)
        {
            ItemIcon.sprite = Sprite.Create(itemIconTexture,
                new Rect(0, 0, itemIconTexture.width, itemIconTexture.height), new Vector2(1f, 1f));
        }

        ItemNameTxt.text = itemData.ItemName; // �����۸� ����
        AttackPowerAmountTxt.text = $"+{itemData.AttackPower}"; //���ݷ� ǥ��
        DefenseAmountTxt.text = $"+{itemData.Defense}"; // ���� ǥ��
        EquipBtnTxt.text = m_EquipmentUIData.IsEquipped ? "Unequip" : "Equip";
    }
    //�׸��� ����, Ż�� ��ư�� ������ �� ȣ�� �� �Լ�
    public void OnClickEquipBtn()
    {
        //UserInventoryData�� �����´�.
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        //null�̸� �����α�
        if(userInventoryData == null)
        {
            Logger.Log("UserInventoryData does not exist");
        }
        //�������̸� Ż��
        if (m_EquipmentUIData.IsEquipped)
        {
            userInventoryData.UnequipItem(m_EquipmentUIData.SerialNumber, m_EquipmentUIData.ItemId);
        }
        else
        {
            userInventoryData.EquipItem(m_EquipmentUIData.SerialNumber, m_EquipmentUIData.ItemId);
        }
        userInventoryData.SaveData(); //�����κ��丮 �����Ϳ� ��ȭ�� �������� ����
        //������ ���� �Ǵ� Ż�� ���� ���� �κ��丮UI�� �����������
        //UI�Ŵ����� ���� �����ִ� �κ��丮 UI�� ������ �޾ƿ�����.
        var inventoryUI = UIManager.Instance.GetActiveUI<InventoryUI>() as InventoryUI;

        if(inventoryUI != null)//�����ִ� UIȭ���� �ִٸ�
        {
            //�������ο� ���� UIó�� �Լ��� ȣ�����ְ���
            if (m_EquipmentUIData.IsEquipped)
            {
                inventoryUI.OnUnequipItem(m_EquipmentUIData.ItemId);
            }
            else
            {
                inventoryUI.OnEquipItem(m_EquipmentUIData.ItemId);
            }
        }
        CloseUI(); //������ �ڿ������� �帧�� ���� ���� �� UI�� �ݾ��ش�.
    }
    
}
