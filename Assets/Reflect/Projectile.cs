using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float speed = 5;                // �߻�ü�� �̵� �ӵ�
    private Vector3 direction;      // �߻�ü�� �̵� ����
    private float halfSize;         // �߻�ü�� x�� ���� ũ��

    public void Setup(Vector3 direction)
    {
        this.direction = direction;
        halfSize = this.transform.localScale.x * 0.5f;
    }

    private void Update()
    {
        this.transform.position += direction * speed * Time.deltaTime;

        // this.transform.position ��ġ���� direction �������� halfSize ������ ������ �߻���.
        if ( Physics.Raycast(this.transform.position, direction, out RaycastHit hit, halfSize) )
        {
            // �Ի� ���� direction�� ǥ�� ���� hit.normal ������ �������� �ݻ� ���� direction�� ����.
            direction = Vector3.Reflect(direction, hit.normal);
        }
    }

    /*// � ������Ʈ�� ����� ��
    private void OnCollisionEnter(Collision collision)
    {
        // �Ի� ���� direction�� ǥ�� ���� collision.GetContact(0).normal ������ �������� �ݻ� ���� direction�� ����.
        direction = Vector3.Reflect(direction, collision.GetContact(0).normal);
    }*/
}