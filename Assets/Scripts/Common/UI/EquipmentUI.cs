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
    public bool IsEquipped; // 해당 아이템이 장착되어 있는지 여부를 구분하는 변수
    //이 변수는 나중에 EquipmentUI(상세UI)화면을 열었을 때 장착되어있는지 여부를 판단하여
    //UI요소를 처리해주는데 사용하고
    //또 상세 UI화면에서 장착처리를 해야될지 탈착처리를 해야 될지를 판단하는데 참조하도록 할 것임.
}
public class EquipmentUI : BaseUI
{
    public Image ItemGradeBg;
    public Image ItemIcon;
    public TextMeshProUGUI ItemGradeTxt;
    public TextMeshProUGUI ItemNameTxt;
    //아이템 스탯을 표시해줄 텍스트 컴퍼넌트
    public TextMeshProUGUI AttackPowerAmountTxt;
    public TextMeshProUGUI DefenseAmountTxt;

    //EquipmentUIData를 받을 변수
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

        //상세 정보를 표시해주기 위해선 아이템 데이터 테이블에서
        //해당 아이템 정보를 가지고 와야 함.
        var itemData = DataTableManager.Instance.GetItemData(m_EquipmentUIData.ItemId);
        if(itemData == null)
        {
            Logger.LogError($"Item data is invalid. ItemId{m_EquipmentUIData.ItemId}");
            return;
        }
        //아이템ID에서 아이템 등급 추출
        var itemGrade = (ItemGrade)((m_EquipmentUIData.ItemId / 1000) % 10);
        //추출한 아이템 등급 정보로 아이템 등급 이미지를 로드
        var gradeBgTexture = Resources.Load<Texture2D>($"Textures/{itemGrade}");

        //이미지가 잘 로드되었으면 ItemGradeBg 컴포넌트에 세팅
        if(gradeBgTexture != null)
        {
            ItemGradeBg.sprite =
                Sprite.Create(gradeBgTexture, new Rect(0, 0, gradeBgTexture.width, gradeBgTexture.height)
                , new Vector2(1f, 1f));
        }
        //아이템 등급을 텍스트로도 표시
        ItemGradeTxt.text = itemGrade.ToString();
        //그 텍스트 컴퍼넌트의 컬러를 아이템 등급 컬러에 맞게 지정
        var hexColor = string.Empty;
        switch (itemGrade)
        {
            case ItemGrade.Common:
                hexColor = "#1AB3FF";
                break;
            case ItemGrade.UnCommon:
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
        //이렇게 컬러값을 지정했으면
        //그 컬러값을 컬러 타입 값으로 변환
        Color color;
        //ColorUtility클래스를 통해서 컬러값을 변환
        //HTML 색상 문자열을 변환하려고 시도한다.
        //hexColor 값을 매개변수로 넣어주면 변환 결과값이
        //out color가 담긴
        //정상적으로 변환이 되었으면 true 값을 반환 컴퍼넌트에 해당 컬러 값을 대입
        if(ColorUtility.TryParseHtmlString(hexColor, out color))
        {
            ItemGradeTxt.color = color;
        }

        //아이템아이콘리소스를 로드해서 세팅
        //이부분은 인벤토리UI슬롯 때와 처리가 동일함.
        StringBuilder sb = new StringBuilder(m_EquipmentUIData.ItemId.ToString());
        sb[1] = '1';
        var itemIconNmae = sb.ToString();

        var itemIconTexture = Resources.Load<Texture2D>($"Textures/{itemIconNmae}");
        if(itemIconTexture != null)
        {
            ItemIcon.sprite = Sprite.Create(itemIconTexture,
                new Rect(0, 0, itemIconTexture.width, itemIconTexture.height), new Vector2(1f, 1f));
        }

        ItemNameTxt.text = itemData.ItemName; // 아이템명 셋팅
        AttackPowerAmountTxt.text = $"+{itemData.AttackPower}"; //공격력 표시
        DefenseAmountTxt.text = $"+{itemData.Defense}"; // 방어력 표시
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
