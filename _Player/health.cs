using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Threading;
using System.Linq;
using Unity.VisualScripting;

public class health : MonoBehaviour, IDataPersistence
//public class health : MonoBehaviour
{   
    [Header ("Health")]
    [SerializeField] public float startingHealth;
    public float currentHealth { get; private set; }
    private bool dead, canTakeDamage;
    private Animator anim;

    [Header ("iFrames")]
    [SerializeField] private float iframe_time;
    [SerializeField] private int number_flashes;
    private SpriteRenderer spriteRend;

    private LayerMask enemiesLayer;

    private static int deaths;

     public void LoadData(GameData data)
    {
        // TODO: ONLY LOADS WHEN THE LEVEL IS COMPLETED (BOOLEAN SAYING: ISCOMPLETED)
    
        /*data.coinsCollected.TryGetValue(id, out collected);
        if (collected)
        {
            gameObject.SetActive(false);
        }*/
    
    }

    public void SaveData(ref GameData data)
    {
        data.deathCount = deaths;
    }

    Shield[] shields;
    private void Start()
    {
        enemiesLayer = (LayerMask)Variables.Object(transform.root.gameObject).Get("enemiesLayer");

        shields =GetComponentsInChildren<Shield>();
        canTakeDamage = true;
    }


    private void Awake()
    {
        dead=false;
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (dead && anim.GetCurrentAnimatorStateInfo(0).IsName("die") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=4) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            anim.SetBool("dead", false);
            anim.SetBool("unstopable", false);
            
        }
    }

    public void AddHealth(float health_value)
    {
        currentHealth = Math.Clamp(currentHealth + health_value, 0, startingHealth);
    }


    public void TakeDamage(float damage, GameObject enemy)
    {
        if (!canTakeDamage) return;

        foreach(Shield shield in shields)
        {
            if (shield.isBlocking && shield.isBlockable(enemy) ) return;
        }

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (enemy != gameObject)
        {
            if (currentHealth > 0)
            {
                StartCoroutine(Invunerability());
            }
            else
            {
                if (!dead)
                {
                    anim.SetTrigger("died");
                    anim.SetBool("dead", true);
                    anim.SetBool("unstopable", true);
                    GetComponent<PlayerMovement1>().enabled = false;
                    GetComponent<PlayerActions>().enabled = false;
                    GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    dead = true;
                    deaths++;
                }
            }
        }
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        anim.SetBool("dead", false);
        anim.SetBool("unstopable", false);
    }


    public IEnumerator Invunerability(int flashes = 0, float invTime = 0, Color? colour1= null, Color? colour2=null)
    {
        Color originalColour=spriteRend.color;
        if (flashes <= 0) flashes = number_flashes;

        if (invTime <= 0) invTime = iframe_time;

        canTakeDamage = false;

        for (int i = 0; i < flashes; i++)
        {

            spriteRend.color = colour1 ?? new Color(0, 0, 0, 0.5f);

            yield return new WaitForSeconds(invTime / (flashes*2));
            spriteRend.color = colour2 ?? Color.white;
            yield return new WaitForSeconds(invTime / (flashes*2));

        }

        spriteRend.color = originalColour;

        canTakeDamage = true;
    }

}
