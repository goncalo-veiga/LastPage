using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;

public class Breakables : MonoBehaviour
{

    private ParticleSystem particle;
    private Animator animator;

    [SerializeField] private GameObject[] drops;
    // Start is called before the first frame update
    private void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        if (!this.gameObject.scene.isLoaded) return;
        foreach (GameObject drop in drops)
        {
            GameObject collect = Instantiate(drop);
            collect.transform.position = transform.position;
        }
    }
    // Update is called once per frame
    public void Breaking()
    {
        animator.SetTrigger("breaking");
        particle.Play();
    }

    public void Terminate()
    {
        Destroy(gameObject);
    }

}
