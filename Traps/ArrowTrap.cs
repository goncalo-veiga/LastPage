using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] arrows;

    private float cooldownTimer;

    private void Attack()
    {
        cooldownTimer = 0;

        // reset the position of the project
        arrows[FindArrows()].transform.position = firePoint.position;

        arrows[FindArrows()].GetComponent<EnemyProjectile>().ActivateProjectile(Mathf.Sign(transform.localScale.x));
        
    } 

    private int FindArrows()
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if(!arrows[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }


    private void Update()
    {

        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= attackCooldown)
            Attack();
    }

}
