using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireWeapon : MonoBehaviour
{
    private LayerMask enemiesLayer;
    // Start is called before the first frame update
    void Start()
    {
        enemiesLayer = (LayerMask)Variables.Object(transform.root.gameObject).Get("enemiesLayer");
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // bool isEnemy = (enemiesLayer.value & (1 << collision.gameObject.layer)) > 0;

        if (collision.TryGetComponent<Burnables>(out Burnables burnObj)) burnObj.Burn();

        /*
        else if (isEnemy && collision.gameObject.GetComponent<health>())
        {
            //collision.gameObject.AddComponent<OnFire>();
            ;
        }
        */
    }
}
