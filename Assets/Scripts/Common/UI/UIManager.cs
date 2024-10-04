using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonBehaviour<UIManager>
{
    public Transform UICanvasTrs; // (UIȭ���� �������� ĵ���� ������Ʈ Ʈ������)
    //UI ȭ���� �� UI ĵ���� Ʈ������ ������ ��ġ��������ϱ� ������ �ʿ���.

    public Transform ClosedUITrs; // UI ȭ���� ���� �� ��Ȱ��ȭ ��Ų UIȭ����� ��ġ������ Ʈ������

    public Image m_Fade; // ���̵� �̹��� ���۳�Ʈ ���� �߰�

    private BaseUI m_FrontUI; //UI ȭ���� �������� �� ���� ��ܿ� �����ִ� UI

    //���� �����ִ�, �� Ȱ��ȭ �Ǿ��ִ� UIȭ���� ��� �ִ� ����(Ǯ)
    private Dictionary<System.Type, GameObject> m_OpenUIPool = new Dictionary<System.Type, GameObject>();
    //�����ִ�, �� ��Ȱ��ȭ �Ǿ� �ִ� UI ȭ���� ��� �ִ� ����(Ǯ)
    private Dictionary<System.Type, GameObject> m_ClosedUIPool = new Dictionary<System.Type, GameObject>();
    //UI ȭ���� �����ִ��� �����ִ��� ������ �ʿ��ϱ� ������ UIǮ�� ���� ���� 2���� ������ ����

    private GoodsUI m_GoodsUI;

    protected override void Init()
    {
        base.Init();
        
        //������Ʈ �������� 0���� �ؼ� ������ �ʵ��� ó��
        m_Fade.transform.localScale = Vector3.zero;

        //������Ʈ�� ������ ���� ������Ʈ�� ã�Ƽ� GoodsUI���۳�Ʈ�� ����
        m_GoodsUI = FindObjectOfType<GoodsUI>();

        if (!m_GoodsUI)
        {
            Logger.Log("No stats ui component found.");
        }
    }

    //���⸦ ���ϴ� UIȭ���� ���� �ν��Ͻ��� �������� �Լ�
    //out �Լ������� �Ѱ��� ���̳� ������ ��ȯ�� �� �ֱ� ������
    //�������� ���̳� ������ ��ȯ�ϰ� ���� �� �̷��� out �Ű� ������ ���
    //���Լ��� BaseUI, isAlreadyOpen �ΰ��� ���� ��Ȱ�� �� �ִ�.
    private BaseUI GetUI<T>(out bool isAlreadyOpen)
    {
        System.Type uiType = typeof(T); // T�� ������ �ϴ� ȭ�� UI Ŭ���� Ÿ��. �̰� uitype���� �޾ƿ´�.

        BaseUI ui = null;
        isAlreadyOpen = false;

        //���࿡ m_OpenUIPool�� ������ �ϴ� UI�� �����Ѵٸ�
        if (m_OpenUIPool.ContainsKey(uiType))
        {
            //Ǯ�� �ִ� �ش� ������Ʈ�� BaseUI ������Ʈ�� ui������ ����
            ui = m_OpenUIPool[uiType].GetComponent<BaseUI>();
            isAlreadyOpen = true;
        } //�׷��� �ʰ� m_OpenUIPool�� �����Ѵٸ�
        else if (m_ClosedUIPool.ContainsKey(uiType))
        {
            //�ش� Ǯ�� �ִ� BaseUI ������Ʈ�� ui������ ����
            ui = m_ClosedUIPool[uiType].GetComponent<BaseUI>();
            m_ClosedUIPool.Remove(uiType); // Ǯ���� �ش� �ν��Ͻ��� ����
        }
        else//�׷��� �ʰ� �ƿ� �ѹ��� ������ ���� ���� �ν��Ͻ����
        {
            //������ ����
            var uiObj = Instantiate(Resources.Load($"UI/{uiType}", typeof(GameObject))) as GameObject;
            //�������� �̸��� �ݵ�� UI Ŭ������ �̸��� �����ؾ���.
            //�ֳ��ϸ� UIŬ������ �̸����� ��θ� ���� ���ҽ� �������� �ε��ؿ���� ��û�ϱ� ����

            ui = uiObj.GetComponent<BaseUI>();
        }

        return ui;
    }

    //UIȭ���� ���� ����ϴ� �Լ�
    public void OpenUI<T>(BaseUIData uIData)
    {
        System.Type uiType = typeof(T);

        Logger.Log($"{GetType()}::OpenUI({uiType})"); // � UIȭ���� �������ϴ��� �α׸� ��´�.
        bool isAlreadyOpen = false; // �̹� �����ִ��� �� �� �ִ� ���� ����

        var ui = GetUI<T>(out isAlreadyOpen);

        if (!ui) // ������ ���� �α�
        {
            Logger.LogError($"{uiType} does not exist");
            return;
        }
        if(isAlreadyOpen) // �̹� ���������� �̰� ���� ���������� ��û�̶�� �����ϰ� �α�
        {
            Logger.LogError($"{uiType} is already open");
            return;
        }

        //���� ��ȿ�� �˻縦 ����ؼ� ���������� UIȭ���� ���� �� �ִٸ�
        //���� ������ UIȭ���� ���� �����͸� ������ �ش�.
        //���̵带 ��� UI��Һ��� �ֻ����� ���� ���̱� ������ -1 -> -2�� �������.
        var siblingIdx = UICanvasTrs.childCount - 2; //childCount ������ �ִ� ���� ������Ʈ ���� // �θ���� ���ڷ� ���⶧���� -1�̾ƴ� -2�� ����
        //����� ����
        ui.Init(UICanvasTrs); //UIȭ�� �ʱ�ȭ(��ġ���Ѿ��� ��� Ʈ������)

        // ���̶���Ű ���� ���� SetSiblingIndex : �Ű������� �־ ������ ����
        ui.transform.SetSiblingIndex(siblingIdx);
        //����� ����
        //siblingIdx�� 0���� �����ϴµ� 0���� 0,1,2,3
        //�̷��� ������ 1������ �þ��.
        //�����ϰ����ϴ� uiȭ���� �̹� �����Ǿ��ִ� uiȭ��� ����
        //��ܿ� ��ġ���� ����ϱ� ������
        //���� �����ϴ� uicanvastrs ������ ������Ʈ���� ������ �޾ƿͼ�
        //siblingIdx ������ �Ѱ��ִ� ����.
        //siblingidx�� 0���� �����ϱ� ������ childCount�� ���ο� uiȭ���� �߰��� ��
        //���� ū siblingIdx���� �Ǳ� ����
        //���� ��� �ڽ��� 2�� ->0, ������ �����Ǹ� 3���� �Ǵµ� �װ� 1�̵ǰ� �״����� �����Ǹ� 2�� �Ǵ� ������
        //�Ź� ĵ���� �󿡼� ���� �Ʒ��� ��ġ�ϰ� �ǰ� ȭ�鿡���� ���� �ֻ�ܿ� ����ȴ�.
        //ĵ������ �������� �����Ѵ� �����ϴ� �� ���� ���ذ���
        ui.gameObject.SetActive(true); // ������Ʈ�� ������ ���ӿ�����Ʈ Ȱ��ȭ
        ui.SetInfo(uIData); //UIȭ�鿡 ���̴� UI����� �����͸� ����
        ui.ShowUI();

        m_FrontUI = ui; //���� �������ϴ� ȭ�� UI�� ���� ��ܿ� �ִ� UI�� �ɰ��̱� ������ �̷��� ����
        m_OpenUIPool[uiType] = ui.gameObject; //m_OpenUIPool�� ������ UI�ν��Ͻ��� �־��ش�.
    }

    //ȭ���� �ݴ� �Լ�
    public void CloseUI(BaseUI ui)
    {
        System.Type uiType = ui.GetType();

        Logger.Log($"CloseUI : UI:{uiType}"); //� ui�� �ݾ��ִ��� �α�

        ui.gameObject.SetActive(false);

        m_OpenUIPool.Remove(uiType); // ����Ǯ���� ����
        m_ClosedUIPool[uiType] = ui.gameObject; //Ŭ����Ǯ�� �߰�
        ui.transform.SetParent(ClosedUITrs); //ClosedUITrs ������ ��ġ

        m_FrontUI = null; // �ֻ�� UI�� �η� �ʱ�ȭ

        //���� �ֻ�ܿ� �ִ� uiȭ�� ������Ʈ�� �����´�.

        var lastChild = UICanvasTrs.GetChild(UICanvasTrs.childCount - 1);

        //���� ui�� �����Ѵٸ� �� uiȭ�� �ν��Ͻ��� �ֻ�� ui�� ����
        if (lastChild)
        {
            m_FrontUI = lastChild.gameObject.GetComponent<BaseUI>();
        }
    }
    //Ư�� UIȭ���� �����ִ��� Ȯ���ϰ� �� �����ִ� UIȭ���� �������� �Լ�
    public BaseUI GetActiveUI<T>() //�̸� �Ű澲�� ���Ŀ� �̸��� �޶� ������ �߻��߾���. -GetActivePopupUI�̷� �̸��̿���
    {
        var uiType = typeof(T);
        //m_OpenUIPool�� Ư�� ȭ�� �ν��Ͻ��� �����Ѵٸ� �� ȭ�� �ν��Ͻ��� ������ �ְ� �׷��� ������ �� ����
        return m_OpenUIPool.ContainsKey(uiType) ? m_OpenUIPool[uiType].GetComponent<BaseUI>() : null;

    }

    //UIȭ���� �������� �ϳ��� �ִ��� Ȯ���ϴ� �Լ�
    public bool ExistsOpenUI()
    {
        return m_FrontUI != null; //m_FrontUI�� null���� �ƴ��� Ȯ���ؼ� bool���� ��ȯ
    }

    //���� ���� �ֻ�ܿ� �ִ� �ν��Ͻ��� �����ϴ� �Լ�

    public BaseUI GetCurrentFrontUI()
    {
        return m_FrontUI;
    }

    //���� �ֻ�ܿ� �ִ� UIȭ�� �ν��Ͻ��� �ݴ� �Լ�
    public void CloseCurrFrontUI()
    {
        m_FrontUI.CloseUI();
    }

    //�����ִ� ��� UIȭ���� ������� �Լ�

    public void CloseAllOpenUI()
    {
        while (m_FrontUI)
        {
            m_FrontUI.CloseUI(true);
        }
    }

    public void EnalbeGoodsUI(bool value) // => EnableGoodsUI
    {
        m_GoodsUI.gameObject.SetActive(value);

        if (value)
        {
            m_GoodsUI.SetValues();//�Լ��� ȣ���ؼ� ������ ��ȭ ���� ǥ��
        }
    }
    //���̵� �Լ�
    public void Fade(Color color, float startAlpha, float endAlpha, float duration, float startDelay, bool deactiveOnFinish, Action onFinish = null)
    {
        StartCoroutine(FadeCo(color, startAlpha, endAlpha, duration, startDelay, deactiveOnFinish, onFinish));
    }
    //���̵� ó�� �ڷ�ƾ
    private IEnumerator FadeCo(Color color, float startAlpha, float endAlpha, float duration, float startDelay, bool deactiveOnFinish, Action onFinish)
    {
        yield return new WaitForSeconds(startDelay);

        m_Fade.transform.localScale = Vector3.one;
        m_Fade.color = new Color(color.r, color.g, color.b, startAlpha);

        var startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < duration)
        {
            m_Fade.color = new Color(color.r, color.g, color.b, Mathf.Lerp(startAlpha , endAlpha, (Time.realtimeSinceStartup - startTime) / duration));
            yield return null;
        }

        m_Fade.color = new Color(color.r, color.g, color.b, endAlpha);

        if (deactiveOnFinish)
        {
            m_Fade.transform.localScale = Vector3.zero;
        }
        onFinish?.Invoke(); // ���̵� ó���� ������ �� ����Ǳ� ���ϴ� ������ �ִٸ�
        //���̵尡 ������ ���ϴ� ó���� ���������� ó���� �� �ֵ���
        //ó���� ���ֵ��� �Ѵ�.
    }
    public void CancelFade()
    {
        m_Fade.transform.localScale = Vector3.zero;
    }
}
