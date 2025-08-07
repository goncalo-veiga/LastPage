using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;
    public AudioSource coins_source;
    public AudioClip coin_sound;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }



}
