using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ChapterListUI : BaseUI
{
    public InfiniteScroll ChapterScrollList; // ��ũ�� �� ����
    //��ũ�� �信�� ���� �������� é��(��Ȯ�� ���ϸ� ������ ���� �ƴ����� ȭ�� ���߾ӿ� �ִ� é��)
    //�� ���� ������ ǥ������ UI��ҵ��� �ֻ��� ������Ʈ ���� ����
    public GameObject SelectedChapterName;
    public TextMeshProUGUI SelectedChapterNameTxt; //���� �������� é�͸��� ǥ������ �ؽ�Ʈ ������Ʈ
    public Button SelectBtn; // ���� ��ư ������Ʈ�� ����

    private int SelectedChapter;// ���� ���� ���� é�� ��ȣ�� ������ ������ ����


    //SetInfo �Լ��� �������̵��ؼ� �ʿ��� UI ó���� ����
    public override void SetInfo(BaseUIData uiData)
    {
        base.SetInfo(uiData);
        //UserPlayData�� �����´�.
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            return;
        }
        //������ ���� �������� é�� ������ �޾� �´�.
        SelectedChapter = userPlayData.SelectedChapter;

        SetSelectedChapter(); // ���� ������ é�Ϳ� ���� UIó���� ���ִ� �Լ� ȣ��
        SetChapterScrollList(); // é�� ��� ��ũ�Ѻ並 �����ϴ� �Լ��� ȣ��

        //��ũ�� �並 ������ ������ ���� ���� ���� é�ͷ� ��ũ�� ��ġ�� ������ �ְ���.
        //InfiniteScroll�� �ִ� MoveTo �Լ�
        //ù��° �Ű����� : �ε��� ��ȣ(�׷���-1) , �ι�° �Ű����� : �̵� ��ġ(�߾����� �̵��ϵ���)
        ChapterScrollList.MoveTo(SelectedChapter - 1, InfiniteScroll.MoveToType.MOVE_TO_CENTER);
        //��ũ���� ���� �� ���� ����� ���������� �ڵ� �̵��ϴ� ���
        //�ڵ� �̵��� ���� �Ŀ� ó���� ���� OnSnap�� ���ٷ� ���ϴ� ó��
        ChapterScrollList.OnSnap = (currentSnappedIndex) =>
        {
            var chapterListUI = UIManager.Instance.GetActiveUI<ChapterListUI>() as ChapterListUI;
            if (chapterListUI != null)
            {
                chapterListUI.OnSnap(currentSnappedIndex + 1);
            }
        };
    }
    //���� ������ é�Ϳ� ���� UI ó���� ���ִ� �Լ�
    private void SetSelectedChapter()
    {
        //���� ���� �߰��� é�Ϳ� �ش��ϸ� ������ é��UI��ҵ��� Ȱ��ȭ �����ش�.
        if(SelectedChapter <= GlobalDefine.MAX_CHAPTER)
        {
            SelectedChapterName.SetActive(true);
            SelectBtn.gameObject.SetActive(true);
            //é�͵��������̺��� �ش� é�Ϳ� ���� �����͸� �����ͼ� é�͸� ǥ��
            var itemData = DataTableManager.Instance.GetChapterData(SelectedChapter);
            if(itemData != null)
            {
                SelectedChapterNameTxt.text = itemData.ChapterName;
            }
        }
        else
        //�ݴ�� ���� ���� ���� �߰����� ���� é�Ͷ�� ������ é�� ����� ��ҵ��� ��Ȱ��ȭ����
        {
            SelectedChapterName.SetActive(false);
            SelectBtn.gameObject.SetActive(false);
        }
    }
    //é�� ����� ��ũ�Ѻ並 �����ϴ� �Լ�
    private void SetChapterScrollList()
    {
        //���� ��ũ�� �� ���ο� �̹� �����ϴ� �������� �ִٸ� ����
        ChapterScrollList.Clear();
        //1���ε������� MAX_CHAPTER+1���� ��ȸ�ϸ鼭 �������� �ϳ��� �߰�
        //(MAX_CHAPTER+1 ���� ���Խ����ִ� ���� ChapterScrollView �������� ComingSoon�̶�� �������� ����� �ֱ� ����)
        for (int i = 1; i <= GlobalDefine.MAX_CHAPTER + 1; i++)
        {
            var chapterItemData = new ChapterScrollItemData();
            chapterItemData.ChapterNo = i; // i+1
            ChapterScrollList.InsertData(chapterItemData);
        }
    }
    //��ũ�� �� �ڵ� �̵��� ���� �� ó���� �� �Լ�
    private void OnSnap(int selectedChapter)
    {
        //���� ������ é�͸� �Ű������� ���� é�͸� �������ְ� SetSelectedChapter()�Լ� �ٽ� ȣ��
        SelectedChapter = selectedChapter;
        SetSelectedChapter();
    }
    //�����ϱ� ��ư�� ������ �� ������ �ش� é�͸� ������ �����ϴ� ó���� �ϴ� �Լ�
    public void OnClickSelect()
    {
        //UserPlayData�� �����ͼ�
        var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
        if(userPlayData == null)
        {
            Logger.LogError("UserPlayData does not exist.");
            return;
        }
        //������ ���� ������ é��(�ر��� é��)���
        if(SelectedChapter <= userPlayData.MaxClearedChapter + 1)
        {
            //�÷��� �����͸� �������ְ�
            //�κ� �ִ� ���� ������ é��UI�� �������ݴϴ�.
            userPlayData.SelectedChapter = SelectedChapter;
            LobbyManager.Instance.LobbyUIController.SetCurrChapter();
            CloseUI(); // ���ӻ��� �ڿ������� �帧�� ���� �� ȭ���� �޾���
        }
    }

}
