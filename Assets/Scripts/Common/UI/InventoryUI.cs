using Gpm.Ui;
using TMPro;


//먼저 어떤 조건에 의해 정렬을 할 것인지를 나타내는 이넘 값을 선언
public enum InventorySortType
{
    ItemGrade, //등급
    ItemType, //종류
}
public class InventoryUI : BaseUI
{
    //먼저 각 장착 슬롯 컴퍼넌트 변수를 선언
    public EquippedItemSlot WeaponSlot;
    public EquippedItemSlot ShieldSlot;
    public EquippedItemSlot ChestArmorSlot;
    public EquippedItemSlot BootsSlot;
    public EquippedItemSlot GlovesSlot;
    public EquippedItemSlot AccessorySlot;

    //인벤토리 UI 관련 
    public InfiniteScroll InventoryScrollList;
    //현재 어떤 조건으로 정렬되어 있는지 표시해줄 텍스트 컴퍼넌트 선언
    public TextMeshProUGUI SortBtnTxt;
    //현재 정렬 방식을 갖고 있는 변수 선언, 초기값은 등급으로.
    private InventorySortType m_InventorySortType = InventorySortType.ItemGrade;

    public TextMeshProUGUI AttackPowerAmountTxt;
    public TextMeshProUGUI DefenseAmountTxt;
    public override void SetInfo(BaseUIData uiData)
    {
        //스크롤뷰 처리를 위한 InfiniteScroll InventoryScrollList;
        base.SetInfo(uiData);

        SetUserStats(); // 초기, 장착, 탈착 할때 호출해줌
        SetEquippedItems();//장착된 아이템에 대한 UI처리를 담당할 함수 호출
        SetInventory();
        //인벤토리 정렬 함수 호출
        SortInventory();
    }

    private void SetUserStats()
    {
        //유저인벤토리데이터를 가져옴
        var userinventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if(userinventoryData == null)
        {
            Logger.LogError("UserInventoryData does not exist");
        }
        //위 userInventoryData 에서 만든 함수 호출에 스탯 초합에 대한 데이터를 가져오고
        var userTotalItemStats = userinventoryData.GetUserTotalItemStats();
        //이를 각 텍스트 컴퍼넌트에 표시
        AttackPowerAmountTxt.text = $"+{userTotalItemStats.AttackPower.ToString("N0")}";
        DefenseAmountTxt.text = $"+{userTotalItemStats.Defense.ToString("N0")}";
    }

    private void SetEquippedItems()
    {
        //UserInventoryData를 가져온다
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        //데이터가 null이면 에러로그
        if(userInventoryData == null)
        {
            Logger.LogError("UserInventoryData does not exist.");
            return;
        }
        //null 아니면 SetItem, null이면 ClearItem 실행
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
        //UI화면을 재활용하기때문에 새롭게 세팅할 때마다 삭제처리를 해주지 않으면
        //기존에 생성된 아이템들이 그대로 남아 있게 됨.
        InventoryScrollList.Clear();
        //유저가 보유한 아이템을 스크롤 뷰에 만들어보자
        //우선 유저 인벤토리 데이터를 유저데이터매니저에 가져온다.
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();

        if (userInventoryData != null)
        {
            //순회하며 각 아이템에 대해서 아이템 슬롯 인스턴스를 만들어 준다.
            foreach (var itemData in userInventoryData.InventoryItemDataList)
            {
                //스크롤 뷰에 아이템 데이터를 하나씩 추가해 줄 때
                //만약 장착된 아이템이라면 예외처리를 해주겠음
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
        //정렬 타입에 따라 분기
        switch (m_InventorySortType)
        {
            case InventorySortType.ItemGrade:
                SortBtnTxt.text = "GRADE"; // 정령 조건 텍스트 셋팅
                //위에서 infiniteScroll에 만든 SortDataList함수를 호출하면서
                //원하는 등급별 정렬 조건 로직을 람다 형식으로 작성해서 넘겨 줌
                InventoryScrollList.SortDataList((a, b) =>
                {
                    //amb 데이터를 InventoryItemSlotData로 받아옴
                    var itemA = a.data as InventoryItemSlotData;
                    var itemB = b.data as InventoryItemSlotData;
                    //sort by item grade
                    //아이템ID의 두번째 자릿수가 아이템의 등급을 나타내기 때문에 그 등급 값을 가져와서 비교
                    //여기서 itemB 등급을 기준으로 비교한 것은 그래야 내림차순 정렬이 되기 때문
                    //높은 등급에서 낮은 등급으로 정렬되기를 원하기 때문에 내림차순으로 했음
                    //CompareTo : 앞인지, 뒤인지 또는 동일한지를 나타내는 정수를 반환한다.
                    //0보다 작으면 이 인스턴스가 value 앞에 오는 경우 이런식임.
                    int compareResult = ((itemB.ItemId / 1000) % 10).CompareTo((itemA.ItemId / 1000) % 10);

                    //결과값이 0이라면, 즉 등급이 같다면 같은 등급 내에서는
                    //종류별로 다시 정렬을 해줘야 함.
                    //종류별 정렬을 아이템ID에서 등급을 나탄매는 두번째 자릿수만을 제외한 나머지 값으로 비교
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
                    //amb 데이터를 InventoryItemSlotData로 받아옴
                    var itemA = a.data as InventoryItemSlotData;
                    var itemB = b.data as InventoryItemSlotData;
                    //sort by item type



                    var itemnAIdstr = itemA.ItemId.ToString();
                    var itemAComp = itemnAIdstr.Substring(0, 1) + itemnAIdstr.Substring(2, 3); //11001 -> 1001

                    var itemBIdStr = itemB.ItemId.ToString();
                    var itemBComp = itemBIdStr.Substring(0, 1) + itemBIdStr.Substring(2, 3); //11001 -> 1001



                    int compareResult = itemAComp.CompareTo(itemBComp);
                    //만약에 동일한 아이템 종류라면 그때는 아이템 등급 기준으로 정렬
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
    
    //인벤토리 UI정렬 버튼을 눌렀을 대 현재 정렬 조건을
    //다른 정렬 조건으로 변경하고 그 값에 따라 다시 정렬해주는 기능 구현
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
        //UserInventoryData를 가져옴
        var userInventoryData = UserDataManager.Instance.GetUserData<UserInventoryData>();
        if(userInventoryData == null)
        {
            Logger.LogError("UserInventoryData does not exist");
        }
        //아이템 종류에 따른 분기 처리
        var itemType = (ItemType)(itemId / 10000);
        switch (itemType)
        {
            case ItemType.Weapon:
                //무기를 장착하는 상황이라면 무기 슬롯을 세팅해줌
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
        SetInventory(); // 인벤토리를 다시 세팅
        SortInventory(); //정렬까지 다시
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
