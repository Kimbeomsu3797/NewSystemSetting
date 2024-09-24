using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : SingletonBehaviour<UserDataManager>
{
    //����� ���� ������ ���� ����
    public bool ExistsSavedData { get; set; }
    //��� ���� ������ �ν��Ͻ��� �����ϴ� �����̳�
    //��� UserDataŬ������ IuserData�������̽��� �����ϱ� ������
    //IUserData Ÿ������ �����̳ʸ� �����ϸ� ��� ����������Ŭ������ �� �����̳ʿ� ������ �� ����.
    public List<IUserData> UserDataList { get; private set; } = new List<IUserData>();

    protected override void Init()
    {
        base.Init(); //Singleton Instance ó���� init�Լ����� ����Ǳ� ������ �������

        //��� ���� �����͸� UserDataList�� �߰�
        UserDataList.Add(new UserSettingData());
        UserDataList.Add(new UserGoodsData());
    }
    //��� ���������͸� �⺻������ �ʱ�ȭ�ϴ� �Լ�
    public void SetDefaultUserData()
    {
        for(int i =0; i< UserDataList.Count; i++)
        {
            UserDataList[i].SetDefaultData();
        }
    }
    //��� ���� ����Ÿ Ŭ������ LoadData�Լ��� ȣ�����ִ� �Լ�
    public void LoadUserData()
    {
        ExistsSavedData = PlayerPrefs.GetInt("ExistsSavedData") == 1 ? true : false;
        //���࿡ ����� �����Ͱ� �����Ѵٸ�
        if (ExistsSavedData)
        {
            //��� ���������� Ŭ������ LoadData�� ȣ��
            for(int i =0; i < UserDataList.Count; i++)
            {
                UserDataList[i].LoadData();
            }
        }
    }
    public void SaveUserData()
    {
        bool hasSaveError = false;
        for(int i =0; i < UserDataList.Count; i++)
        {
            bool isSaveSuccess = UserDataList[i].SaveData(); //save�� ���������� �̷�������� Ȯ��
            if(!isSaveSuccess) //������ ���ٸ�
            {
                hasSaveError = true;
            }
        }
        //�̷��� �Ǹ� ������ ���������� �� ��, ��� ���̺� ������ ������ ��
        //�ϳ��� ������ �߻��� ����������Ŭ������ �ִٸ� hasSaveError = true�� �ɰ���.
        //���̺꿡���� �ϳ��� �߻����� �ʾҴٸ�(���̺갡 ���������� �̷����ٸ�)
        if (!hasSaveError)
        {
            ExistsSavedData = true;
            PlayerPrefs.SetInt("ExistsSavedData", 1);
            //PlayerPrefs.Save(); //���õ���̽��� ����
        }
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
