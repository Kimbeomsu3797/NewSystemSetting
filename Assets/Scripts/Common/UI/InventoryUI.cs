using Gpm.Ui;
using TMPro;


//���� � ���ǿ� ���� ������ �� �������� ��Ÿ���� �̳� ���� ����
public enum InventorySortType
{
    ItemGrade, //���
    ItemType, //����
}
public class InventoryUI : BaseUI
{
    //���� �� ���� ���� ���۳�Ʈ ������ ����
    public EquippedItemSlot WeaponSlot;
    public EquippedItemSlot ShieldSlot;
    public EquippedItemSlot ChestArmorSlot;
    public EquippedItemSlot BootsSlot;
    public EquippedItemSlot GlovesSlot;
    public EquippedItemSlot AccessorySlot;

    //�κ��丮 UI ���� 
    public InfiniteScroll InventoryScrollList;
    //���� � �������� ���ĵǾ� �ִ��� ǥ������ �ؽ�Ʈ ���۳�Ʈ ����
    public TextMeshProUGUI SortBtnTxt;
    //���� ���� ����� ���� �ִ� ���� ����, �ʱⰪ�� �������.
    private InventorySortType m_InventorySortType = InventorySortType.ItemGrade;

    public TextMeshProUGUI AttackPowerAmountTxt;
    public TextMeshProUGUI DefenseAmountTxt;
    public override void SetInfo(BaseUIData uiData)
    {
        //��ũ�Ѻ� ó���� ���� InfiniteScroll InventoryScrollList;
        base.SetInfo(uiData);

        SetUserStats(); // �ʱ�, ����, Ż�� �Ҷ� ȣ������
        SetEquippedItems();//������ �����ۿ� ���� UIó���� ����� �Լ� ȣ��
        SetInventory();
        //�κ��丮 ���� �Լ� ȣ��
        SortInventory();
    }

    private void SetUserStats()
    {
        //�����κ��丮�����͸� ������
        var userinventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if(userinventoryData == null)
        {
            Logger.LogError("UserInventoryData does not exist");
        }
        //�� userInventoryData ���� ���� �Լ� ȣ�⿡ ���� ���տ� ���� �����͸� ��������
        var userTotalItemStats = userinventoryData.GetUserTotalItemStats();
        //�̸� �� �ؽ�Ʈ ���۳�Ʈ�� ǥ��
        AttackPowerAmountTxt.text = $"+{userTotalItemStats.AttackPower.ToString("N0")}";
        DefenseAmountTxt.text = $"+{userTotalItemStats.Defense.ToString("N0")}";
    }

    private void SetEquippedItems()
    {
        //UserInventoryData�� �����´�
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        //�����Ͱ� null�̸� �����α�
        if(userInventoryData == null)
        {
            Logger.LogError("UserInventoryData does not exist.");
            return;
        }
        //null �ƴϸ� SetItem, null�̸� ClearItem ����
        if(userInventoryData.EquippedWeaponData != null)
        {
            WeaponSlot.SetItem(userInventoryData.EquippedWeaponData);
        }
        else
        {
            WeaponSlot.ClearItem();
        }
        if (userInventoryData.EquippedGlovesData != null)
        {
            GlovesSlot.SetItem(userInventoryData.EquippedGlovesData);
        }
        else
        {
            GlovesSlot.ClearItem();
        }
        if (userInventoryData.EquippedShieldData != null)
        {
            ShieldSlot.SetItem(userInventoryData.EquippedShieldData);
        }
        else
        {
            ShieldSlot.ClearItem();
        }
        if (userInventoryData.EquippedChestArmorData != null)
        {
            ChestArmorSlot.SetItem(userInventoryData.EquippedChestArmorData);
        }
        else
        {
            ChestArmorSlot.ClearItem();
        }
        if (userInventoryData.EquippedBootsData != null)
        {
            BootsSlot.SetItem(userInventoryData.EquippedBootsData);
        }
        else
        {
            BootsSlot.ClearItem();
        }
        if (userInventoryData.EquippedAcessoryData != null)
        {
            AccessorySlot.SetItem(userInventoryData.EquippedAcessoryData);
        }
        else
        {
            AccessorySlot.ClearItem();
        }
    }

    private void SetInventory()
    {
        //UIȭ���� ��Ȱ���ϱ⶧���� ���Ӱ� ������ ������ ����ó���� ������ ������
        //������ ������ �����۵��� �״�� ���� �ְ� ��.
        InventoryScrollList.Clear();
        //������ ������ �������� ��ũ�� �信 ������
        //�켱 ���� �κ��丮 �����͸� ���������͸Ŵ����� �����´�.
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();

        if (userInventoryData != null)
        {
            //��ȸ�ϸ� �� �����ۿ� ���ؼ� ������ ���� �ν��Ͻ��� ����� �ش�.
            foreach (var itemData in userInventoryData.InventoryItemDataList)
            {
                //��ũ�� �信 ������ �����͸� �ϳ��� �߰��� �� ��
                //���� ������ �������̶�� ����ó���� ���ְ���
                if (userInventoryData.IsEquipped(itemData.SerialNumber))
                {
                    continue;
                }
                
                
                var itemSlotData = new InventoryItemSlotData();
                itemSlotData.SerialNumber = itemData.SerialNumber;
                itemSlotData.ItemId = itemData.ItemId;
                InventoryScrollList.InsertData(itemSlotData);
            }
        }
    }
    private void SortInventory()
    {
        //���� Ÿ�Կ� ���� �б�
        switch (m_InventorySortType)
        {
            case InventorySortType.ItemGrade:
                SortBtnTxt.text = "GRADE"; // ���� ���� �ؽ�Ʈ ����
                //������ infiniteScroll�� ���� SortDataList�Լ��� ȣ���ϸ鼭
                //���ϴ� ��޺� ���� ���� ������ ���� �������� �ۼ��ؼ� �Ѱ� ��
                InventoryScrollList.SortDataList((a, b) =>
                {
                    //amb �����͸� InventoryItemSlotData�� �޾ƿ�
                    var itemA = a.data as InventoryItemSlotData;
                    var itemB = b.data as InventoryItemSlotData;
                    //sort by item grade
                    //������ID�� �ι�° �ڸ����� �������� ����� ��Ÿ���� ������ �� ��� ���� �����ͼ� ��
                    //���⼭ itemB ����� �������� ���� ���� �׷��� �������� ������ �Ǳ� ����
                    //���� ��޿��� ���� ������� ���ĵǱ⸦ ���ϱ� ������ ������������ ����
                    //CompareTo : ������, ������ �Ǵ� ���������� ��Ÿ���� ������ ��ȯ�Ѵ�.
                    //0���� ������ �� �ν��Ͻ��� value �տ� ���� ��� �̷�����.
                    int compareResult = ((itemB.ItemId / 1000) % 10).CompareTo((itemA.ItemId / 1000) % 10);

                    //������� 0�̶��, �� ����� ���ٸ� ���� ��� ��������
                    //�������� �ٽ� ������ ����� ��.
                    //������ ������ ������ID���� ����� ��ź�Ŵ� �ι�° �ڸ������� ������ ������ ������ ��
                    //
                    //if same item grade, sort by item type
                    if (compareResult == 0)
                    {
                        var itemnAIdstr = itemA.ItemId.ToString();
                        var itemAComp = itemnAIdstr.Substring(0, 1) + itemnAIdstr.Substring(2, 3); //11001 -> 1001

                        var itemBIdStr = itemB.ItemId.ToString();
                        var itemBComp = itemBIdStr.Substring(0, 1) + itemBIdStr.Substring(2, 3); //11001 -> 1001

                        compareResult = itemAComp.CompareTo(itemBComp);
                    }
                    return compareResult;
                });
                break;
            case InventorySortType.ItemType:
                SortBtnTxt.text = "TYPE";
                InventoryScrollList.SortDataList((a, b) =>
                {
                    //amb �����͸� InventoryItemSlotData�� �޾ƿ�
                    var itemA = a.data as InventoryItemSlotData;
                    var itemB = b.data as InventoryItemSlotData;
                    //sort by item type



                    var itemnAIdstr = itemA.ItemId.ToString();
                    var itemAComp = itemnAIdstr.Substring(0, 1) + itemnAIdstr.Substring(2, 3); //11001 -> 1001

                    var itemBIdStr = itemB.ItemId.ToString();
                    var itemBComp = itemBIdStr.Substring(0, 1) + itemBIdStr.Substring(2, 3); //11001 -> 1001



                    int compareResult = itemAComp.CompareTo(itemBComp);
                    //���࿡ ������ ������ ������� �׶��� ������ ��� �������� ����
                    //if same item type, sort by item grade
                    if (compareResult == 0)
                    {
                        compareResult = ((itemB.ItemId / 1000) % 10).CompareTo((itemA.ItemId / 1000) % 10);
                    }
                    return compareResult;
                });
                break;
            default:
                break;
        }
    }
    
    //�κ��丮 UI���� ��ư�� ������ �� ���� ���� ������
    //�ٸ� ���� �������� �����ϰ� �� ���� ���� �ٽ� �������ִ� ��� ����
    public void OnclickSortBtn()
    {
        switch (m_InventorySortType)
        {
            case InventorySortType.ItemGrade:
                m_InventorySortType = InventorySortType.ItemType;
                break;
            case InventorySortType.ItemType:
                m_InventorySortType = InventorySortType.ItemGrade;
                break;
            default:
                break;
        }
        SortInventory();
    }
    public void OnEquipItem(int itemId)
    {
        //UserInventoryData�� ������
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if(userInventoryData == null)
        {
            Logger.LogError("UserInventoryData does not exist");
        }
        //������ ������ ���� �б� ó��
        var itemType = (ItemType)(itemId / 10000);
        switch (itemType)
        {
            case ItemType.Weapon:
                //���⸦ �����ϴ� ��Ȳ�̶�� ���� ������ ��������
                WeaponSlot.SetItem(userInventoryData.EquippedWeaponData);
                break;
            case ItemType.Shield:
                ShieldSlot.SetItem(userInventoryData.EquippedShieldData);
                break;
            case ItemType.Gloves:
                GlovesSlot.SetItem(userInventoryData.EquippedGlovesData);
                break;
            case ItemType.ChestArmor:
                ChestArmorSlot.SetItem(userInventoryData.EquippedChestArmorData);
                break;
            case ItemType.Boots:
                BootsSlot.SetItem(userInventoryData.EquippedBootsData);
                break;
            case ItemType.Accessory:
                AccessorySlot.SetItem(userInventoryData.EquippedAcessoryData);
                break;
            default:
                break;
        }
        SetUserStats();
        SetInventory(); // �κ��丮�� �ٽ� ����
        SortInventory(); //���ı��� �ٽ�
    }
    public void OnUnequipItem(int itemId)
    {
        var userinventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if(userinventoryData == null)
        {
            Logger.LogError("UserinventoryData does not exist");
        }
        var itemType = (ItemType)(itemId / 10000);
        switch (itemType)
        {
            case ItemType.Weapon:
                WeaponSlot.ClearItem();
                break;
            case ItemType.Shield:
                ShieldSlot.ClearItem();
                break;
            case ItemType.Gloves:
                GlovesSlot.ClearItem();
                break;
            case ItemType.ChestArmor:
                ChestArmorSlot.ClearItem();
                break;
            case ItemType.Boots:
                BootsSlot.ClearItem();
                break;
            case ItemType.Accessory:
                AccessorySlot.ClearItem();
                break;
        }
        SetUserStats();
        SetInventory();
        SortInventory();
    }
}
