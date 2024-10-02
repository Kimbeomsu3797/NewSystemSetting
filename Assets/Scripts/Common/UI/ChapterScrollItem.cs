using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterScrollItemData : InfiniteScrollData
{
    public int ChapterNo;
}
public class ChapterScrollItem : InfiniteScrollItem
{
    public GameObject CurrChapter; //é�� ������Ʈ ����
    public RawImage CurrChapterBg; //é���̹��� ���۳�Ʈ ����
    //é�Ͱ� �رݵ��� �ʾ��� �� ǥ������ UI���۳�Ʈ�� ����
    public Image Dim;
    public Image LockIcon;
    public Image Round;
    //���� �������� �ʴ� é��(���� ������Ʈ���� �߰� �� é��)�� ���� UI
    public ParticleSystem ComingSoonFX;
    public TextMeshProUGUI ComingSoonTxt;

    //ChapterScrollItemData�� �޾Ƽ� ��Ƶ� ������ ����
    private ChapterScrollItemData m_ChapterScrollItemData;

    //UI ó���� ���ֱ� ���� ȣ�� �Ǵ� UpdateData �Լ� ���������
    public override void UpdateData(InfiniteScrollData scrollData)
    {
        base.UpdateData(scrollData);
        //�Ű������� ���� ��ũ�� �����͸� �޾� ��
        m_ChapterScrollItemData = scrollData as ChapterScrollItemData;

        if(m_ChapterScrollItemData == null)
        {
            Logger.LogError("Invalid ChapterScrollItemData");
            return;
        }
        //���� ǥ���ؾ� �� é�� �ѹ��� �۷ι� ���ǿ� �ִ� MAX_CHAPTER�� ��
        //�� ���ӳ� �����ϴ� �ִ� é�ͺ��� ũ�ٸ�
        if(m_ChapterScrollItemData.ChapterNo > GlobalDefine.MAX_CHAPTER)
        {
            //é��ǥ��UI�� ��Ȱ��ȭ ó���ϰ�
            // ComingSoon Ȱ��ȭ
            CurrChapter.SetActive(false);
            ComingSoonFX.gameObject.SetActive(true);
            ComingSoonTxt.gameObject.SetActive(true);
        }
        else
        {
            CurrChapter.SetActive(true);
            ComingSoonFX.gameObject.SetActive(false);
            ComingSoonTxt.gameObject.SetActive(false);
            //�����÷��̵����͸� ������
            var userPlayData = UserDataManager.Instance.GetUserData<UserPlayData>();
            if(userPlayData != null)
            {
                //���� �ִ�� Ŭ������ é�Ϳ� ���Ͽ� é���� �ر� ���θ� �Ǵ�
                var isLocked = m_ChapterScrollItemData.ChapterNo > userPlayData.MaxClearedChapter + 1; //���������� �ο��� ���� > ���� Ŭ������ �ִ� é�͸� ���Ͽ� true false�� ����

                //�׸��� �ر� ���ο� ���� �̹��� ������Ʈ�� ó���� ��
                Dim.gameObject.SetActive(isLocked); // ��� = ������ ������� �� �رݵ��� �ʾ����� Ȱ��ȭ
                LockIcon.gameObject.SetActive(isLocked);//��� �����ܵ� �رݵ����ʾ����� Ȱ��ȭ
                //�׵θ� �̹����� �رݵǾ����� ��� , �׷��� ������ ��Ӱ� ó��
                Round.color = isLocked ? new Color(0.5f, 0.5f, 0.5f, 1f) : Color.white;
            }
            //�ش� é�� �ѹ��� �´� ��� �̹����� �ε��Ͽ� ��������
            var bgTexture = Resources.Load($"ChapterBG/Background_{m_ChapterScrollItemData.ChapterNo.ToString("D3")}") as Texture2D;
            if(bgTexture != null)
            {
                CurrChapterBg.texture = bgTexture;
            }
        }
    }

}
