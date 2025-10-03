using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject cubePrefab;              // 생성할 큐브 프리팹 정보를 저장하는 변수

    [SerializeField]
    private Vector3 sizeDelta = Vector3.one;    // 월드 내부에 생성되도록 폭 조절을 위한 변수

    [SerializeField]
    private float spawnTime = 5f;               // 큐브가 생성되는 주기

    [SerializeField]
    private int maxCubeCount = 10;              // 큐브의 최대 생성 개수
    private int currentCubeCount = 0;           // 현재 생성된 큐브 개수

    private IEnumerator Start()
    {
        // Ground 오브젝트의 크기(transform.localScale)을 저장함.
        Vector3 fieldSize = this.transform.localScale;

        while (currentCubeCount < maxCubeCount)
        {
            // 생성하는 큐브의 x, z 위치는 맵 내부 임의의 위치로 설정함.
            float x = Random.Range(-fieldSize.x * 0.5f + sizeDelta.x, fieldSize.x * 0.5f - sizeDelta.x);
            float z = Random.Range(-fieldSize.z * 0.5f + sizeDelta.z, fieldSize.z * 0.5f - sizeDelta.z);

            // y 위치는 0.5로 고정한 뒤에 랜덤한 위치에 큐브를 생성함.
            Instantiate(cubePrefab, new Vector3(x, 0.5f, z), Quaternion.identity);
            currentCubeCount++;

            yield return new WaitForSeconds(spawnTime);
        }
    }
}