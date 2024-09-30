using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ������ ���� ������ ������ ������ Ŭ����
//[Serializable]
//�÷��̾������������� int, float, string���� ������ �� �ֱ� ������
//�� Ŭ������ �ν��Ͻ� ��ü�� String ������ ��ȯ�ؼ� ������ ����.
//�׷��� �� Ŭ������ �ν��Ͻ��� String������ ��ȯ�� �����ϰ� ����ȭ�� �����ϴٰ� ������ ���ִ� ����.
//����ȭ�� �ν��Ͻ��� byte�� string ������ ��ȯ�ϴ� ���� �ǹ�
[Serializable]
public class UserItemData
{
    //��ü�����ۿ��� Ư�� �������� �ĺ��ϱ� ���� id
    public long SerialNumber; //unique value

    //�ش� �������� �����۵���Ÿ���̺� ���� ItemId
    //���� UI�� ó���� �� �� ������ID�� �����Ͽ� ���������̺�Ŵ�������
    //�ش� �����۵������� �� �����͸� �����ͼ� ������ ���̳� ���ݷ�, ���°� ����
    //������ ǥ�ø� ���� ����
    public int ItemId;
    //������
    public UserItemData(long serialNumber, int itemId)
    {
        SerialNumber = serialNumber;
        ItemId = itemId;
    }
}
//������ ������ ������ ����Ʈ�� ���� ����̽��� �ε��ϰ� �����ϴµ� �� ���� Ŭ����
//�� Ŭ������ �� ������� �ϸ� �ٷ� �Ʒ��� ���� UserInventoryData��
//�����ϰ� �� ������ ������ ����
//�׸��� �ٷ� �� ������ ������ ������ ������ ����� ����� �ǵ�
//�̷��� ����Ʈ�� �� ��ü �����͸� �÷��̾��������� ������ �� Json��Ʈ������
//��ȯ�� �ؼ� ������ ����
//�׷��� ����Ƽ���� �����ϴ� JsonUtility��� Json APIŬ������
//����Ʈ�� ������ �� �� ����Ʈ �����̳ʸ� �ٷ� Json��Ʈ������ ��ȯ�ϴ� ����� �������� ����.
//�� �̷��� ���� Ŭ������ �ְ� �� �ȿ� ����Ʈ�� ����� ���·� ����� ���
//��ȯ�� ����� ���ֱ� ������ �� ���� Ŭ������ ����� ����.
//wrapper class to parse data to JSON using JSONUtility
[Serializable]
public class UserInventoryItemDataListWrapper
{
    public List<UserItemData> InventoryItemDataList;
}

public class UserInventoryData : IUserData
{
    //�� ���� ������ ������ ����
    public UserItemData EquippedWeaponData { get; set; }
    public UserItemData EquippedShieldData { get; set; }
    public UserItemData EquippedChestArmorData { get; set; }
    public UserItemData EquippedBootsData { get; set; }
    public UserItemData EquippedGlovesData { get; set; }
    public UserItemData EquippedAcessoryData { get; set; }




    public List<UserItemData> InventoryItemDataList { get; set; } = new List<UserItemData>();

    public void SetDefaultData()
    {
        Logger.Log($"{GetType()}::SetDefaultData");


        //�⺻������ 12���� �������� ������ �ֵ��� �ϰ���.
        /*
        �������� �ø��� �ѹ��� �ٸ� �����۰� ��ġ�� �ʴ� ������ ���̾�� �Ѵ�.
        ���� �� ������Ʈ���� ������ ��Ģ���� �ø��� �ѹ��� ����
        ������ID�� ���������� �����ۿ� ���� ������ �����ϸ鼭 ������ ���� ���� ������� ����
        ��Ģ�� ������ �� �ƴϹǷ� �ڽ��� ��Ģ���� �ø��� �ѹ��� ����� �ȴ�.
        */
        //���� �ð��� ������ ���� �ٿ��� �ø��� �ѹ� ����
        //serial number => DateTime.Now.ToString("yyyyMMddHHmmss") + UnityEngine.Random.Range(0, 9999).ToString("D4") 
        //���ο� ���� �����۵����͸� �����԰� ���ÿ� InventoryItemDataList�� �߰�
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
    }

    //������ ������ �������� �ε��ϴ� �Լ�
    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");

        bool result = false;
        try
        {
            //���� ������ �����͸� �ε��ϴ� ������ �ۼ�
            //���� ���� �����͸� �ε�
            //�÷��̾� ���������� "EquippedWeaponData"�� ����Ǿ��ִ� ���ڿ� �����Ͱ��ִٸ�
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
            string GlovesJson = PlayerPrefs.GetString("EquippedGolvesData");
            if (!string.IsNullOrEmpty(GlovesJson))
            {
                EquippedGlovesData = JsonUtility.FromJson<UserItemData>(GlovesJson);
                Logger.Log($"EquippedGlovesData: SN:{EquippedGlovesData.SerialNumber}ItemId{EquippedGlovesData.ItemId}");
            }
            string AcessoryJson = PlayerPrefs.GetString("EquippedAcessoryData");
            if (!string.IsNullOrEmpty(AcessoryJson))
            {
                EquippedAcessoryData = JsonUtility.FromJson<UserItemData>(AcessoryJson);
                Logger.Log($"EquippedAcessoryData: SN:{EquippedAcessoryData.SerialNumber}ItemId{EquippedAcessoryData.ItemId}");
            }
        }
        catch
        {
            throw;
        }
        try
        {

            //�κ��丮�����۵����� ����Ʈ�� ����� ��Ʈ�� ���� �ִ��� Ȯ��
            //���� �����Ͱ� �����Ѵٸ� JsonUtilityŬ������ �̿��� ������ ���� ����Ŭ������ ����� �����͸� �޾ƿ�
            string inventoryItemDataListJson = PlayerPrefs.GetString("InventoryItemDataList");
            if (!string.IsNullOrEmpty(inventoryItemDataListJson))
            {
                UserInventoryItemDataListWrapper itemDataListWrapper =
                JsonUtility.FromJson<UserInventoryItemDataListWrapper>(inventoryItemDataListJson);
                //�� ���� Ŭ���� ���� �ִ� InventoryItemDataLis�� �ִ� �����͸�
                //UserInventoryData�� InventoryItemDataList ������ ����
                InventoryItemDataList = itemDataListWrapper.InventoryItemDataList;

                Logger.Log("InventoryItemDataList"); //�� �ε� �Ǿ����� �α�
                //�ø��� �ѹ��� ������ �Ƶ� ����
                foreach (var item in InventoryItemDataList)
                {
                    Logger.Log($"SerialNumber:{item.SerialNumber} ItemId:{item.ItemId}");
                }
            }

            result = true;
        }
        catch (System.Exception e)
        {
            Logger.Log("Load failed (" + e.Message + ")");
        }

        return result;
    }

    //������ ������ �������� �����ϴ� �Լ�
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
            PlayerPrefs.SetString("EquippedGolvesData", GlovesJson);
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
        }
        catch
        {
            throw;
        }
        try
        {
            //���忡 �ʿ��� ���� Ŭ���� �ν��ͽ��� ����
            //�� �ν��ͽ� �ȿ� �ִ� ������ �����͸���Ʈ�� ������ ���� ������ �κ��丮 ������ ������
            //��� �ִ� �κ��丮 ������ ������ ����Ʈ�� ����
            UserInventoryItemDataListWrapper inventoryItemDataListWrapper =
            new UserInventoryItemDataListWrapper();

            inventoryItemDataListWrapper.InventoryItemDataList = InventoryItemDataList;
            //�� �����͸� JsonUtilityŬ������ �̿��ؼ� ��Ʈ������ ��ȯ
            string inventoryItemDataListJson = JsonUtility.ToJson(inventoryItemDataListWrapper);
            //�� ��Ʈ�� ���� �÷��̾��������� ����
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
}
