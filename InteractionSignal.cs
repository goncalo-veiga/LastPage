using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSignal : MonoBehaviour {

    [SerializeField] private Text pickUpText;


	private void Start () 
    {
        pickUpText.gameObject.SetActive(false);
	}
	
	private void Update () 
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            pickUpText.gameObject.SetActive(true);
        }        
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            pickUpText.gameObject.SetActive(false);
        }
    }
}
