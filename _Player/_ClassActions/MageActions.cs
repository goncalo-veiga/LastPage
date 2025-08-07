using System;
using System.Collections;
using System.Dynamic;
using System.Linq;
using Unity.VisualScripting;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.XR;

public class MageActions : ClassActions
{
    private Staff Staff;
    private GameObject Spell;

    private int attack1Stage = 0;
    private Vector2 savePos;
    public bool isAttack1 { get; private set; }
    public bool isAttack2 { get; private set; }


    override protected void Awake()
    {
        base.Awake();

        Transform _staff = transform.Find("Weapons/Staff");

        _staff.parent = LeftHand.GetChild(0).transform;
        _staff.position = _staff.parent.position;
        _staff.rotation = _staff.parent.rotation;

        Color colour = _staff.Find("Sprite").GetComponent<SpriteRenderer>().color;
        colour.a = 1f;
        _staff.Find("Sprite").GetComponent<SpriteRenderer>().color=colour;

        _staff.Find("Sprite").GetComponent<SpriteRenderer>().sortingOrder = 3;

        Staff = _staff.GetComponent<Staff>();

        Transform _book = transform.Find("Weapons/Book");

        _book.parent = RightHand.GetChild(0).transform;
        _book.position = _book.parent.position;
        _book.rotation = _book.parent.rotation;

        _book.Find("Sprite").GetComponent<SpriteRenderer>().color = colour;

        _book.Find("Sprite").GetComponent<SpriteRenderer>().sortingOrder = 1;


        RightHand.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;


    }

    protected override void Start()
    {
        base.Start();
    }
    // Update is called once per frame
    public override void Attack1()
    {
        if (attack1Stage == 0)
        {
            Spell = Staff.SummonSpell();

            MagicPlace();
            
            attack1Stage += 1;
        }
        else if (attack1Stage == 1)
        {
            Debug.Log(LeftHand.name);

            Rotate(LeftHand, savePos);
            RotateBolt(Spell.transform, savePos - (Vector2)Spell.transform.position + (Vector2)LeftHand.position);


            Staff.LetItGo();
            attack1Stage += 1;
        }
    }
    public override void Attack2()
    {
        Staff.YouShallNotPass();
    }

    public override void OtherAction()
    {
        int dir = Math.Sign(transform.localScale.x);
        Vector2 checkPos = transform.position;
        RaycastHit2D raycastHit = Physics2D.CircleCast(checkPos, GetComponent<Collider2D>().bounds.extents.y, Vector2.right * dir, 1f, GetComponent<PlayerActions>().movableLayers);

        if (raycastHit.collider!=null && raycastHit.collider.TryGetComponent<Levitables>(out Levitables levitableObj)) levitableObj.Levitate();
    }

    public override void UpdateAction()
    {
        if (!(Staff.isAttacking1 == animator.GetCurrentAnimatorStateInfo(1).IsName("cast")))
        {
            Staff.isAttacking1 = !Staff.isAttacking1;
            attack1Stage = 0;
            animator.SetBool("unstopable", Staff.isAttacking1);
            GetComponent<PlayerMovement1>().canTurn = !Staff.isAttacking1;

            if (!Staff.isAttacking1) LeftHand.rotation = Quaternion.identity;
            else
            {
                savePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                savePos -= (Vector2)transform.position;

                Vector2 scale = transform.localScale;

                scale.x *= (scale.x * (savePos.x) >= 0) ? 1 : -1;

                transform.localScale = scale;
            }

        }

        if(!(Staff.isAttacking2 == animator.GetCurrentAnimatorStateInfo(1).IsName("youShallNotPass")))
        {
            Staff.isAttacking2 = !Staff.isAttacking2;
            animator.SetBool("unstopable", Staff.isAttacking2);

            GetComponent<PlayerMovement1>().enabled = !Staff.isAttacking2;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            animator.SetBool("run", !Staff.isAttacking2);
        }
    }

    override public void GetWeaponsBack()
    {
        Transform Weapons = transform.root.Find("Weapons");
        Transform _staff = LeftHand.GetChild(0).GetChild(0).transform;

        _staff.parent.localRotation = Quaternion.Euler(0f, 0f, 0);
        _staff.parent = Weapons;

        Transform _book = RightHand.GetChild(0).GetChild(0).transform;
        _book.parent.localRotation = Quaternion.Euler(0f, 0f, 0);
        _book.parent = Weapons;

        RightHand.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
    }

    private void MagicPlace()
    {
        Spell.transform.parent = Staff.transform.Find("summonPoint");
        Spell.transform.localScale = new Vector3(1, 1, 1);
        Spell.transform.localPosition = Vector3.zero;
        Spell.transform.localRotation = Quaternion.identity;

    }

    protected void RotateBolt(Transform bolt, Vector2 difference = default)
    {
        difference = (difference != default) ? difference : Camera.main.ScreenToWorldPoint(Input.mousePosition) - bolt.position;
        Vector2 prev = bolt.localScale;

        difference = difference.normalized;


        if (prev.x * difference.x < 0)
        {
            bolt.localScale = new Vector2(-prev.x, prev.y);
        }

        if (difference.x*transform.localScale.x < 0) difference= -difference;
        
        bolt.right = difference;
    }
}