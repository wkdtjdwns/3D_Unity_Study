using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject cubePrefab;              // ������ ť�� ������ ������ �����ϴ� ����

    [SerializeField]
    private Vector3 sizeDelta = Vector3.one;    // ���� ���ο� �����ǵ��� �� ������ ���� ����

    [SerializeField]
    private float spawnTime = 5f;               // ť�갡 �����Ǵ� �ֱ�

    [SerializeField]
    private int maxCubeCount = 10;              // ť���� �ִ� ���� ����
    private int currentCubeCount = 0;           // ���� ������ ť�� ����

    private IEnumerator Start()
    {
        // Ground ������Ʈ�� ũ��(transform.localScale)�� ������.
        Vector3 fieldSize = this.transform.localScale;

        while (currentCubeCount < maxCubeCount)
        {
            // �����ϴ� ť���� x, z ��ġ�� �� ���� ������ ��ġ�� ������.
            float x = Random.Range(-fieldSize.x * 0.5f + sizeDelta.x, fieldSize.x * 0.5f - sizeDelta.x);
            float z = Random.Range(-fieldSize.z * 0.5f + sizeDelta.z, fieldSize.z * 0.5f - sizeDelta.z);

            // y ��ġ�� 0.5�� ������ �ڿ� ������ ��ġ�� ť�긦 ������.
            Instantiate(cubePrefab, new Vector3(x, 0.5f, z), Quaternion.identity);
            currentCubeCount++;

            yield return new WaitForSeconds(spawnTime);
        }
    }
}