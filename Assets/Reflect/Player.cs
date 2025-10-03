using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;    // ���콺 ���� ��ư�� ������ �� ������ �߻�ü ������

    [SerializeField]
    private Transform spawnPoint;           // �߻�ü�� �����ϴ� ��ġ

    private void Update()
    {
        // ī�޶� ��ġ���� ȭ���� ���콺 ��ġ�� �������� ����(ray) ����
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // (0, 1, 0) ��ġ�� ǥ�� ���Ͱ� (0, 1, 0)�� ������ ����� ������.
        Plane plane = new Plane(Vector3.up, Vector3.up);

        // ������ ���� �����ϴ� �� Ȯ���ϰ�, ���� ���������� �Ÿ��� �����.
        if ( plane.Raycast(ray, out float distance) )
        {
            // ray.GetPoint(distance)�� ���� �����ϴ� ������ ��ġ�� ����.
            Vector3 direction = ray.GetPoint(distance) - this.transform.position;

            // �÷��̾ (direction.x, 0, direction.z) ������ �ٶ󺸵��� ȸ����.
            this.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }

        // ���콺 ���� ��ư�� ������ ��
        if ( Input.GetMouseButtonDown(0) )
        {
            // spawnPoint.position ��ġ�� �߻�ü�� ������.
            // Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);

            // ������ �߻�ü�� clone�� �����ϰ�
            GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);

            // clone.Projectile.Setup() �޼ҵ带 ȣ����. (�Ű������� �÷��̾��� ���� ������ ������)
            clone.GetComponent<Projectile>().Setup(this.transform.forward);
        }
    }
}