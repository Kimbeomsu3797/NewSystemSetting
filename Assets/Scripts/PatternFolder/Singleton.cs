using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        //get접근자로 public ctatic 속성을 구현했다.
        //이 접근자에서 새로운 인스턴스를 초기화하기 전에 다른 인스턴스를 확인한다
        get
        {
            if(_instance == null)
            {
                //findobjectoftype은 지정한 타입의 첫번째로 로드된 오브젝트를 검색
                _instance = FindObjectOfType<T>();
                //찾을수없다면 새로운 gameobj를 생성하고 이름변경 후 지정되지않은 유형의 컴퍼넌트를 추가
                if(_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
        
    }
    //싱글턴 클래스의 마지막부분 구현
    //파생 클래스에서 다시 정의할 수 있다는 의미인 virtual awake메서드
    //awake에서 메모리에 초기화된 자신의 인스턴스가 이미 있는지 확인
    //이미 있다면 복제를 막기위해 스스로 제거
    public virtual void Awake()
    {
        if(_instance == null)
        {
            _instance = this as T;
            //이게 인스턴스라면
            //이거 제거하는걸 막고
            //게임 씬이 전환되어도 유지되도록하기 위하여 이렇게 작성
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
