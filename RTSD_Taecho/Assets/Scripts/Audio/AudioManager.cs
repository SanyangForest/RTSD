using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip; // bgm
    public float bgmVolume;  // bgm의 볼륨
    AudioSource bgmPlayer; // bgm을 재생할 오디오 소스

    [Header("#SFX")]
    public AudioClip[] sfxClips;  // 효과음의 클립들 (효과음은 많으니 배열로)
    public float sfxVolume;  // 효과음 볼륨
    public int channels;  // 여러 효과음을 동시에 낼 수 있게 해주는 채널 개수의 변수
    AudioSource[] sfxPlayers;  // 효과음을 재생할 오디오 소스들 (효과음은 많으니 배열로)
    int channelIndex; // 현재 재생하고 있는 채널의 인덱스

    public enum Sfx  // 여러가지 효과음들 지정 - 현재는 임시 효과음이므로 변경할 필요 있음
    {
        Dead,
        Hit,
        LevelUp = 3,
        Lose,
        Melle,
        Range = 7,
        Select,
        Win
    }


    private void Awake()
    {
        instance = this;
        Init();
    }

    private void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;  // 배경음을 담당히는 자식 오브젝트 생성
        bgmPlayer = bgmObject.AddComponent<AudioSource>(); // AddComponent 함수로 오디오소스를 생성하고 변수에 저장
        bgmPlayer.playOnAwake = false; // 시작과 동시에 bgm이 플레이되진 않게 만들어 놨다.
        bgmPlayer.loop = true;  // bgm 반복
        bgmPlayer.volume = bgmVolume; // bgm의 볼륨은 bgmVolume 변수를 따른다
        bgmPlayer.clip = bgmClip; // 재생할 bgm은 bgmClip에 들어있는 것을 재생한다.

        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform; // 배경음과 같다.
        sfxPlayers = new AudioSource[channels]; // 오디오소스를 channels의 개수만큼 만들 수 있게 생성

        for(int index = 0; index < sfxPlayers.Length; index++)  // 반복문을 열고 모든 효과음 오디오 소스를 생성하면서 저장한다. - 배열이기 때문에
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for(int index = 0; index < sfxPlayers.Length;index++)
        {
           int loopIndex = (index + channelIndex) % sfxPlayers.Length; // 효과음의 인덱스가 끝나면 처음으로 되돌리기 위한 코드. 길이로 나눈 나머지를 사용하는 이유는 인덱스 초과를 막기 위해서

            if (sfxPlayers[loopIndex].isPlaying)  // 효과음이 중간에 끊기는 것을 방지하기 위해 효과음이 실행중이면 for문을 넘긴다
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break; 
        }
    }

    // 이제 사용할 곳에서 AudioManager.instance.PlaySfx(AudioManager.Sfx.Select); 와 같은 식으로 사용하면 된다.
}
