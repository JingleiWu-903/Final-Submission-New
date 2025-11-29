using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float speed = 20f;  // �����ٶ�
    public float lifeTime = 5f;  // ��������ڵ�ʱ��

    private void Start()
    {
        // ������������ķ���
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * speed; // ʹ���尴һ���ٶ���ǰ��������
        }

        // �������������������
        Destroy(gameObject, lifeTime);  // ��������ָ����ʱ�������
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �����������������巢����ײʱ�Ĵ����߼�
        // ��ײ����������ʱ�ݻ���������ɺ��
        if (collision.gameObject.CompareTag("LargeTrash"))
        {
            Destroy(collision.gameObject); // ���ٴ�������
            // ����ײ������ɺ��
            GameObject coral = Instantiate(Resources.Load("CoralPrefab")) as GameObject;
            coral.transform.position = collision.transform.position;  // ����ɺ����λ��

        }
    }
}