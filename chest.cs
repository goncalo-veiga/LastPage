using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chest : MonoBehaviour
{
    [SerializeField] private bool isLooted = false;
    [SerializeField] private Sprite chestClose;
    [SerializeField] private Sprite chestOpen;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public bool playerIsClose;
    [SerializeField] private List<string> items = new List<string>();
    public Text interactionText;
    private bool hasInteracted = false;

    public ItemCollector itemCollector;

    private void Start()
    {
        spriteRenderer.sprite = chestClose;
        InteractionSignalOFF();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsClose = true;
            if (!hasInteracted)
            {
                InteractionSignalON();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsClose = false;
            InteractionSignalOFF();
        }
    }

    private void Update()
    {
        if (!isLooted && Input.GetKeyDown(KeyCode.Q) && playerIsClose)
        {
            OpenChest();
            playerIsClose = false;
            hasInteracted = true;
           
        }

        if (!isLooted  && !playerIsClose)
        {
            spriteRenderer.sprite = chestClose;
        }

    }

    private void OpenChest()
    {
        isLooted = true;
        spriteRenderer.sprite = chestOpen;
        InteractionSignalOFF();
        foreach (string item in items)
        {
            Debug.Log("Found item: " + item);

             if(item == "Coins")
                {
                    SoundManager.instance.coins_source.volume = 0.25f; 
                    SoundManager.instance.coins_source.PlayOneShot(SoundManager.instance.coin_sound);
                    ItemCollector.collector_ctr += 10;
                    itemCollector.coins_text.text = "" + ItemCollector.collector_ctr;
                    //itemCollector.collector_ctr = itemCollector.collector_ctr + 10; 
                    //itemCollector.coins_text.text = "" + itemCollector.collector_ctr;
                }
        }
        items.Clear();
    }

    private void InteractionSignalON()
    {
        interactionText.gameObject.SetActive(true);
        interactionText.text = "'Q'";
        //interactionText.transform.position = spriteRenderer.transform.position + Vector3.up * 1.5f;
    }

    private void InteractionSignalOFF()
    {
        interactionText.gameObject.SetActive(false);
    }
}
