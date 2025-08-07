using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : ButtonObjects
{
    private bool buttonPressed = false;
    [SerializeField] bool playerToggle, projectileToggle;
    protected override void Update()
    {
        base.Update();

        if (buttonPressed && Input.GetKeyUp(KeyCode.F)) buttonPressed = false;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!onChange && collision.gameObject.layer == LayerMask.NameToLayer("Player") && collision.gameObject.tag == "Projectile" && projectileToggle) OnActivation();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!onChange && collision.gameObject.layer==LayerMask.NameToLayer("Player") && playerToggle)
        {
            if (collision.gameObject.tag == "Player" && Input.GetKey(KeyCode.F) && !buttonPressed) 
            { 
                OnActivation();
                buttonPressed = true;
            }
        }
    }
}
