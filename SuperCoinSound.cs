using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperCoinSound : MonoBehaviour
{

    public static SuperCoinSound instance;
    public AudioSource supersound_source;
    public AudioClip supersound_sound;

    
    private void Awake()
    {
        instance = this;
    }


}

