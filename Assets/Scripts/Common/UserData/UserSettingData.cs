using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//IUserData 잠재적 수정사항을 표시해서 인터페이스 구현
public class UserSettingData : IUserData
{
    //사운드 on/off 여부
    public bool Sound { get; set; }
    public void SetDefaultData()
    {
        //GetType()을 호출해 클래스명을 출력하고 함수명을 그대로 출력
        Logger.Log($"{GetType()}::SetDefaultData");
        Sound = true;
    }
    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");

        bool result = false;
        //트라이 캐치문을 작성할 때 try치고 탭 두번 시 자동으로 지문 장성
        try
        {
            //플레이어 프랩스가 불값은 제공하지 않기 때문에 정수값 비교해서 bool로 변경
            //플레이어 프리팹은 정수, 스트링, float만 저장 가능
            Sound = PlayerPrefs.GetInt("Sound") == 1 ? true : false;
            result = true;
            Logger.Log($"Sound:{Sound}");
        }
        //데이터를 로드하다가 에러가 났을 경우 에러메세지 출력
        catch (System.Exception e)
        {
            Logger.Log("Load failed (" + e.Message + ")");
        }
        return result;
    }

    public bool SaveData()
    {
        Logger.Log($"{GetType()}::SaveData");

        bool result = false;

        try
        {
            PlayerPrefs.SetInt("Sound", Sound ? 1 : 0); //사운드가 트루면 1 아니면 0
            PlayerPrefs.Save();
            result = true;
            Logger.Log($"Sound:{Sound}");
        }
        catch (System.Exception e)
        {
            Logger.Log("Load failed (" + e.Message + ")");
        }
        return result;
    }

    

}
