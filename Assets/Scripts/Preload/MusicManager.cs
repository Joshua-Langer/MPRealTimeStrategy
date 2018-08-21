using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour {

    [Header("GameMusic")]
    [SerializeField]
    AudioClip levelMusic;

    [Header("Audio Source")]
    [SerializeField]
    AudioSource gameMusic;

    void LoadAudioSource()
    {
        gameMusic = GetComponent<AudioSource>();
    }

    public void GameAudio()
    {
        LoadAudioSource();
        gameMusic.clip = levelMusic;
        gameMusic.volume = .75f;
        gameMusic.loop = true;
        gameMusic.Play();
    }
}
