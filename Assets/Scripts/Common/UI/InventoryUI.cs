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
    public override void SetInfo(BaseUIData uiData)
    {
        //��ũ�Ѻ� ó���� ���� InfiniteScroll InventoryScrollList;
        base.SetInfo(uiData);

        SetEquippedItems();//������ �����ۿ� ���� UIó���� ����� �Լ� ȣ��
        SetInventory();
        //�κ��丮 ���� �Լ� ȣ��
        SortInventory();
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
        if (userInventoryData.EquippedWeaponData != null)
        {
            WeaponSlot.SetItem(userInventoryData.EquippedWeaponData);
        }
        else
        {
            WeaponSlot.ClearItem();
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
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
