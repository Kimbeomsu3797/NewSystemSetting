using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

//InventoryItemSlot 인스턴스 생성을 위해 필요한 데이터 클래스
//InfiniteScrollData 상속 : 인피니티 스크롤 컴포넌트를 사용하여 스크롤 아이템을 생성하기 위해(그냥 써보자)
public class InventoryItemSlotData : InfiniteScrollData
{
    //필요한 데이터는 유저 아이템 데이터와 동일하게 시리얼 넘버와 아이템 아이디
    public long SerialNumber;
    public int ItemId;
}

//InFiniteScrollItem 상속 : InfiniteScroll클래스에서 스크롤 데이터와 동일하게
//스크롤 아이템 들이 내부적으로 InfiniteScrollItem 인스턴스로 관리되기 때문
public class InventoryItemSlot : InfiniteScrollItem
{

    public Image itemGradeBg; //등급에 따라 이미지를 처리해줄 백그라운드이미지 컴포넌트
    public Image itemIcon; //아이템이 무엇인지에 따라 아이템 이미지를 처리해줄 아이콘이미지컴포넌트


    //InventoryItemSlotData를 받을 변수
    private InventoryItemSlotData m_InventoryItemSlotData;
    

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    //InfiniteScrollItem에 있는 UpdateData를 오버라이드 해서
    //슬롯 UI에 대한 처리를 하게함
    //인피니티스크롤데이터를 매개변수로 받아와서 ui를 세팅해 줄 함수
    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);
        //스크롤 데이터를 인벤토리아이템슬롯데이터로 변환해서 받는다.
        m_InventoryItemSlotData = scrollData as InventoryItemSlotData;

        //데이터가 null인지 체크하고 null이면 로그
        if(m_InventoryItemSlotData == null)
        {
            Logger.LogError("m_InventoryItemSlotData is InvalidCastException.");
            return;
        }

        //아이템 등급에 따른 백그라운드 이미지 처리
        //(이 수식이면)아이템ID에서 등급 숫자만 추출 가능 , 이것을(ItemGrade) 이넘값으로 변환해서 받아온다.
        var itemGrade = (ItemGrade)((m_InventoryItemSlotData.ItemId / 1000) % 10); //11001
        //이렇게 받아온 이넘값을 그대로 이미지 명으로 사용하면 됨.
        var gradeBgTexture = Resources.Load<Texture2D>($"Textures/{itemGrade}");

        //null체크 이상이 없으면 아이템 그레이드 백그라운드 이미지 컴포넌튿에 해당 텍스쳐를 세팅해 줌
        if(gradeBgTexture != null)
        {
            itemGradeBg.sprite = Sprite.Create(gradeBgTexture, new Rect(0, 0, gradeBgTexture.width, gradeBgTexture.height), new Vector2(1f, 1f));
        }
        //일반등급의 아이템 ID로 이미지를 명명해 두었음. 아이템ID를 등급값만 1로 치환해보겠음
        StringBuilder sb = new StringBuilder(m_InventoryItemSlotData.ItemId.ToString());
        sb[1] = '1'; // 두번째 자리에 강제로 1을 넣어줌.
        var itemIconName = sb.ToString(); //다시 문자열로 변환
        //이렇게 아이콘 이미지 명이 완성
        //이 이미지 명으로 텍스쳐를 로드
        var itemIconTexture = Resources.Load<Texture2D>($"Textures/{itemIconName}");
        //null체크하고 이상이 없으면 아이템 등급 이미지와 마찬가지로 아이콘 이미지 컴포넌트의 텍스처를 세팅해 줌
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
