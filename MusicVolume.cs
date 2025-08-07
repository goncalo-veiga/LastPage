using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicVolume : MonoBehaviour
{

    AudioSource myAudioSource;

    float music_volume = 0.5f;

    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        myAudioSource.volume = music_volume;
    }

    public void Volume(float volume)
    {
        music_volume = volume;
    }
}
