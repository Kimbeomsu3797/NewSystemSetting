using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager1 : MonoBehaviour
{
    private DateTime _sessionStartTime;
    private DateTime _sessionEndTime;

    private void Start()
    {
        // Todo: (잠재적인 작업 목록)
        //플레이어 세이브 로드에 시간 참조
        _sessionStartTime = DateTime.Now;
        
    }
    private void OnApplicationQuit()
    {
        //게임 끌때 시간 참조
        //subtract(timespan) 이 인스턴스의 값에서 지정된 기간을 뺀 새 datetime을 반환합니다.
        //subtract(datetime) 이 인스턴스의 값에서 지정된 날짜와 시간을 뺀 새 timespan을 반환합니다.
        _sessionEndTime = DateTime.Now;
        TimeSpan timeDifference = _sessionEndTime.Subtract(_sessionStartTime);

    }
    private void OnGUI()
    {
        if(GUILayout.Button("Next Scene"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
