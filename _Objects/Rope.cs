using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : Burnables
{
    // Start is called before the first frame update

    protected override void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent<Movables>(out Movables movable)) movable.enabled = false;

            if (transform.GetChild(i).TryGetComponent<Rigidbody2D>(out Rigidbody2D rb)) rb.bodyType = RigidbodyType2D.Static;
        }
    }

    public override void Burn()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);

            child.parent=null;

            if (child.TryGetComponent<Movables>(out Movables movable)) movable.enabled = true;

            if (child.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb)) rb.bodyType = RigidbodyType2D.Dynamic;
        }

        Destroy(gameObject);
    }
}
