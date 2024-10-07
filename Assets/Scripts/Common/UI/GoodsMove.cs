using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsMove : MonoBehaviour
{
    public float MoveSpeed = 5f;//�̵� ����
    private Vector3 m_DestPosition;//�̵��ؾ��� ��ġ ����
    private Transform m_Transform;//�� ������Ʈ�� Ʈ�������� ���� ����
    private RectTransform m_RectTransform; //��Ʈ Ʈ�������� ���� ����
    
    //�̵��� ó���϶�� ��û�� �Լ�, �Ű����� : �ε��� ���� ������ ��ġ
    public void SetMove(int idx, Vector3 destPosition)
    {
        m_Transform = transform;
        m_RectTransform = GetComponent<RectTransform>();
        m_DestPosition = new Vector3(destPosition.x, destPosition.y, 0);

        StartCoroutine(MoveCo(idx)); // ������ �� ������ �̵��� ������ �ڷ�ƾ �Լ� ȣ��
    }
    private IEnumerator MoveCo(int idx)
    {
        //�̵��ϱ� ���� ����� �ð��� �ε��� ���� ���� ���
        //�̵��� ��ȭ �ν��Ͻ����� ���ÿ� �̵����� �ʰ� ���� ������ ������
        //���ʷ� �̵��ϵ��� ó�����ֱ� ����
        yield return new WaitForSeconds(0.1f + 0.08f * idx);

        //�� ������Ʈ�� ���� ��ġ���� ������ �� ������ Ȯ���ϸ鼭
        //���� ��ġ�� �������� �ʾҴٸ� �� ������ �̵�������
        while(m_Transform.position.y < m_DestPosition.y)
        {
            m_Transform.position = Vector2.MoveTowards(m_Transform.position, m_DestPosition, MoveSpeed * Time.deltaTime);
            var rectLocalPosition = m_RectTransform.localPosition;
            m_RectTransform.localPosition = new Vector3(rectLocalPosition.x, rectLocalPosition.y, 0f); // z���� 0���� ����

            yield return null;
        }
        Destroy(gameObject);
    }
   
}
