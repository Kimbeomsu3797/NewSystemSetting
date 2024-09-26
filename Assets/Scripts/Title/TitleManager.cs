using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TitleManager : MonoBehaviour
{
    //로고
    public Animation LogoAnim;
    public TextMeshProUGUI LogoTxt;

    //타이틀
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
        //유저 데이터 로드
        UserDataManager.Instance.LoadUserData();
        //저장된 유저 데이터가 없으면 기본값으로 세팅 후 저장
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
        //이 코루틴 함수는 게임의 로딩을 처음 시작하는 중요한 함수이기 때문에
        //로그를 찍음.
        //GetType() : 클래스 명을 출력
        //"타이틀 매니저에서 호출하는 로드게임코루틴이라는 함수" 확인
        Logger.Log($"{GetType()}::LoadGameCo");

        LogoAnim.Play(); // 로고애니메이션 재생
        yield return new WaitForSeconds(LogoAnim.clip.length); //애니메이션클립의 길이만큼 대기

        LogoAnim.gameObject.SetActive(false);
        Title.SetActive(true);
        //비동기로 씬을 로딩하는 함수 호출
        m_AsyncOperation = SceneLoader.Instance.LoadSceneAsync(SceneType.Lobby);

        if(m_AsyncOperation == null)
        {
            Logger.Log("Lobby async loading error.");
            yield break; //코루틴 강제로 종료
        }
        //이상없이 잘 반환되어져 왔다면
        //allowSceneActivation false로 지정
        m_AsyncOperation.allowSceneActivation = false;
        //로딩 시간이 짧은 경우 로딩 슬라이더 변화가 너무 빨라 보이지 않을 수 있다
        //일부러 몇 초간 50%로 보여줌으로써 시각적으로 더 자연스럽게 처리한다
        LoadingSlider.value = 0.5f;
        LoadingProgressTxt.text = $"{(int)(LoadingSlider.value * 100)}%";
        yield return new WaitForSeconds(0.5f);

        while(!m_AsyncOperation.isDone) // 로딩이 진행 중 일때
        {
            //로딩 슬라이더 업데이트
            LoadingSlider.value = m_AsyncOperation.progress < 0.5f ? 0.5f : m_AsyncOperation.progress;
            LoadingProgressTxt.text = $"{(int)(LoadingSlider.value * 100)}%";
            
            //씬 로딩이 완료되었다면 로비로 전환하고 코루틴 종료
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
