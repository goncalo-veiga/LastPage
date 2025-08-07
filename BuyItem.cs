using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyItem : MonoBehaviour
{
    public int item_price = 12; 
    public int weapon_damage = 3;
    [SerializeField] Text coins_text;
    [SerializeField] GameObject npc_purchase_menu;
    [SerializeField] ItemCollector itemCollector;
    [SerializeField] PlayerActions playeractions;
    [SerializeField] PlayerMovement1 movement;
    [SerializeField] GameObject yesno_button;
    [SerializeField] GameObject warning_money;
    [SerializeField] GameObject dialogue;

    public NPC script_npc;
    [SerializeField] MeleeWeapon melee_w;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            npc_purchase_menu.SetActive(true);
            playeractions.enabled = false;

        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            npc_purchase_menu.SetActive(false);
   
        }
    }

    public void Yes()
    {

        script_npc.zeroText();
        //dialogue.SetActive(false);
        npc_purchase_menu.SetActive(true);
        yesno_button.SetActive(false);
        script_npc.enabled = false;
        movement.enabled = false;
    }

    public void No()
    {
        script_npc.zeroText();
        //dialogue.SetActive(false);
        yesno_button.SetActive(false);

    }

    public void exitbuy()
    {
        npc_purchase_menu.SetActive(false);
        script_npc.enabled = true;
        movement.enabled = true;
    }

    public void Buy()
    {

         if (ItemCollector.collector_ctr >= item_price)
            {
                ItemCollector.collector_ctr -= item_price;
                itemCollector.coins_text.text = "" + ItemCollector.collector_ctr;
                npc_purchase_menu.SetActive(false);

            //MUDA A SPRITE DA ESPADA, E MUDA AINDA O DAMAGE
                melee_w.damageMultiplier += weapon_damage;
                Debug.Log(melee_w.damageMultiplier);

            }

        else
        {
            warning_money.SetActive(true);
            npc_purchase_menu.SetActive(false);
            
        }
        script_npc.enabled = true;
        movement.enabled = true;
        
    }
}
