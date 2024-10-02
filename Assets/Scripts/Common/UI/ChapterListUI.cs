using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ChapterListUI : BaseUI
{
    public InfiniteScroll ChapterScrollList; // 스크롤 뷰 선언
    //스크롤 뷰에서 현재 선택중인 챕터(정확히 말하면 선택한 것은 아니지만 화면 정중앙에 있는 챕터)
    //에 대한 정보를 표시해줄 UI요소들의 최상위 오브젝트 변수 선언
    public GameObject SelectedChapterName;
    public TextMeshProUGUI SelectedChapterNameTxt; //현재 선택중인 챕터명을 표시해줄 텍스트 컴포넌트
    public Button SelectBtn; // 선택 버튼 오브젝트도 선언

    private int SelectedChapter;// 현재 선택 중인 챕터 번호를 저장할 변수도 선언


    //SetInfo 함수를 오버라이드해서 필요한 UI 처리를 하자
    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
        //UserPlayData를 가져온다.
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            return;
        }
        //유저가 현재 선택중인 챕터 정보를 받아 온다.
        SelectedChapter = userPlayData.SelectedChapter;

        SetSelectedChapter(); // 현재 선택한 챕터에 대한 UI처리를 해주는 함수 호출
        SetChapterScrollList(); // 챕터 목록 스크롤뷰를 세팅하는 함수를 호출

        //스크롤 뷰를 세팅한 다음에 현재 선택 중인 챕터로 스크롤 위치를 움직여 주겠음.
        //InfiniteScroll에 있는 MoveTo 함수
        //첫번째 매개변수 : 인덱스 번호(그래서-1) , 두번째 매개변수 : 이동 위치(중앙으로 이동하도록)
        ChapterScrollList.MoveTo(SelectedChapter - 1, InfiniteScroll.MoveToType.MOVE_TO_CENTER);
        //스크롤이 끝난 후 가장 가까운 아이템으로 자동 이동하는 기능
        //자동 이동이 끝난 후에 처리를 위해 OnSnap에 람다로 원하는 처리
        ChapterScrollList.OnSnap = (currentSnappedIndex) =>
        {
            var chapterListUI = UIManager.Instance.GetActiveUI<ChapterListUI>() as ChapterListUI;
            if (chapterListUI != null)
            {
                chapterListUI.OnSnap(currentSnappedIndex + 1);
            }
        };
    }
    //현재 선택한 챕터에 대한 UI 처리를 해주는 함수
    private void SetSelectedChapter()
    {
        //게임 내에 추가된 챕터에 해당하면 선택한 챕터UI요소들을 활성화 시켜준다.
        if(SelectedChapter <= GlobalDefine.MAX_CHAPTER)
        {
            SelectedChapterName.SetActive(true);
            SelectBtn.gameObject.SetActive(true);
            //챕터데이터테이블에서 해당 챕터에 대한 데이터를 가져와서 챕터명도 표시
            var itemData = DataTableManager.Instance.GetChapterData(SelectedChapter);
            if(itemData != null)
            {
                SelectedChapterNameTxt.text = itemData.ChapterName;
            }
        }
        else
        //반대로 아직 게임 내에 추가되지 않은 챕터라면 선택한 챕터 요소의 요소들을 비활성화해줌
        {
            SelectedChapterName.SetActive(false);
            SelectBtn.gameObject.SetActive(false);
        }
    }
    //챕터 목록의 스크롤뷰를 셋팅하는 함수
    private void SetChapterScrollList()
    {
        //먼저 스크롤 뷰 내부에 이미 존재하는 아이템이 있다면 삭제
        ChapterScrollList.Clear();
        //1번인덱스부터 MAX_CHAPTER+1까지 순회하면서 아이템을 하나씩 추가
        //(MAX_CHAPTER+1 까지 포함시켜주는 것은 ChapterScrollView 마지막에 ComingSoon이라는 아이템을 만들어 주기 위함)
        for (int i = 1; i <= GlobalDefine.MAX_CHAPTER + 1; i++)
        {
            var chapterItemData = new ChapterScrollItemData();
            chapterItemData.ChapterNo = i; // i+1
            ChapterScrollList.InsertData(chapterItemData);
        }
    }
    //스크롤 후 자동 이동이 끝난 후 처리를 할 함수
    private void OnSnap(int selectedChapter)
    {
        //현재 선택한 챕터를 매개변수로 받은 챕터를 변경해주고 SetSelectedChapter()함수 다시 호출
        SelectedChapter = selectedChapter;
        SetSelectedChapter();
    }
    //선택하기 버튼을 눌렀을 때 실제로 해당 챕터를 유저가 선택하는 처리를 하는 함수
    public void OnClickSelect()
    {
        //UserPlayData를 가져와서
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exist.");
            return;
        }
        //유저가 선택 가능한 챕터(해금한 챕터)라면
        if(SelectedChapter <= userPlayData.MaxClearedChapter + 1)
        {
            //플레이 데이터를 변경해주고
            //로비에 있는 현재 선택한 챕터UI를 갱신해줍니다.
            userPlayData.SelectedChapter = SelectedChapter;
            LobbyManager.Instance.LobbyUIController.SetCurrChapter();
            CloseUI(); // 게임사의 자연스러운 흐름을 위해 이 화면을 받아줌
        }
    }

}
