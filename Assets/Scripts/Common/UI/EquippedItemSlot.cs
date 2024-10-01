using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class EquippedItemSlot : MonoBehaviour
{
    //������ �������� ���� �� ǥ������[+] ������ ����
    public Image AddIcon;
    public Image EquippedItemGradeBg; // ��� �̹��� ���۳�Ʈ ����
    //������ �������� ���� �� ǥ������ ������ ������ ����
    public Image EquippedItemIcon;

    //������ ������ �����͸� ������ ����
    private UserItemData m_EquippedItemData;
    //������ �������� ������ UI��Ҹ� ó���� �ִ� �Լ�
    public void SetItem(UserItemData userItemData)//�Ű������� ������ ������ �����͸� �Ѱ��ش�
    {
       
        m_EquippedItemData = userItemData;
        AddIcon.gameObject.SetActive(false); //������ �������� ���� �� ǥ������ [+] ������ ��Ȱ��ȭ
        EquippedItemGradeBg.gameObject.SetActive(true);
        EquippedItemIcon.gameObject.SetActive(true);//������ �������� ���� �� ǥ������ ������ ������ Ȱ��ȭ

        //������ ��޿� �´� �̹����� �ε��ؼ� ����
        var itemGrade = (ItemGrade)((m_EquippedItemData.ItemId / 1000) % 10);
        
        var gradeBgTexture = Resources.Load<Texture2D>($"Textures/{itemGrade}");
        if(gradeBgTexture != null)
        {
            EquippedItemGradeBg.sprite = Sprite.Create
                (gradeBgTexture, new Rect(0, 0, gradeBgTexture.width, gradeBgTexture.height), new Vector2(1f, 1f));
        }
        
        //������ID�� �´� �̹����� �ε��ؼ� ������ ���۳�Ʈ�� ����
        //�ؽ�ó�� �� �ε�Ǿ����� �����۾������̹������۳�Ʈ�� ����
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
    //�ݴ�� ������ �������� ���� �� ó���ϴ� UI��Ҹ� �ʱ�ȭ �ϴ� �Լ�
    public void ClearItem()
    {
        m_EquippedItemData = null;
        AddIcon.gameObject.SetActive(true);
        EquippedItemGradeBg.gameObject.SetActive(false);
        EquippedItemIcon.gameObject.SetActive(false);
    }

    //�Ϲ� �κ��丮 ������ ���԰� ����������
    //�� ���������� ���Ե� Ŭ���ϸ� ������ ��UI(EquipmentUI)�� �����ִ� ó��
    public void OnClickEquippedItemSlot()
    {
        //�����κ��丮�����͸� EquipmentUIData�� ����
        var uiData = new EquipmentUIData();
        uiData.SerialNumber = m_EquippedItemData.SerialNumber;
        uiData.ItemId = m_EquippedItemData.ItemId;
        //F12�� ���� EquipmentUIData�� ���ǵǾ� �ִ� ������ ���� ���� �߰� ������
        //IsEquipped
        if (uiData == null)
        {
            uiData.IsEquipped = false;
        }
        else
        {
            uiData.IsEquipped = true;
        }
        UIManager.Instance.OpenUI<EquipmentUI>(uiData);
    }
 
}
