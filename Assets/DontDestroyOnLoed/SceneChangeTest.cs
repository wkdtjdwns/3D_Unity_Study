using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SceneManager Ŭ������ ����ϱ� ���� namespace using
using UnityEngine.SceneManagement;

public class SceneChangeTest : MonoBehaviour
{
    public void SceneChanege(int index)
    {
        // SceneManager.LoadScene("Scene �̸�"); : �ش� �̸��� ���� Scene�� �ҷ���
        SceneManager.LoadScene("DontDestroyOnLoedTestScene " + index); // index = 0�̸� TestScene 0�̶�� �̸��� ���� Scene�� �ҷ���
    }
}
