using System;
using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T> //���׸� Ŭ����
{
    //�� ��ȯ �� �������� ����
    protected bool m_IsDestroyOnLoad = false;

    //�� Ŭ������ ����ƽ �ν��Ͻ� ����
    protected static T m_Instance;

    public static T Instance
    {
        get { return m_Instance; }
    }

    private void Awake()
    {
        Init();        
    }
    //�����Լ�
    //��[SingletonBehaviour]Ŭ������ ����ؼ� ����� Ŭ��������
    //�� �Լ��� Ȯ���ؼ� �������� �ٸ� ó������� �߰��� �� �ְ� ���ַ��� �ǵ�
    protected virtual void Init()
    {
        if(m_Instance == null)
        {
            m_Instance = (T)this;
            if (!m_IsDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }
        }
        else
        {
            //null�� �ƴѵ� init�Լ��� ȣ���ϰ� �ȴٸ�
            //�̹� �ν��Ͻ��� �ִµ� �ٸ� �ν��Ͻ��� �� ����� �ַ��� �ǵ���� �Ǵ��ؼ�
            //�׷��� �Ϸ��� �� �ν��Ͻ� ��ü�� �������ֵ��� �Ѵ�.
            Destroy(gameObject);
        }
    }
    protected virtual void OnDestroy()
    {
        Dispose();
    }

    protected virtual void Dispose()
    {
        m_Instance = null;
    }
}
