using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // 다른 스크립트들에서 SoundManager를 쉽게 사용할 수 있도록 해주는 변수
    // 이게 없으면 다른 스크립트에서 SoundManager를 사용할 때 변수를 추가하고 초기화까지 해야함.
    public static SoundManager instance;

    // [SerializeField] : public이 아닌 속성들을 Inspector 창에서 편집할 수 있게 해줌
    [SerializeField] private AudioClip[] audioClips;    // 효과음들을 넣을 공간
    [SerializeField] private AudioClip[] bgmClips;      // 배경음들을 넣을 공간

    // 오디오를 출력해주는 AudioSource 컴포넌트 변수들
    private AudioSource bgmPlayer;
    private AudioSource sfxPlayer;

    // 오디오의 볼륨을 조절할 슬라이더 변수들
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        instance = this; // 위에서 선언한 intance에 자신을 넣음 -> 다른 스크립트들이 자신을 사용할 수 있게 함.

        // 변수 초기화
        bgmPlayer = GameObject.Find("Bgm Player").GetComponent<AudioSource>();
        sfxPlayer = GameObject.Find("Sfx Player").GetComponent<AudioSource>();

        bgmSlider = bgmSlider.GetComponent<Slider>();
        sfxSlider = sfxSlider.GetComponent<Slider>();

        // Slider의 이벤트를 할당해줌. (슬라이더 값이 변경될 때 실행할 함수)
        bgmSlider.onValueChanged.AddListener(ChangeBgmSound);
        sfxSlider.onValueChanged.AddListener(ChangeSfxSound);
    }

    // 효과음을 출력하는 함수
    public void PlaySound(string type) // 어떤 소리를 낼지 받아옴
    {
        int index = 0; // 어떤 소리를 낼지 결정하는 int 변수

        switch (type) // 받아온 값에 따라서 index의 값 변경
        {
            // type을 Get Gold, Die와 같이 직관적이게 설정하면 좋음.
            case "Sound 1": index = 0; break;
            case "Sound 2": index = 1; break;
            case "Sound 3": index = 2; break;
        }

        sfxPlayer.clip = audioClips[index]; // 어떤 소리를 낼지 index의 값을 따라서 결정하고
        sfxPlayer.PlayOneShot(sfxPlayer.clip); // sfxPlayer의 clip에 들어 있는 모든 소리를 출력함

        // Play() : 여러 사운드가 들어와도 마지막에 들어온 사운드 1개만 출력함. (사운드를 출력하다 끊길 수 있음.)
        // PlayOneShot() : 여러 사운드가 들어오면 다 출력함. (사운드를 출력하다 끊기지 않음.)
    }

    // 배경음을 출력하는 함수
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

    // 볼륨을 바꾸는 함수들 (위에서 초기화한 슬라이더의 이벤트에 들어가는 함수들임.)
    private void ChangeBgmSound(float value)
    {
        bgmPlayer.volume = value;
    }

    private void ChangeSfxSound(float value)
    {
        sfxPlayer.volume = value;
    }
}