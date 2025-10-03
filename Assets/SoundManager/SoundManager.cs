using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // �ٸ� ��ũ��Ʈ�鿡�� SoundManager�� ���� ����� �� �ֵ��� ���ִ� ����
    // �̰� ������ �ٸ� ��ũ��Ʈ���� SoundManager�� ����� �� ������ �߰��ϰ� �ʱ�ȭ���� �ؾ���.
    public static SoundManager instance;

    // [SerializeField] : public�� �ƴ� �Ӽ����� Inspector â���� ������ �� �ְ� ����
    [SerializeField] private AudioClip[] audioClips;    // ȿ�������� ���� ����
    [SerializeField] private AudioClip[] bgmClips;      // ��������� ���� ����

    // ������� ������ִ� AudioSource ������Ʈ ������
    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;

    // ������� ������ ������ �����̴� ������
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        instance = this; // ������ ������ intance�� �ڽ��� ���� -> �ٸ� ��ũ��Ʈ���� �ڽ��� ����� �� �ְ� ��.

        // ���� �ʱ�ȭ
        bgmPlayer = GameObject.Find("Bgm Player").GetComponent<AudioSource>();
        sfxPlayer = GameObject.Find("Sfx Player").GetComponent<AudioSource>();

        bgmSlider = bgmSlider.GetComponent<Slider>();
        sfxSlider = sfxSlider.GetComponent<Slider>();

        // Slider�� �̺�Ʈ�� �Ҵ�����. (�����̴� ���� ����� �� ������ �Լ�)
        bgmSlider.onValueChanged.AddListener(ChangeBgmSound);
        sfxSlider.onValueChanged.AddListener(ChangeSfxSound);
    }

    // ȿ������ ����ϴ� �Լ�
    public void PlaySound(string type) // � �Ҹ��� ���� �޾ƿ�
    {
        int index = 0; // � �Ҹ��� ���� �����ϴ� int ����

        switch (type) // �޾ƿ� ���� ���� index�� �� ����
        {
            // type�� Get Gold, Die�� ���� �������̰� �����ϸ� ����.
            case "Sound 1": index = 0; break;
            case "Sound 2": index = 1; break;
            case "Sound 3": index = 2; break;
        }

        sfxPlayer.clip = audioClips[index]; // � �Ҹ��� ���� index�� ���� ���� �����ϰ�
        sfxPlayer.PlayOneShot(sfxPlayer.clip); // sfxPlayer�� clip�� ��� �ִ� ��� �Ҹ��� �����

        // Play() : ���� ���尡 ���͵� �������� ���� ���� 1���� �����. (���带 ����ϴ� ���� �� ����.)
        // PlayOneShot() : ���� ���尡 ������ �� �����. (���带 ����ϴ� ������ ����.)
    }

    // ������� ����ϴ� �Լ�
    public void PlayBgm(string type)
    {
        int index = 0;

        switch (type)
        {
            case "Bgm 1": index = 0; break;
            case "Bgm 2": index = 1; break;
            case "Bgm 3": index = 2; break;
        }

        bgmPlayer.clip = bgmClips[index];
        bgmPlayer.Play();
    }

    // ������ �ٲٴ� �Լ��� (������ �ʱ�ȭ�� �����̴��� �̺�Ʈ�� ���� �Լ�����.)
    private void ChangeBgmSound(float value)
    {
        bgmPlayer.volume = value;
    }

    private void ChangeSfxSound(float value)
    {
        sfxPlayer.volume = value;
    }
}