using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    //������ ���̺� ���ϵ��� ����ִ� ��θ� String������ ����
    private const string DATA_PATH = "DataTable";


    protected override void Init()
    {
        base.Init();

        LoadChapterDataTable();
        LoadItemDataTable();
    }
    #region CHAPTER_DATA
    //é�� ������ ���̺� ���ϸ��� ���� String ����
    private const string CHAPTER_DATA_TABLE = "ChapterDataTable";
    //��� é�� �����͸� ������ �� �ִ� �����̳� ��, �ڷᱸ���� ����
    private List<ChapterData> ChapterDataTable = new List<ChapterData>();
    //é�͵���Ÿ���̺��� �ε��ϴ� �Լ�
    private void LoadChapterDataTable()
    {
        //CSVReader.Read()�� ����Ʈ�� ��ȯ�ϴ� �Լ�
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{CHAPTER_DATA_TABLE}");
        //var Ÿ�� : ������ Ÿ���� �Ű澲�� �ʾƵ� �˾Ƽ� ������ �ִ� ���� �Ǵ��ؼ� Ÿ���� �ν��ϴ� Ÿ��
        //���������θ� ��� ����
        //�ݵ�� ����� ���ÿ� �ʱ�ȭ(�׷��� ���������� ������ Ÿ���� ������)
        //Ÿ���� ������ ��� ���������� ����Ҷ��� var�� ����ص� ������.
        

        //���̺��� ��ȸ�ϸ鼭 �� �����͸�
        //ChapterData�ν��Ͻ��� ����
        //ChapterDataTable �����̳ʿ� �־���
        foreach(var data in parsedDataTable)
        {
            var chapterData = new ChapterData
            {
                //������Ʈ Ÿ���̶� ������ ��ü�� ���� 32��Ʈ ��ȣ �ִ� ������ ��ȯ
                ChapterNo = Convert.ToInt32(data["chapter_no"]),
                ChapterName = data["chapter_name"].ToString(), // ChapterName������ �ε��� ������ ���� �������ִ� �ڵ� �߰�
                TotalStages = Convert.ToInt32(data["total_stages"]),
                ChapterRewardGem = Convert.ToInt32(data["chapter_reward_gem"]),
                ChapterRewardGold = Convert.ToInt32(data["chapter_reward_gold"]),
            };
            ChapterDataTable.Add(chapterData);
        }
    }
    //�̷��� �ε��� ChapterDataTable���� ã���� �ϴ� ChapterData�� �������� �Լ�
    public ChapterData GetChapterData(int chapterNo)
    {
        //Ư�� é�� �ѹ��� é�� ������ ���̺��� �˻��ؼ�
        //�� é�� �ѹ��� �ش��ϴ� �����͸� ��ȯ�ϴ� �Լ�
        //��ũ ��� -> ��ũ : �˻�, ������ �� �� �����ϰ� ���ִ� ���
        //���� ��ũ�� ������� �ʴ´ٸ�
        /*
         foreach (var item in ChapterDataTable)
        {
            if(item.ChapterNo == chapterNO)
                {
                    return item;
                }
            
        }
            return null;
        */
        //Linq
        //.Where ���ǽ��� true�� ��Ҹ� ���͸�
        //FirstOrDefault() �Լ� : �Ű������� ������ ��� �÷����� ù ��° ��Ҹ� ��ȯ�մϴ�.
        //�־��� ������ �����ϴ� ���������� ù ��° ��Ҹ� �˻��ϴ� LINQ�� �޼���
        //new[] {"A","B","C"}.FirstOrDefault();
        // A

        //�� �����̳� �ȿ� �ִ� ������ �߿��� é�� �ѹ��� �Ű����� é�� �ѹ����� ���� �� ����
        //�� ���ǿ� �����ϴ� ù ������Ʈ�� �����ϰų� �ƴϸ� �� ���ǿ� �´� ������Ʈ�� �������� ���� ����
        return ChapterDataTable.Where(item => item.ChapterNo == chapterNo).FirstOrDefault();
        //�� ���̺�ȿ��� ����ֳ�[()�ȿ� �ִ� ���ǿ� �´� ��ġ == ������ Ʈ���϶���]���� ù��° ���� ��ȯ
    }
    #endregion

    #region ITEM_DATA
    //������ ���̺� ���� ���� ������ ����
    private const string ITEM_DATA_TABLE = "ItemDataTable";
    //�����۵����͸� ���� �����̳ʸ� ����
    private List<ItemData> ItemDataTable = new List<ItemData>();

    //������ �����͸� �ε��ϴ� �Լ�
    private void LoadItemDataTable()
    {
        //csv������ �о��
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{ITEM_DATA_TABLE}");

        //�����͸� �����ؼ� ������ �����͸� ����� ����
        foreach(var data in parsedDataTable)
        {
            var itemData = new ItemData
            {
                ItemId = Convert.ToInt32(data["item_id"]),
                ItemName = data["item_name"].ToString(),
                AttackPower = Convert.ToInt32(data["attack_power"]),
                Defense = Convert.ToInt32(data["defense"]),
            };
            ItemDataTable.Add(itemData);
        }

        
    }
    public ItemData GetItemData(int itemid)
    {
        return ItemDataTable.Where(item => item.ItemId == itemid).FirstOrDefault();
    }

    #endregion
}



public class ChapterData
{
    public int ChapterNo; //é�� �ѹ�
    public string ChapterName; // é�͸� ���� �߰�
    public int TotalStages; // é�� �� �������� ����
    public int ChapterRewardGem;//é�͸� Ŭ���� ���� �� �ް� �Ǵ� ����
    public int ChapterRewardGold;//é�͸� Ŭ���� ���� �� �ް� �Ǵ� ���
}
//������ ���̺� �ִ� 4���� �÷� ���� �����ϴ� ������ �������
public class ItemData
{
    public int ItemId;
    public string ItemName;
    public int AttackPower;
    public int Defense;
}
//������ ������ �� enum���� �����ϵ��� ����� ��
public enum ItemType
{
    Weapon = 1,
    Shield,
    ChestArmor,
    Gloves,
    Boots,
    Accessory,
}
//������ ����� �� enum���� �����ϵ��� ����� ��
public enum ItemGrade
{
    Common = 1,
    Uncommon,
    Rare,
    Epic,
    Legendary,
}
