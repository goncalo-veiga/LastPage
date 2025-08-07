using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour, IDataPersistence
//public class ItemCollector : MonoBehaviour
{
    public static int collector_ctr;
    public static int potion_ctr;

    [SerializeField] public Text coins_text, potion_text;

    
    public void LoadData(GameData data)
    {
        /*collector_ctr = data.coinsCount;
        potion_ctr = data.potionsCount;
        coins_text.text =  "" +collector_ctr;
        potion_text.text =  "" +potion_ctr;*/
    }

    public void SaveData(ref GameData data)
    {
        data.coinsCount = collector_ctr;
        data.potionsCount = potion_ctr;
    }
    
    private void Start()
    {
        coins_text.text =  "" +collector_ctr;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

       // health playerHealth = GetComponent<health>();
    

        if(collision.gameObject.CompareTag("Potion"))
        {
            SoundManager.instance.coins_source.volume = 0.25f; 
            SoundManager.instance.coins_source.PlayOneShot(SoundManager.instance.coin_sound);
            Destroy(collision.gameObject);
            potion_ctr++;
            potion_text.text =  "" +potion_ctr;
            
        }

        if(collision.gameObject.CompareTag("Coin"))
        {
            SoundManager.instance.coins_source.volume = 0.25f; 
            SoundManager.instance.coins_source.PlayOneShot(SoundManager.instance.coin_sound);
            Destroy(collision.gameObject);
            collector_ctr++;
            coins_text.text =  "" +collector_ctr;
            
        }

        if(collision.gameObject.CompareTag("Super Coin"))
        {
            SuperCoinSound.instance.supersound_source.volume = 0.8f; 
            SuperCoinSound.instance.supersound_source.PlayOneShot(SuperCoinSound.instance.supersound_sound);
            Destroy(collision.gameObject);
            collector_ctr = collector_ctr +5 ;
            coins_text.text =  "" +collector_ctr;
            
        }

        if(collision.gameObject.CompareTag("Costa_Coin"))
        {
            SuperCoinSound.instance.supersound_source.volume = 1f; 
            SuperCoinSound.instance.supersound_source.PlayOneShot(SuperCoinSound.instance.supersound_sound);
            Destroy(collision.gameObject);
            collector_ctr = collector_ctr +50;
            coins_text.text =  "" + collector_ctr;
            
        }

        if(collision.gameObject.CompareTag("ArrowTrap"))
        {

            //health playerHealth = GetComponent<health>();
            //if (playerHealth.currentHealth > 0)
            //{
            ArrowSound.instance.arrowsound_source.volume = 0.1f; 
            ArrowSound.instance.arrowsound_source.PlayOneShot(ArrowSound.instance.arrowsound_sound);
            //}
            
        }

        



    }
}
