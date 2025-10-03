using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float speed = 5;                // 발사체의 이동 속도
    private Vector3 direction;      // 발사체의 이동 방향
    private float halfSize;         // 발사체의 x축 절반 크기

    public void Setup(Vector3 direction)
    {
        this.direction = direction;
        halfSize = this.transform.localScale.x * 0.5f;
    }

    private void Update()
    {
        this.transform.position += direction * speed * Time.deltaTime;

        // this.transform.position 위치에서 direction 방향으로 halfSize 길이의 광선을 발사함.
        if ( Physics.Raycast(this.transform.position, direction, out RaycastHit hit, halfSize) )
        {
            // 입사 벡터 direction과 표면 벡터 hit.normal 정보를 바탕으로 반사 벡터 direction을 구함.
            direction = Vector3.Reflect(direction, hit.normal);
        }
    }

    /*// 어떤 오브젝트든 닿았을 때
    private void OnCollisionEnter(Collision collision)
    {
        // 입사 벡터 direction과 표면 벡터 collision.GetContact(0).normal 정보를 바탕으로 반사 벡터 direction을 구함.
        direction = Vector3.Reflect(direction, collision.GetContact(0).normal);
    }*/
}