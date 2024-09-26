using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TitleManager : MonoBehaviour
{
    //�ΰ�
    public Animation LogoAnim;
    public TextMeshProUGUI LogoTxt;

    //Ÿ��Ʋ
    public GameObject Title;
    public Slider LoadingSlider;
    public TextMeshProUGUI LoadingProgressTxt;

    private AsyncOperation m_AsyncOperation;
    private void Awake()
    {
        LogoAnim.gameObject.SetActive(true);
        Title.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        //���� ������ �ε�
        UserDataManager.Instance.LoadUserData();
        //����� ���� �����Ͱ� ������ �⺻������ ���� �� ����
        if (!UserDataManager.Instance.ExistsSavedData)
        {
            UserDataManager.Instance.SetDefaultUserData();
            UserDataManager.Instance.SaveUserData();
        }
        ChapterData chapterData1 = DataTableManager.Instance.GetChapterData(10);
        ChapterData chapterData2 = DataTableManager.Instance.GetChapterData(50);
        Logger.Log(chapterData1.ToString());
        Logger.Log(chapterData2.ToString());
        /*var confirmUIData = new confirmUIData();
        confirmUIData.ConfirmType = ConfirmType.OK_CANCEL;
        confirmUIData.Titletxt = "UI Test";
        confirmUIData.DescTxt = "This is UI Test";
        confirmUIData.OKBtnTxt = "OK";
        confirmUIData.CancleBtnTxt = "Cancle";
        UIManager.Instance.OpenUI<ConfirmUI>(confirmUIData);*/
        
        StartCoroutine(LoadGameGo());
    }

    private IEnumerator LoadGameGo()
    {
        //�� �ڷ�ƾ �Լ��� ������ �ε��� ó�� �����ϴ� �߿��� �Լ��̱� ������
        //�α׸� ����.
        //GetType() : Ŭ���� ���� ���
        //"Ÿ��Ʋ �Ŵ������� ȣ���ϴ� �ε�����ڷ�ƾ�̶�� �Լ�" Ȯ��
        Logger.Log($"{GetType()}::LoadGameCo");

        LogoAnim.Play(); // �ΰ�ִϸ��̼� ���
        yield return new WaitForSeconds(LogoAnim.clip.length); //�ִϸ��̼�Ŭ���� ���̸�ŭ ���

        LogoAnim.gameObject.SetActive(false);
        Title.SetActive(true);
        //�񵿱�� ���� �ε��ϴ� �Լ� ȣ��
        m_AsyncOperation = SceneLoader.Instance.LoadSceneAsync(SceneType.Lobby);

        if(m_AsyncOperation == null)
        {
            Logger.Log("Lobby async loading error.");
            yield break; //�ڷ�ƾ ������ ����
        }
        //�̻���� �� ��ȯ�Ǿ��� �Դٸ�
        //allowSceneActivation false�� ����
        m_AsyncOperation.allowSceneActivation = false;
        //�ε� �ð��� ª�� ��� �ε� �����̴� ��ȭ�� �ʹ� ���� ������ ���� �� �ִ�
        //�Ϻη� �� �ʰ� 50%�� ���������ν� �ð������� �� �ڿ������� ó���Ѵ�
        LoadingSlider.value = 0.5f;
        LoadingProgressTxt.text = $"{(int)(LoadingSlider.value * 100)}%";
        yield return new WaitForSeconds(0.5f);

        while(!m_AsyncOperation.isDone) // �ε��� ���� �� �϶�
        {
            //�ε� �����̴� ������Ʈ
            LoadingSlider.value = m_AsyncOperation.progress < 0.5f ? 0.5f : m_AsyncOperation.progress;
            LoadingProgressTxt.text = $"{(int)(LoadingSlider.value * 100)}%";
            
            //�� �ε��� �Ϸ�Ǿ��ٸ� �κ�� ��ȯ�ϰ� �ڷ�ƾ ����
            if(m_AsyncOperation.progress >= 0.9f)
            {
                m_AsyncOperation.allowSceneActivation = true;
                yield break;
            }
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
