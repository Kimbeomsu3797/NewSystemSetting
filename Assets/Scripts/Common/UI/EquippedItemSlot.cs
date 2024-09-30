using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class EquippedItemSlot : MonoBehaviour
{
    //장착된 아이템이 없을 때 표시해줄[+] 아이콘 변수
    public Image AddIcon;
    //장착된 아이템이 있을 때 표시해줄 아이템 아이콘 변수
    public Image EquippedItemIcon;

    //장착한 아이템 데이터를 저장할 변수
    private UserItemData m_EquippedItemData;
    //장착된 아이템이 있을시 UI요소를 처리해 주는 함수
    public void SetItem(UserItemData userItemData)//매개변수로 장착할 아이템 데이터를 넘겨준다
    {
        m_EquippedItemData = userItemData;
        AddIcon.gameObject.SetActive(false); //장착될 아이템이 없을 때 표시해줄 [+] 아이콘 비활성화
        EquippedItemIcon.gameObject.SetActive(true);//장착될 아이템이 있을 때 표시해줄 아이템 아이콘 활성화

        //아이템ID에 맞는 이미지를 로드해서 아이템 컴퍼넌트에 세팅
        //텍스처가 잘 로드되었으면 아이템아이콘이미지컴퍼넌트에 세팅
        StringBuilder sb = new StringBuilder(m_EquippedItemData.ItemId.ToString());
        sb[1] = '1';

        var itemIconName = sb.ToString();

        var itemIconTexture = Resources.Load<Texture2D>($"Textures/{itemIconName}");
        if(itemIconTexture != null)
        {
            EquippedItemIcon.sprite = Sprite.Create(itemIconTexture,
                new Rect(0, 0, itemIconTexture.width, itemIconTexture.height), new Vector2(1f, 1f));
        }
    }
    //반대로 장착한 아이템이 없을 때 처리하는 UI요소를 초기화 하는 함수
    public void ClearItem()
    {
        m_EquippedItemData = null;
        AddIcon.gameObject.SetActive(true);
        EquippedItemIcon.gameObject.SetActive(false);
    }

    //일반 인벤토리 아이템 슬롯과 마찬가지로
    //이 장착아이템 슬롯도 클릭하면 아이템 상세UI(EquipmentUI)를 열어주는 처리
    public void OnClickEquippedItemSlot()
    {
        //유저인벤토리데이터를 EquipmentUIData로 설정
        var uiData = new EquipmentUIData();
        uiData.SerialNumber = m_EquippedItemData.SerialNumber;
        uiData.ItemId = m_EquippedItemData.ItemId;
        //F12를 눌러 EquipmentUIData가 정의되어 있는 곳으로 가서 변수 추가 해주자
        //IsEquipped
        uiData.IsEquipped = true;
        UIManager.Instance.OpenUI<EquipmentUI>(uiData);
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
