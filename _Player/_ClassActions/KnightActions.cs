using System;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class KnightActions : ClassActions
{
    private MeleeWeapon Weapon;
    private Shield Shield;

    private int reset;

    override protected void Awake()
    {
        base.Awake();

        Transform _weapon = transform.Find("Weapons/MeleeWeapon");
        _weapon.parent = LeftHand.GetChild(0).transform;
        _weapon.position = _weapon.parent.position;
        _weapon.rotation = _weapon.parent.rotation;

        Color colour = _weapon.Find("Sprite").GetComponent<SpriteRenderer>().color;
        colour.a = 1f;
        _weapon.Find("Sprite").GetComponent<SpriteRenderer>().color=colour;

        _weapon.Find("Sprite").GetComponent<SpriteRenderer>().sortingOrder = 3;


        Transform _shield = transform.Find("Weapons/Shield");
        _shield.parent = RightHand.GetChild(0).transform;

        Weapon = LeftHand.Find("LHand/MeleeWeapon").GetComponent<MeleeWeapon>();
        Shield = RightHand.Find("RHand/Shield").GetComponent<Shield>();

    }

    // Update is called once per frame

    public override void Attack1()
    {
        Rotate(LeftHand);
    }
    public override void Attack2()
    {
        if (reset>0 && reset%2==0) Weapon.ClearEnemies();
        reset++;
        Vector3 prev = transform.localScale;
        transform.localScale = new Vector3(-prev.x, prev.y, prev.z);
    }

    public override void OtherAction()
    {
        animator.SetTrigger("OtherAction");
    }

    public override void UpdateAction()
    {
        if (!(Weapon.isAttacking == animator.GetCurrentAnimatorStateInfo(1).IsTag("attacking")))
        {
            reset = 0;
            Weapon.UpdateAttacking(transform.localScale.y);
            animator.SetBool("unstopable", Weapon.isAttacking);

            GetComponent<PlayerMovement1>().canTurn = !Weapon.isAttacking;
            if (!Weapon.isAttacking) LeftHand.rotation = Quaternion.identity;
        }

        if (!(Shield.isBlocking == animator.GetCurrentAnimatorStateInfo(1).IsName("bash")))
        {
            Shield.SwitchBlocking();

            animator.SetBool("unstopable", Shield.isBlocking);
            GetComponent<PlayerMovement1>().onDash = Shield.isBlocking;
        }
    }

    override public void GetWeaponsBack()
    {
        Transform Weapons = transform.root.Find("Weapons");

        Transform _weapon = LeftHand.GetChild(0).GetChild(0).transform;
        _weapon.parent = Weapons;

        Transform _shield = RightHand.GetChild(0).GetChild(0).transform;
        _shield.parent = Weapons;
    }
}