using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : TrapsDamage // damage the player everytime they touch
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;

    public void ActivateProjectile(float direction)
    {
        lifetime = 0;
        gameObject.SetActive(true);

    }

    private void Update()
    {
        float movementSpeed = speed* Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if(lifetime > resetTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.SetActive(false);
            collision.GetComponent<health>().TakeDamage(damage, gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            gameObject.SetActive(false);
        }
    }

}
