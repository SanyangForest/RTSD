using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip; // bgm
    public float bgmVolume;  // bgm�� ����
    AudioSource bgmPlayer; // bgm�� ����� ����� �ҽ�

    [Header("#SFX")]
    public AudioClip[] sfxClips;  // ȿ������ Ŭ���� (ȿ������ ������ �迭��)
    public float sfxVolume;  // ȿ���� ����
    public int channels;  // ���� ȿ������ ���ÿ� �� �� �ְ� ���ִ� ä�� ������ ����
    AudioSource[] sfxPlayers;  // ȿ������ ����� ����� �ҽ��� (ȿ������ ������ �迭��)
    int channelIndex; // ���� ����ϰ� �ִ� ä���� �ε���

    public enum Sfx  // �������� ȿ������ ���� - ����� �ӽ� ȿ�����̹Ƿ� ������ �ʿ� ����
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
        // ����� �÷��̾� �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;  // ������� ������� �ڽ� ������Ʈ ����
        bgmPlayer = bgmObject.AddComponent<AudioSource>(); // AddComponent �Լ��� ������ҽ��� �����ϰ� ������ ����
        bgmPlayer.playOnAwake = false; // ���۰� ���ÿ� bgm�� �÷��̵��� �ʰ� ����� ����.
        bgmPlayer.loop = true;  // bgm �ݺ�
        bgmPlayer.volume = bgmVolume; // bgm�� ������ bgmVolume ������ ������
        bgmPlayer.clip = bgmClip; // ����� bgm�� bgmClip�� ����ִ� ���� ����Ѵ�.

        // ȿ���� �÷��̾� �ʱ�ȭ
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform; // ������� ����.
        sfxPlayers = new AudioSource[channels]; // ������ҽ��� channels�� ������ŭ ���� �� �ְ� ����

        for(int index = 0; index < sfxPlayers.Length; index++)  // �ݺ����� ���� ��� ȿ���� ����� �ҽ��� �����ϸ鼭 �����Ѵ�. - �迭�̱� ������
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
           int loopIndex = (index + channelIndex) % sfxPlayers.Length; // ȿ������ �ε����� ������ ó������ �ǵ����� ���� �ڵ�. ���̷� ���� �������� ����ϴ� ������ �ε��� �ʰ��� ���� ���ؼ�

            if (sfxPlayers[loopIndex].isPlaying)  // ȿ������ �߰��� ����� ���� �����ϱ� ���� ȿ������ �������̸� for���� �ѱ��
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break; 
        }
    }

    // ���� ����� ������ AudioManager.instance.PlaySfx(AudioManager.Sfx.Select); �� ���� ������ ����ϸ� �ȴ�.
}
