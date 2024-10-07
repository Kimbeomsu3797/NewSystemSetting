using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsMove : MonoBehaviour
{
    public float MoveSpeed = 5f;//이동 변수
    private Vector3 m_DestPosition;//이동해야할 위치 변수
    private Transform m_Transform;//이 오브젝트의 트랜스폼을 담을 변수
    private RectTransform m_RectTransform; //렉트 트랜스폼을 담을 변수
    
    //이동을 처리하라는 요청의 함수, 매개변수 : 인덱스 값과 목적지 위치
    public void SetMove(int idx, Vector3 destPosition)
    {
        m_Transform = transform;
        m_RectTransform = GetComponent<RectTransform>();
        m_DestPosition = new Vector3(destPosition.x, destPosition.y, 0);

        StartCoroutine(MoveCo(idx)); // 실제로 매 프레임 이동을 시켜줄 코루틴 함수 호출
    }
    private IEnumerator MoveCo(int idx)
    {
        //이동하기 전에 대기할 시간을 인덱스 값에 따라 계산
        //이동할 재화 인스턴스들이 동시에 이동하지 않고 일정 간격을 가지고
        //차례로 이동하도록 처리해주기 위함
        yield return new WaitForSeconds(0.1f + 0.08f * idx);

        //이 오브젝트가 목적 위치까지 갔는지 매 프레임 확인하면서
        //목적 위치에 도달하지 않았다면 매 프레임 이동시켜줌
        while(m_Transform.position.y < m_DestPosition.y)
        {
            m_Transform.position = Vector2.MoveTowards(m_Transform.position, m_DestPosition, MoveSpeed * Time.deltaTime);
            var rectLocalPosition = m_RectTransform.localPosition;
            m_RectTransform.localPosition = new Vector3(rectLocalPosition.x, rectLocalPosition.y, 0f); // z값은 0으로 보정

            yield return null;
        }
        Destroy(gameObject);
    }
   
}
