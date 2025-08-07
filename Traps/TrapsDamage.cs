using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsDamage : MonoBehaviour
{
    [SerializeField] public float damage;

    protected void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "Player")
        {
            collision.GetComponent<health>().TakeDamage(damage, gameObject);
        }

    }


}
