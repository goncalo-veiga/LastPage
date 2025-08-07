using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakePotion : MonoBehaviour
{

    public int potion_health;
    [SerializeField] public Text potion_text;
    

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
            {
                if(ItemCollector.potion_ctr != 0)
                {
                    if(GetComponent<health>().currentHealth != GetComponent<health>().startingHealth)
                    {
                        GetComponent<health>().AddHealth(potion_health);
                        ItemCollector.potion_ctr --;
                        potion_text.text =  "" + ItemCollector.potion_ctr;
                    }
                }
            }
    }
}
