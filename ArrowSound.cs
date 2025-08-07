using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSound : MonoBehaviour
{

    public static ArrowSound instance;
    public AudioSource arrowsound_source;
    public AudioClip arrowsound_sound;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }


}

