using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : ButtonActivatedObjects
{
    private SpriteRenderer sprt;
    private Collider2D col;
    private bool active;
    void Start()
    {
        sprt= GetComponent<SpriteRenderer>();
        col= GetComponent<Collider2D>();
        active = true;
    }

    public override void Change()
    {
        active = !active;
        sprt.enabled = active;
        col.enabled = active;
    }
}
