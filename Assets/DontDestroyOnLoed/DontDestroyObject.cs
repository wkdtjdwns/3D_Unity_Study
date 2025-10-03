using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{
    private void Awake()
    {
        // var : 변수의 자료형을 자동으로 저장하는 것임.
        // var : 지역 변수로 선언해야 하고 선언과 동시에 초기화를 해줘야 함.
        // FindObjectsOfType<DontDestroyObject>()
        // -> DontDestroyObject라는 컴포넌트를 가지고 있는 모든 오브젝트를 검색해서 반환 해줌.
        var obj = FindObjectsOfType<DontDestroyObject>();

        // 받아온 오브젝트들이 1개라면 DontDestroyOnLoad하면 됨.
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(this.gameObject);
        }

        // 2개 이상이라면 오브젝트를 Destroy(파괴)함.
        else
        {
            Destroy(this.gameObject);
        }
    }
}
