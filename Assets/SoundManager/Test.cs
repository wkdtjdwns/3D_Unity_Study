using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
/*  instance�� ������� �ʾ��� ���

    private GameObject soundManagerObj;
    private SoundManager soundManager;

    private void Awake()
    {
        // Find �Լ��� �޸𸮸� ���� ��ƸԾ Unity�� ��õ���� �ʱ� ��.
        soundManagerObj = GameObject.Find("SoundManager").gameObject;   // SoundManager��� �̸��� ���� ������Ʈ�� ã�ƿ�.
        soundManager = soundManagerObj.GetComponent<SoundManager>();    // ã�ƿ� SoundManager ������Ʈ���� ������Ʈ�� ������.
    }

    public void SoundPlay()
    {
        soundManager.PlaySound("Sound 1");
    }
*/

    public void SoundPlay() // Button�� ����� ���̱� ������ public ��� (�ƴϸ� �Ҵ� ����)
    {
        // SoundManager���� ������ instance�� ���
        SoundManager.instance.PlaySound("Sound 1");
    }

    public void SoundPlay(string type) // �޼ҵ� �����ε�
    {
        // Ÿ���� �޾ƿͼ� �� Ÿ�Կ� ���� ���� ���
        SoundManager.instance.PlaySound(type);
    }

    public void BgmChange(string type)
    {
        SoundManager.instance.PlayBgm(type);
    }
}
