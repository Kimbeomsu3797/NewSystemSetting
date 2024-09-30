using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
public class CustomTools : Editor
{
    [MenuItem("CustomTools/Add User Gem (+10)")]
    public static void AddUserGem()
    {
        var Gem = long.Parse(PlayerPrefs.GetString("Gem"));
        Gem += 10;

        PlayerPrefs.SetString("Gem", Gem.ToString());
        PlayerPrefs.Save();
    }
   [MenuItem("CustomTools/Add User Gold (+100)")]
   public static void AddUserGold()
    {
        var Gold = long.Parse(PlayerPrefs.GetString("Gold"));
        Gold += 100;

        PlayerPrefs.SetString("Gold", Gold.ToString());
        PlayerPrefs.Save();
    }
}
#endif