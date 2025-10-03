using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour
{
    private void Awake()
    {
        // var : ������ �ڷ����� �ڵ����� �����ϴ� ����.
        // var : ���� ������ �����ؾ� �ϰ� ����� ���ÿ� �ʱ�ȭ�� ����� ��.
        // FindObjectsOfType<DontDestroyObject>()
        // -> DontDestroyObject��� ������Ʈ�� ������ �ִ� ��� ������Ʈ�� �˻��ؼ� ��ȯ ����.
        var obj = FindObjectsOfType<DontDestroyObject>();

        // �޾ƿ� ������Ʈ���� 1����� DontDestroyOnLoad�ϸ� ��.
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(this.gameObject);
        }

        // 2�� �̻��̶�� ������Ʈ�� Destroy(�ı�)��.
        else
        {
            Destroy(this.gameObject);
        }
    }
}
