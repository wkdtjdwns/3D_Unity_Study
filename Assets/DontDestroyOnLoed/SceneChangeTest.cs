using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SceneManager 클래스를 사용하기 위한 namespace using
using UnityEngine.SceneManagement;

public class SceneChangeTest : MonoBehaviour
{
    public void SceneChanege(int index)
    {
        // SceneManager.LoadScene("Scene 이름"); : 해당 이름을 가진 Scene을 불러옴
        SceneManager.LoadScene("DontDestroyOnLoedTestScene " + index); // index = 0이면 TestScene 0이라는 이름을 가진 Scene을 불러옴
    }
}
