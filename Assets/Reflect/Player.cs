using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;    // 마우스 왼쪽 버튼을 눌렀을 때 생성할 발사체 프리팹

    [SerializeField]
    private Transform spawnPoint;           // 발사체를 생성하는 위치

    private void Update()
    {
        // 카메라 위치에서 화면의 마우스 위치를 지나가는 광선(ray) 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // (0, 1, 0) 위치에 표면 벡터가 (0, 1, 0)인 가상의 평면을 생성함.
        Plane plane = new Plane(Vector3.up, Vector3.up);

        // 광선이 평면과 교차하는 지 확인하고, 교차 지점까지의 거리를 계산함.
        if ( plane.Raycast(ray, out float distance) )
        {
            // ray.GetPoint(distance)로 평면과 교차하는 지점의 위치를 구함.
            Vector3 direction = ray.GetPoint(distance) - this.transform.position;

            // 플레이어가 (direction.x, 0, direction.z) 방향을 바라보도록 회전함.
            this.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }

        // 마우스 왼쪽 버튼을 눌렀을 때
        if ( Input.GetMouseButtonDown(0) )
        {
            // spawnPoint.position 위치에 발사체를 생성함.
            // Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);

            // 생성한 발사체를 clone에 저장하고
            GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);

            // clone.Projectile.Setup() 메소드를 호출함. (매개변수는 플레이어의 전방 방향을 전달함)
            clone.GetComponent<Projectile>().Setup(this.transform.forward);
        }
    }
}