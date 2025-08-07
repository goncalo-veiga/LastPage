using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesUI : MonoBehaviour
{
    public Image[] Abilities;

    public Image attack_1;
    public Image attack_2;
    public Image csa;
    public Image dash;

    public float cooldown_attack1;
    public float cooldown_attack2;
    public float cooldown_csa;

    public float cooldown_attack1_k;
    public float cooldown_attack2_k;
    public float cooldown_csa_k;
    public float cooldown_attack1_a;
    public float cooldown_attack2_a;
    public float cooldown_csa_a;
    public float cooldown_attack1_m;
    public float cooldown_attack2_m;
    public float cooldown_csa_m;
    public float cooldown_dash;


    bool iscooldown = false;
    bool iscooldown2 = false;
    bool iscooldown3 = false;
    bool iscooldown4 = false;
    public KeyCode ability1;
    public KeyCode ability2;
    public KeyCode ability3;
    public KeyCode ability4;

    [SerializeField] GameObject Knight;
    bool isknight = true;
    [SerializeField] GameObject Archer;
    bool isarcher = false;
    [SerializeField] GameObject Mage;
    bool ismage = false;

    [SerializeField] PlayerActions player_actions;


    void Start()
    {

        for (int i=0; i< Abilities.Length; i++)
        {
           Abilities[i].fillAmount = 0;
        }

        attack_1.fillAmount = 0;
        attack_2.fillAmount = 0;
        csa.fillAmount = 0;
        dash.fillAmount = 0;
   
    }

    void Update()
    {
        ChangeSkillset();
        Ability1();
        Ability2();
        Ability4();
        Ability3();

    }

    void Ability1()
    {
        if(Input.GetKey(ability1) && iscooldown == false)
        {
            iscooldown = true;
            attack_1.fillAmount = 1;
            
        }

        if(iscooldown)
        {
            attack_1.fillAmount -= 1 / cooldown_attack1 * Time.deltaTime;

            if(attack_1.fillAmount <=0)
            {
                attack_1.fillAmount = 0;
                iscooldown = false;
            }
        }
    }

    void Ability2()
    {
        if(Input.GetKey(ability2) && iscooldown2 == false)
        {
            iscooldown2 = true;
            attack_2.fillAmount = 1;
        }

        if(iscooldown2)
        {
            attack_2.fillAmount -= 1 / cooldown_attack2 * Time.deltaTime;

            if(attack_2.fillAmount <=0)
            {
                attack_2.fillAmount = 0;
                iscooldown2 = false;
            }
        }
    }

    void Ability3()
    {
        if(Input.GetKey(ability3) && iscooldown3 == false)
        {
            iscooldown3 = true;
            csa.fillAmount = 1;
        }

        if(iscooldown3)
        {
            csa.fillAmount -= 1 / cooldown_csa * Time.deltaTime;

            if(csa.fillAmount <=0)
            {
                csa.fillAmount = 0;
                iscooldown3 = false;
            }
        }
    }

    void Ability4()
    {
        if(Input.GetKey(ability4) && iscooldown4 == false)
        {
            iscooldown4 = true;
            dash.fillAmount = 1;
        }

        if(iscooldown4)
        {
            dash.fillAmount -= 1 / cooldown_dash * Time.deltaTime;

            if(dash.fillAmount <=0)
            {
                dash.fillAmount = 0;
                iscooldown4 = false;
            }
        }
    }

    void ChangeSkillset()
    {
        if (PlayerActions.currentClassIndex == 0 && isknight == false)
        {
            Archer.SetActive(false);
            Mage.SetActive(false);
            Knight.SetActive(true);
            isknight = true;
            isarcher = false;
            ismage = false;
        }

        if (PlayerActions.currentClassIndex == 1 && isarcher == false)
        {
            Archer.SetActive(true);
            Mage.SetActive(false);
            Knight.SetActive(false);
            isarcher = true;
            isknight = false;
            ismage = false;
        }

        if (PlayerActions.currentClassIndex == 2 && ismage == false)
        {
            Archer.SetActive(true);
            Mage.SetActive(true);
            Knight.SetActive(false);
            isarcher = false;
            isknight = false;
            ismage = true;

        }

        

    }
}
