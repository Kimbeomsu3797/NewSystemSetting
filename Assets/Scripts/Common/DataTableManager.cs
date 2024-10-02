using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataTableManager : SingletonBehaviour<DataTableManager>
{
    //데이터 테이블 파일들이 들어있는 경로를 String값으로 설정
    private const string DATA_PATH = "DataTable";


    protected override void Init()
    {
        base.Init();

        LoadChapterDataTable();
        LoadItemDataTable();
    }
    #region CHAPTER_DATA
    //챕터 데이터 테이블 파일명을 갖는 String 변수
    private const string CHAPTER_DATA_TABLE = "ChapterDataTable";
    //모든 챕터 데이터를 저장할 수 있는 컨테이너 즉, 자료구조를 선언
    private List<ChapterData> ChapterDataTable = new List<ChapterData>();
    //챕터데이타테이블을 로드하는 함수
    private void LoadChapterDataTable()
    {
        //CSVReader.Read()는 리스트를 반환하는 함수
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{CHAPTER_DATA_TABLE}");
        //var 타입 : 데이터 타입을 신경쓰지 않아도 알아서 변수에 있는 값을 판단해서 타입을 인식하는 타입
        //지역변수로만 사용 가능
        //반드시 선언과 동시에 초기화(그래야 실질적으로 데이터 타입이 정해짐)
        //타입이 복잡한 경우 지역변수로 사용할때는 var을 사용해도 괜찮다.
        

        //테이블을 순회하면서 각 데이터를
        //ChapterData인스턴스로 만들어서
        //ChapterDataTable 컨테이너에 넣어줌
        foreach(var data in parsedDataTable)
        {
            var chapterData = new ChapterData
            {
                //오브젝트 타입이라 지정된 개체의 값을 32비트 부호 있는 정수로 변환
                ChapterNo = Convert.ToInt32(data["chapter_no"]),
                ChapterName = data["chapter_name"].ToString(), // ChapterName변수에 로드한 데이터 값을 대입해주는 코드 추가
                TotalStages = Convert.ToInt32(data["total_stages"]),
                ChapterRewardGem = Convert.ToInt32(data["chapter_reward_gem"]),
                ChapterRewardGold = Convert.ToInt32(data["chapter_reward_gold"]),
            };
            ChapterDataTable.Add(chapterData);
        }
    }
    //이렇게 로드한 ChapterDataTable에서 찾고자 하는 ChapterData만 가져오는 함수
    public ChapterData GetChapterData(int chapterNo)
    {
        //특정 챕터 넘버로 챕터 데이터 테이블을 검색해서
        //그 챕터 넘버에 해당하는 데이터를 반환하는 함수
        //링크 사용 -> 링크 : 검색, 변경을 좀 더 용이하게 해주는 기능
        //만약 링크를 사용하지 않는다면
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
        //.Where 조건식이 true인 요소만 필터링
        //FirstOrDefault() 함수 : 매개변수가 생략된 경우 컬렉션의 첫 번째 요소를 반환합니다.
        //주어진 조건을 만족하는 시퀀스에서 첫 번째 요소를 검색하는 LINQ의 메서드
        //new[] {"A","B","C"}.FirstOrDefault();
        // A

        //이 컨테이너 안에 있는 아이템 중에서 챕터 넘버가 매개변수 챕터 넘버값과 같을 시 리턴
        //이 조건에 부합하는 첫 엘리먼트를 리턴하거나 아니면 이 조건에 맞는 엘리먼트가 없을때는 널을 리턴
        return ChapterDataTable.Where(item => item.ChapterNo == chapterNo).FirstOrDefault();
        //이 테이블안에서 어디에있냐[()안에 있는 조건에 맞는 위치 == 조건이 트루일때만]에서 첫번째 값을 반환
    }
    #endregion

    #region ITEM_DATA
    //데이터 테이블 명을 가진 변수를 선언
    private const string ITEM_DATA_TABLE = "ItemDataTable";
    //아이템데이터를 담을 컨테이너를 선언
    private List<ItemData> ItemDataTable = new List<ItemData>();

    //아이템 데이터를 로드하는 함수
    private void LoadItemDataTable()
    {
        //csv파일을 읽어옮
        var parsedDataTable = CSVReader.Read($"{DATA_PATH}/{ITEM_DATA_TABLE}");

        //데이터를 참고해서 아이템 데이터를 만들어 주자
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
    public int ChapterNo; //챕터 넘버
    public string ChapterName; // 챕터명 변수 추가
    public int TotalStages; // 챕터 내 스테이지 개수
    public int ChapterRewardGem;//챕터를 클리어 했을 시 받게 되는 보석
    public int ChapterRewardGold;//챕터를 클리어 했을 시 받게 되는 골드
}
//아이템 테이블에 있던 4개의 컬럼 값에 대응하는 변수를 만들어줌
public class ItemData
{
    public int ItemId;
    public string ItemName;
    public int AttackPower;
    public int Defense;
}
//아이템 종류가 이 enum값에 상응하도록 만들어 줌
public enum ItemType
{
    Weapon = 1,
    Shield,
    ChestArmor,
    Gloves,
    Boots,
    Accessory,
}
//아이템 등급이 이 enum값에 상응하도록 만들어 줌
public enum ItemGrade
{
    Common = 1,
    Uncommon,
    Rare,
    Epic,
    Legendary,
}
