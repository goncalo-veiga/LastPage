using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    [field: SerializeField, Header("Health")] public float startingHealth { get; private set; }
    public float currentHealth { get; private set; }
    private bool dead;
    private Animator anim;

    [Header("iFrames")]
    [SerializeField] private float iframe_time;
    [SerializeField] private int number_flashes;
    private SpriteRenderer spriteRend;
    [SerializeField] bool isboss;

    Vector2 startPos;

    public Slider slider;

    private void Start()
    {
        startPos = transform.position;
        if (isboss)
        {
            SetMaxHealth();
        }
    }


    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }


    public void TakeDamage(float damage, GameObject enemy)
    {

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
        if (isboss)
            {
                SetHealth();
            }

        if (currentHealth > 0)
        {
            //anim.SetTrigger("hurt");
            StartCoroutine(Invunerability());
        }
        else
        {
            if (!dead)
            {
                //anim.SetTrigger("die");
                spriteRend.color = new Color(1, 0, 0, 0.5f);
                dead = true;
                gameObject.SetActive(false);
                

            }
        }
    }

    private void SetMaxHealth()
    {
        slider.maxValue = startingHealth;
        slider.value = currentHealth;
    }

    public void SetHealth()
    {
        slider.value = currentHealth;
    }
    

    private IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < number_flashes; i++)
        {

            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iframe_time / (number_flashes) * 2);
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iframe_time / (number_flashes) * 2);

        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }

}

