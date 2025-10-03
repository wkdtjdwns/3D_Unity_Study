using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
/*  instance를 사용하지 않았을 경우

    private GameObject soundManagerObj;
    private SoundManager soundManager;

    private void Awake()
    {
        // Find 함수는 메모리를 많이 잡아먹어서 Unity도 추천하지 않긴 함.
        soundManagerObj = GameObject.Find("SoundManager").gameObject;   // SoundManager라는 이름을 가진 오브젝트를 찾아옴.
        soundManager = soundManagerObj.GetComponent<SoundManager>();    // 찾아온 SoundManager 오브젝트에서 컴포넌트를 가져옴.
    }

    public void SoundPlay()
    {
        soundManager.PlaySound("Sound 1");
    }
*/

    public void SoundPlay() // Button에 사용할 것이기 때문에 public 사용 (아니면 할당 못함)
    {
        // SoundManager에서 선언한 instance의 사용
        SoundManager.instance.PlaySound("Sound 1");
    }

    public void SoundPlay(string type) // 메소드 오버로딩
    {
        // 타입을 받아와서 그 타입에 따른 사운드 출력
        SoundManager.instance.PlaySound(type);
    }

    public void BgmChange(string type)
    {
        SoundManager.instance.PlayBgm(type);
    }
}
