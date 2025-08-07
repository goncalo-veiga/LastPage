using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public string[] dialogue;
    private int index;

    public float wordSpeed;
    public bool playerIsClose;

    //public GameObject contButton;

    private Coroutine typingCoroutine;

    public Text interactionText;

    public GameObject myNPC;

    private GameObject player;
    private PlayerActions playeractions;

    public GameObject yesno_button;


    private void Start()
    {

        player = GameObject.Find("Player");

        playeractions = player.GetComponent<PlayerActions>();
    }

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q) && playerIsClose)
        {


            if (dialoguePanel.activeInHierarchy)
            {
                zeroText();
                playeractions.enabled = true;
                yesno_button.SetActive(false);
                
            }
            else
             {   
                if (myNPC != null && myNPC.CompareTag("NPC_Purchase"))
                {
                    yesno_button.SetActive(true);
                }

                playeractions.enabled = false;
                dialoguePanel.SetActive(true);
                if (typingCoroutine != null)
                    StopCoroutine(typingCoroutine);
                typingCoroutine = StartCoroutine(Typing());
                InteractionSignalOFF();
          
               
            }
        }



        /*if (dialogueText.text == dialogue[index])
        {
            contButton.SetActive(true);
        }*/
    }

    public void zeroText()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    /*public void NextLine()
    {
        //contButton.SetActive(false);

        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(Typing());
        }
        else
        {
            zeroText();
        }
    }*/

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsClose = true;
            InteractionSignalON();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
            InteractionSignalOFF();
            playeractions.enabled = true;
            yesno_button.SetActive(false);

        }
    }

        private void InteractionSignalON()
    {
        interactionText.gameObject.SetActive(true);
        string tag = gameObject.tag;
        interactionText.text = "'Q'";
    }

    private void InteractionSignalOFF()
    {
        interactionText.gameObject.SetActive(false);
    }
}
