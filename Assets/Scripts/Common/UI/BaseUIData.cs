using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseUIData
{
    //�Լ��� ���� �� �ִ� ������� ����
    //������ UIȭ�鿡 ���ؼ��� � ��Ȳ������ A��� ����� ����������ϰ�
    //� ��Ȳ������ B��� ����� ��������� �� ���� ����.
    //�׷��� ������ UIȭ�� Ŭ���� �ȿ��� �̷� OnShow�� OnClose�� �����ϴ� �ͺ���
    //�� ȭ���� ���ڴٰ� UI�Ŵ����� ȣ���� �� � ������ ����� ���� �����ؼ�
    //�Ѱ��ִ� ���� �� �����ϰ� ���ϴ� ��ȹ ������ ������ �� �ִ�.

    public Action Onshow; //UIȭ���� ������ �� ���ְ� ���� ������ ����
    public Action OnClose; //UIȭ���� �����鼭 �����ؾ� �Ǵ� ��� ����

    
    
}
public class BaseUI : MonoBehaviour
{
    //UI �����ٶ� ����� �ִϸ��̼� ����
    public Animation m_UIOpenAnim;

    //ȭ���� �� �� �����ؾ��� ���
    //ȭ���� ���� �� �����ؾ��� �׼� ���� ����
    private Action m_OnShow;
    private Action m_OnClose;
    //�� ���� ���� ȭ���� ���� �Ű������� �Ѿ�� UIDataŬ������
    //���ǵ� OnShow�� OnColse �״�� BaseUI Ŭ������ �ִ� m_OnShow�� m_OnClose�� ���Ͽ�
    //m_OnShow = uiData.OnShow; �̷�������

    public virtual void Init(Transform anchor)
    {
        Logger.Log($"{GetType()} init.");

        m_OnShow = null;
        m_OnClose = null;

        transform.SetParent(anchor); // anchor : UIĵ���� ������Ʈ�� Ʈ������

        var rectTransform = GetComponent<RectTransform>();
        if (!rectTransform)
        {
            Logger.LogError("UI does not have rectransform.");
            return;
        }

        //�⺻ ������ ���� �ʱ�ȭ
        rectTransform.localPosition = new Vector3(0f, 0f, 0f);
        rectTransform.localScale = new Vector3(1f, 1f, 1f);
        rectTransform.offsetMin = new Vector2(0, 0);
        rectTransform.offsetMax = new Vector2(0, 0);
    }

    //UIȭ�鿡 UI��Ҹ� �������ִ� �Լ�
    public virtual void SetInfo(BaseUIData uiData)
    {
        Logger.Log($"{GetType()} set info.");

        m_OnShow = uiData.Onshow;
        m_OnClose = uiData.OnClose;
    }

    public virtual void ShowUI()
    {
        if (m_UIOpenAnim)
        {
            m_UIOpenAnim.Play();
        }

        m_OnShow?.Invoke(); // m_OnShow�� null�� �ƴ϶�� m_OnShow����
        /* ? Ű���带 ��� �� ����ó��
          _action?.Invoke(3); // if(_action !=null)�� ?Ű���带 ����Ͽ� null���� üũ*/
        m_OnShow = null; //���� �� null������ �ʱ�ȭ
    }
    public virtual void CloseUI(bool isCloseAll = false)
    {
        //isCloseAll : ���� ��ȯ�ϰų� �Ҷ� �����ִ� ȭ���� ���� �� �ݾ��� �ʿ䰡 ���� ��
        //true�� �Ѱ��༭ ȭ���� ���� �� �ʿ��� ó������ �� �����ϰ� ȭ�鸸 �ݾ��ֱ� ���ؼ� ����Ұ���

        if (!isCloseAll)
        {
            m_OnClose?.Invoke();
        }
        m_OnClose = null;

        UIManager.Instance.CloseUI(this); // ���� �ۼ����� �ʾ����� 
        //CloseUI�� �ٷ� �� �ν��Ͻ��� �Ű������� �־��ش�.
    }

    public virtual void OnClickCloseButton()
    {
        AudioManager.Instance.PlaySFX(SFX.ui_button_click);
        CloseUI();
    }
}
