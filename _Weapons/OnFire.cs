using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFire : MonoBehaviour
{
    private health Health;
    private float damageTimer, finalTimer,currTime;
    private int i;
    // Start is called before the first frame update
    void Start()
    {
        Health = GetComponent<health>();
        damageTimer = 3;
        finalTimer = 15;
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (currTime > finalTimer) Destroy(this);
        else if (currTime > damageTimer * i)
        {
            i++;
            Health.TakeDamage(1, transform.gameObject);
        }
        currTime += Time.deltaTime;
    }
}
