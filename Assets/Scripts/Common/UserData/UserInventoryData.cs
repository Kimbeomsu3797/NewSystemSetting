using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//먼저 보유한 개별 아이템 정보를 저장할 클래스
//[Serializable]
//플레이어프렙스에서는 int, float, string값만 저장할 수 있기 때문에
//이 클래스의 인스턴스 객체를 String 값으로 변환해서 저장할 것임.
//그래서 이 클래스의 인스턴스가 String값으로 변환이 가능하게 직렬화가 가능하다고 선언을 해주는 것임.
//직렬화란 인스턴스를 byte나 string 값으로 변환하는 것을 의미
[Serializable]
public class UserItemData
{
    //전체아이템에서 특정 아이템을 식별하기 위한 id
    public long SerialNumber; //unique value

    //해당 아이템의 아이템데이타테이블 상의 ItemId
    //이후 UI를 처리할 때 이 아이템ID를 참고하여 데이터테이블매니저에서
    //해당 아이템데이터의 상세 데이터를 가져와서 아이템 명이나 공격력, 방어력과 같은
    //스탯을 표시를 해줄 예정
    public int ItemId;
    //생성자
    public UserItemData(long serialNumber, int itemId)
    {
        SerialNumber = serialNumber;
        ItemId = itemId;
    }
}
//유저가 보유한 아이템 리스트를 로컬 디바이스에 로드하고 저장하는데 쓸 래퍼 클래스
//이 클래스를 왜 만드느냐 하면 바로 아래에 만들 UserInventoryData에
//동일하게 이 변수를 선언할 예정
//그리고 바로 그 변수에 유저가 보유한 아이템 목록이 저장될 건데
//이렇게 리스트로 된 객체 데이터를 플레이어프랩스로 저장할 때 Json스트링으로
//변환을 해서 저장할 것임
//그런데 유니티에서 제공하는 JsonUtility라는 Json API클래스가
//리스트를 저장할 때 이 리스트 컨테이너를 바로 Json스트링으로 변환하는 기능을 제공하지 않음.
//꼭 이렇게 래퍼 클래스가 있고 그 안에 리스트가 선언된 형태로 만들어 줘야
//변환을 제대로 해주기 때문에 이 래퍼 클래스를 만드는 서임.
//wrapper class to parse data to JSON using JSONUtility
[Serializable]
public class UserInventoryItemDataListWrapper
{
    public List<UserItemData> InventoryItemDataList;
}

public class UserItemStats
{
    public int AttackPower;
    public int Defense;
    //생성자
    public UserItemStats(int attackPower, int defense)
    {
        AttackPower = attackPower;
        Defense = defense;
    }
}

public class UserInventoryData : IUserData
{
    //각 장착 슬롯의 변수를 선언
    public UserItemData EquippedWeaponData { get; set; } //무기 장착 슬롯
    public UserItemData EquippedShieldData { get; set; }
    public UserItemData EquippedChestArmorData { get; set; }
    public UserItemData EquippedBootsData { get; set; }
    public UserItemData EquippedGlovesData { get; set; }
    public UserItemData EquippedAcessoryData { get; set; }





    public List<UserItemData> InventoryItemDataList { get; set; } = new List<UserItemData>();

    // 장착된 아이템 데이터를 한곳에 모아 관리하는 딕셔너리 변수 추가
    //key : 장착한 아이템의 시리얼 넘버, Value : 아이템의 공격력과 방어력을 모아놓은 새로운 클래스
    //이미 장착 아이템 변수들이 있는데 이 딕셔너리에 장착한 모든 아이템들의 데이터를 추가하는 이유는
    //특정 아이템이 장착이 되어 있는지 여부, 또 장착한 아이템들의 스텟의 총합이 어떻게 되는지 등의 처리가 필요할때
    //매번 이 개별적인 변수들이 null인지 확인하고 그 후에 아이템 데이터 테이블에서 데이터를 찾아와서
    //스텟 값들이 어떻게 되는지 확인하는 것이 코드도 복잡하고 연산도 오래걸리기 때문이다.
    //그러나 이렇게 하나의 딕셔너리로 관리하게된다면 딕셔너리의 특성 때문에 장착한 아이템의 데이터를
    //검색하는 것도 빠르고 또 유저의 스텟 계산을 하는 것도 더 간편하기 때문에 사용한다.
    public Dictionary<long, UserItemStats> EquippedItemDic { get; set; } = new Dictionary<long, UserItemStats>();
    public void SetDefaultData()
    {
        Logger.Log($"{GetType()}::SetDefaultData");


        //기본적으로 12개의 아이템을 지급해 주도록 하겠음.
        /*
        아이템의 시리얼 넘버는 다른 아이템과 겹치지 않는 고유의 값이어야 한다.
        실제 각 프로젝트마다 고유의 법칙으로 시리얼 넘버를 만듬
        아이템ID와 마찬가지로 아이템에 대한 정보를 내포하면서 고유한 값을 갖는 방식으로 만듬
        규칙이 정해진 게 아니므로 자신의 규칙으로 시리얼 넘버를 만들면 된다.
        */
        //현재 시간에 랜덤한 수를 붙여서 시리얼 넘버 생성
        //serial number => DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4") 
        //새로운 유저 아이템데이터를 생성함과 동시에 InventoryItemDataList에 추가
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 11001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 11002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 22001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 22002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 33001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 33002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 44001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 44002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 55001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 55002));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 65001));
        InventoryItemDataList.Add(new UserItemData(long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4")), 65002));

        EquippedWeaponData = new UserItemData(InventoryItemDataList[0].SerialNumber, InventoryItemDataList[0].ItemId);
        EquippedShieldData = new UserItemData(InventoryItemDataList[2].SerialNumber, InventoryItemDataList[2].ItemId);
        
        //게임을 처음 실행했을 때는 저장되어있는 아이템을 로딩하는것이아니라 //SetDefaultData()에서 호출하므로
        //여기서 호출해 줘야 장착된 아이템이 딕셔너리에 추가됨.
        SetEquippedItemDic();
    }

    //유저가 보유한 아이템을 로드하는 함수
    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");

        bool result = false;
       
        try
        {
            //장착 아이템 데이터를 로드하는 로직을 작성
            //무기 슬롯 데이터를 로드
            //플레이어 프랩스에서 "EquippedWeaponData"로 저장되어있는 문자열 데이터가있다면
            string weaponJson = PlayerPrefs.GetString("EquippedWeaponData");
            if (!string.IsNullOrEmpty(weaponJson))
            {
                EquippedWeaponData = JsonUtility.FromJson<UserItemData>(weaponJson);
                Logger.Log($"EquippedWeaponData: SN:{EquippedWeaponData.SerialNumber}ItemId{EquippedWeaponData.ItemId}");
            }
            string shieldJson = PlayerPrefs.GetString("EquippedShieldData");
            if (!string.IsNullOrEmpty(shieldJson))
            {
                EquippedShieldData = JsonUtility.FromJson<UserItemData>(shieldJson);
                Logger.Log($"EquippedShieldData: SN:{EquippedShieldData.SerialNumber}ItemId{EquippedShieldData.ItemId}");
            }
            string chestArmorJson = PlayerPrefs.GetString("EquippedchestArmorData");
            if (!string.IsNullOrEmpty(chestArmorJson))
            {
                EquippedChestArmorData = JsonUtility.FromJson<UserItemData>(chestArmorJson);
                Logger.Log($"EquippedChestArmorData: SN:{EquippedChestArmorData.SerialNumber}ItemId{EquippedChestArmorData.ItemId}");
            }
            string BootsJson = PlayerPrefs.GetString("EquippedbootsData");
            if (!string.IsNullOrEmpty(BootsJson))
            {
                EquippedBootsData = JsonUtility.FromJson<UserItemData>(BootsJson);
                Logger.Log($"EquippedBootsData: SN:{EquippedBootsData.SerialNumber}ItemId{EquippedBootsData.ItemId}");
            }
            string glovesJson = PlayerPrefs.GetString("EquippedGlovesData");
            if (!string.IsNullOrEmpty(glovesJson))
            {
                EquippedGlovesData = JsonUtility.FromJson<UserItemData>(glovesJson);
                Logger.Log($"EquippedGlovesData: SN:{EquippedGlovesData.SerialNumber}ItemId{EquippedGlovesData.ItemId}");
            }
            string AcessoryJson = PlayerPrefs.GetString("EquippedAcessoryData");
            if (!string.IsNullOrEmpty(AcessoryJson))
            {
                EquippedAcessoryData = JsonUtility.FromJson<UserItemData>(AcessoryJson);
                Logger.Log($"EquippedAcessoryData: SN:{EquippedAcessoryData.SerialNumber}ItemId{EquippedAcessoryData.ItemId}");
            }
            //인벤토리아이템데이터 리스트로 저장된 스트링 값이 있는지 확인
            //만약 데이터가 존재한다면 JsonUtility클래스를 이용해 위에서 만든 래퍼클래스로 저장된 데이터를 받아옮
            string inventoryItemDataListJson = PlayerPrefs.GetString("InventoryItemDataList");
            if (!string.IsNullOrEmpty(inventoryItemDataListJson))
            {
                UserInventoryItemDataListWrapper itemDataListWrapper =
                JsonUtility.FromJson<UserInventoryItemDataListWrapper>(inventoryItemDataListJson);
                //그 래퍼 클래스 내에 있는 InventoryItemDataLis에 있는 데이터를
                //UserInventoryData의 InventoryItemDataList 변수에 대입
                InventoryItemDataList = itemDataListWrapper.InventoryItemDataList;

                Logger.Log("InventoryItemDataList"); //잘 로드 되었는지 로그
                //시리얼 넘버와 아이템 아디도 찍어보자
                foreach (var item in InventoryItemDataList)
                {
                    Logger.Log($"SerialNumber:{item.SerialNumber} ItemId:{item.ItemId}");
                }
            }
            //유저 인벤토리데이터를 로드할 시 모든 로딩이 끝나고 나면 함수 호출
            SetEquippedItemDic();
            result = true;
        }
        catch (System.Exception e)
        {
            Logger.Log("Load failed (" + e.Message + ")");
        }

        return result;
    }

    //유저가 보윤한 아이템을 저장하는 함수
    public bool SaveData()
    {
        Logger.Log($"{GetType()}::SaveData");

        bool result = false;
       
        try
        {
            string weaponJson = JsonUtility.ToJson(EquippedWeaponData);
            PlayerPrefs.SetString("EquippedWeaponData", weaponJson);
            if (EquippedWeaponData != null)
            {
                Logger.Log($"EquippedWeaponData: SN:{EquippedWeaponData.SerialNumber}ItemId{EquippedWeaponData.ItemId}");
            }
            string shieldJson = JsonUtility.ToJson(EquippedShieldData);
            PlayerPrefs.SetString("EquippedShieldData", shieldJson);
            if (EquippedShieldData != null)
            {
                Logger.Log($"EquippedShieldData: SN:{EquippedShieldData.SerialNumber}ItemId{EquippedShieldData.ItemId}");
            }
            string chestArmorJson = JsonUtility.ToJson(EquippedChestArmorData);
            PlayerPrefs.SetString("EquippedchestArmorData", chestArmorJson);
            if (EquippedChestArmorData != null)
            {
                Logger.Log($"EquippedChestArmorData: SN:{EquippedChestArmorData.SerialNumber}ItemId{EquippedChestArmorData.ItemId}");
            }
            string BootsJson = JsonUtility.ToJson(EquippedBootsData);
            PlayerPrefs.SetString("EquippedbootsData", BootsJson);
            if (EquippedBootsData != null)
            {
                Logger.Log($"EquippedBootsData: SN:{EquippedBootsData.SerialNumber}ItemId{EquippedBootsData.ItemId}");
            }
            string GlovesJson = JsonUtility.ToJson(EquippedGlovesData);
            PlayerPrefs.SetString("EquippedGlovesData", GlovesJson);
            if (EquippedGlovesData != null)
            {
                Logger.Log($"EquippedGlovesData: SN:{EquippedGlovesData.SerialNumber}ItemId{EquippedGlovesData.ItemId}");
            }
            string AcessoryJson = JsonUtility.ToJson(EquippedAcessoryData);
            PlayerPrefs.SetString("EquippedAcessoryData", AcessoryJson);
            if (EquippedAcessoryData != null)
            {
                Logger.Log($"EquippedAcessoryData: SN:{EquippedAcessoryData.SerialNumber}ItemId{EquippedAcessoryData.ItemId}");
            }
            //저장에 필요한 랩퍼 클래스 인스터스를 생성
            //그 인스터스 안에 있는 아이템 데이터리스트에 유저가 현재 보유한 인벤토리 아이템 정보를
            //담고 있는 인벤토리 아이템 데이터 리스트를 대입
            UserInventoryItemDataListWrapper inventoryItemDataListWrapper =
            new UserInventoryItemDataListWrapper();

            inventoryItemDataListWrapper.InventoryItemDataList = InventoryItemDataList;
            //이 데이터를 JsonUtility클래스를 이용해서 스트링으로 변환
            string inventoryItemDataListJson = JsonUtility.ToJson(inventoryItemDataListWrapper);
            //이 스트링 값을 플레이어프렙프에 저장
            PlayerPrefs.SetString("InventoryItemDataList", inventoryItemDataListJson);

            Logger.Log("InventoryItemDataList");
            foreach (var item in InventoryItemDataList)
            {
                Logger.Log($"SerialNumber:{item.SerialNumber} ItemId:{item.ItemId}");
            }
            
            PlayerPrefs.Save();
            result = true;
        }
        catch (System.Exception e)
        {
            Logger.Log("Load failed (" + e.Message + ")");
        }

        return result;
    }
    //데이터를 확인해서 장착된 아이템이 있다면 필요한 정보를 가공해서
    //EquippedItemDic에 추가
    public void SetEquippedItemDic()
    {
        if(EquippedWeaponData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedWeaponData.ItemId);
            if(itemData != null)
            {
                EquippedItemDic.Add(EquippedWeaponData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
        if (EquippedShieldData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedShieldData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedShieldData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
        if (EquippedGlovesData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedGlovesData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedGlovesData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
        if (EquippedChestArmorData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedChestArmorData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedChestArmorData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
        if (EquippedBootsData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedBootsData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedBootsData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
        if (EquippedAcessoryData != null)
        {
            var itemData = DataTableManager.Instance.GetItemData(EquippedAcessoryData.ItemId);
            if (itemData != null)
            {
                EquippedItemDic.Add(EquippedAcessoryData.SerialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
            }
        }
    }

    //특정 아이템이 장착되어있는지 여부를 확인하는 함수
    public bool IsEquipped(long serialNumber)
    {
        return EquippedItemDic.ContainsKey(serialNumber);
    }
    //아이템 장착처리를 해주는 함수
    public void EquipItem(long serialNumber, int itemId)
    {
        //데이터테이블에서 해당 아이템에 대한 데이터를 가져온다.
        var itemData = DataTableManager.Instance.GetItemData(itemId);
        //없으면 에러로그
        if(itemData == null)
        {
            Logger.LogError($"Item data does not exist. ItemId:{itemId}");
            return;
        }
        //아이템 종류 추출 (첫번째 숫자가 아이템 종류)
        var itemType = (ItemType)(itemId / 10000);
        switch (itemType)
        {
            case ItemType.Weapon:
                if(EquippedWeaponData != null)
                {
                    EquippedItemDic.Remove(EquippedWeaponData.SerialNumber);
                    EquippedWeaponData = null;
                }
                EquippedWeaponData = new UserItemData(serialNumber, itemId);
                break;
            case ItemType.Shield:
                if (EquippedShieldData != null)
                {
                    EquippedItemDic.Remove(EquippedShieldData.SerialNumber);
                    EquippedShieldData = null;
                }
                EquippedShieldData = new UserItemData(serialNumber, itemId);
                break;
            case ItemType.ChestArmor:
                if (EquippedChestArmorData != null)
                {
                    EquippedItemDic.Remove(EquippedChestArmorData.SerialNumber);
                    EquippedChestArmorData = null;
                }
                EquippedChestArmorData = new UserItemData(serialNumber, itemId);
                break;
            case ItemType.Gloves:
                if (EquippedGlovesData != null)
                {
                    EquippedItemDic.Remove(EquippedGlovesData.SerialNumber);
                    EquippedGlovesData = null;
                }
                EquippedGlovesData = new UserItemData(serialNumber, itemId);
                break;
            case ItemType.Boots:
                if (EquippedBootsData != null)
                {
                    EquippedItemDic.Remove(EquippedBootsData.SerialNumber);
                    EquippedBootsData = null;
                }
                EquippedBootsData = new UserItemData(serialNumber, itemId);
                break;
            case ItemType.Accessory:
                if (EquippedAcessoryData != null)
                {
                    EquippedItemDic.Remove(EquippedAcessoryData.SerialNumber);
                    EquippedAcessoryData = null;
                }
                EquippedAcessoryData = new UserItemData(serialNumber, itemId);
                break;
            default:
                break;
        }
        EquippedItemDic.Add(serialNumber, new UserItemStats(itemData.AttackPower, itemData.Defense));
    }
    public void UnequipItem(long serialNumber, int itemId)
    {
        //마찬가지로 아이템 종류값을 추출하고
        var itemType = (ItemType)(itemId / 10000);
        //아이템 종류에 따라 해당 변수를 초기화해줌
        switch (itemType)
        {
            case ItemType.Weapon:
                EquippedWeaponData = null;
                break;
            case ItemType.Shield:
                EquippedShieldData = null;
                break;
            case ItemType.Gloves:
                EquippedGlovesData = null;
                break;
            case ItemType.ChestArmor:
                EquippedChestArmorData = null;
                break;
            case ItemType.Boots:
                EquippedBootsData = null;
                break;
            case ItemType.Accessory:
                EquippedAcessoryData = null;
                break;
            default:
                break;
        }
        EquippedItemDic.Remove(serialNumber);
    }
    public UserItemStats GetUserTotalItemStats()
    {
        var totalAttackPower = 0;
        var totalDefense = 0;
        foreach(var item in EquippedItemDic)
        {
            totalAttackPower += item.Value.AttackPower;
            totalDefense += item.Value.Defense;
        }
        return new UserItemStats(totalAttackPower, totalDefense);
    }
}
