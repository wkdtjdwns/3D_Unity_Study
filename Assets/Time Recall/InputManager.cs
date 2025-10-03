using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ##################################################################################################
 * 1. 스크립트명: InputManager.cs
 * 2. 역할: 시간 역행 테스트에 필요한 폭발(오브젝트 이동) 및 시간 역행 실행 담당
 * ################################################################################################## */

public class InputManager : MonoBehaviour
{
    public float explosionForce = 10f;      // 폭발 힘
    public float upwardForce = 1f;          // 위쪽으로 가해지는 힘
    public KeyCode recallKey = KeyCode.E;   // 시간 역행 시작 키

    /* ##################################################################################################
     * 폭발 효과 및 시간 역행 실행
     * ################################################################################################## */
    private void Update()
    {
        // 마우스 왼쪽 버튼 클릭 시 폭발 실행
        if (Input.GetMouseButtonDown(0))
        {
            ApplyExplosion();
            print("Boom");
        }

        // 'E' 키 입력 시 시간 역행 실행
        if (Input.GetKeyDown(recallKey))
        {
            RecallManager.RecallAll();
            print("Recall");
        }
    }

    /* ##################################################################################################
     * 폭발 효과 적용 함수
     * ################################################################################################## */
    private void ApplyExplosion()
    {
        // 폭발 위치 설정
        Vector3 explosionCenter = new Vector3(0, 0.55f, 0);

        foreach (Recaller recaller in RecallManager.RecallerList)
        {
            // 모든 Recaller의 Rigidbody를 가져옴
            Rigidbody rigidbody = recaller.GetComponent<Rigidbody>();
            
            // Rigidbody가 존재하고 현재 시간 역행 중이 아니며 자기 자신이 아닌 경우 폭발
            if (rigidbody != null && !recaller.IsRecalling && rigidbody.gameObject != gameObject)
            {
                rigidbody.AddExplosionForce(explosionForce, explosionCenter, 1000f, upwardForce, ForceMode.Impulse);
            }
        }
    }
}
