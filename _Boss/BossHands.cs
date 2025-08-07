using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHands : MonoBehaviour
{
    [HideInInspector] public Transform Player;
    [HideInInspector] public Collider2D col;
    [HideInInspector] public float damageMultiplier;
    // Start is called before the first frame update
    void Awake()
    {
       col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform == Player) Player.GetComponent<health>().TakeDamage(damageMultiplier, gameObject);
    }
}
