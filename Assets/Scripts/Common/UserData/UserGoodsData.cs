using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGoodsData : IUserData
{
    public long Gem { get; set; }
    public long Gold { get; set; }
    public void SetDefaultData()
    {
        Logger.Log($"{GetType()}::SetDefaultData");
        Gem = 0;
        Gold = 0;
    }
    public bool LoadData()
    {
        Logger.Log($"{GetType()}::LoadData");

        bool result = false;
        
        try
        {
            //Gem, Gold 는 long이기 때문에 문자열을 이용해서 데이터를 저장하고 불러와야함
            Gem = long.Parse(PlayerPrefs.GetString("Gem"));
            Gold = long.Parse(PlayerPrefs.GetString("Gold"));
            result = true;
            Logger.Log($"Gem:{Gem} Gold:{Gold}");
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
            PlayerPrefs.SetString("Gem", Gem.ToString());
            PlayerPrefs.SetString("Gold", Gold.ToString());
            PlayerPrefs.Save();
            result = true;
            Logger.Log($"Gem:{Gem} Gold:{Gold}");
        }
        //데이터를 로드하다가 에러가 났을 경우 에러메세지 출력
        catch (System.Exception e)
        {
            Logger.Log("Load failed (" + e.Message + ")");
        }
        return result;
    }

    

}
