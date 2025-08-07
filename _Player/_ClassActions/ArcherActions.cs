using System;
using System.Collections;
using System.Dynamic;
using System.Linq;
using Unity.VisualScripting;
//using UnityEditor.Animations;
using UnityEngine;

public class ArcherActions : ClassActions
{
    private Bow Bow;
    private GameObject[] Arrows;
    [SerializeField] float spread = 1f;


    public bool isAttack1 { get; private set; }
    public bool isAttack2 { get; private set; }


    override protected void Awake()
    {
        base.Awake();

        Transform _bow = transform.Find("Weapons/Bow");

        _bow.parent = RightHand.GetChild(0).transform;
        _bow.position = _bow.parent.position;
        _bow.rotation = _bow.parent.rotation;

        Color colour = _bow.Find("Sprite").GetComponent<SpriteRenderer>().color;
        colour.a = 1f;
        _bow.Find("Sprite").GetComponent<SpriteRenderer>().color=colour;

        _bow.Find("Sprite").GetComponent<SpriteRenderer>().sortingOrder = 1;

        Bow = RightHand.Find("RHand/Bow").GetComponent<Bow>();
        _bow.parent.localRotation = Quaternion.Euler(0f, 0f, -45f);
        RightHand.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;


    }
    // Update is called once per frame
    public override void Attack1()
    {
        GetComponent<PlayerMovement1>().canTurn = false;

        Rotate(LeftHand);
        Rotate(RightHand);

        Arrows = Bow.StartCharging(1);

        ArrowsPlace();

        isAttack1 = true;
        animator.SetBool("unstopable", true);
    }
    public override void Attack2()
    {
        GetComponent<PlayerMovement1>().canTurn = false;

        Rotate(LeftHand);
        Rotate(RightHand);

        Arrows = Bow.StartCharging(3);

        ArrowsPlace();

        isAttack2 = true;
        animator.SetBool("unstopable", true);
    }

    public override void OtherAction()
    {
        Bow.SetFireArrows();
    }

    public override void UpdateAction()
    {
        if (animator.GetCurrentAnimatorStateInfo(1).IsTag("attacking")) 
        {
            Rotate(LeftHand);
            Rotate(RightHand);

            if ((isAttack1 && !Input.GetKey(KeyCode.Mouse0)) || (isAttack2 && !Input.GetKey(KeyCode.Mouse1))) 
            {
                

                if(animator.GetCurrentAnimatorStateInfo(1).IsName("charge1Arrow") || animator.GetCurrentAnimatorStateInfo(1).IsName("charge3Arrows"))
                {
                    StartCoroutine(AbleToRelease());
                }
                else
                {
                    animator.SetTrigger("Release");
                    Bow.LetItGo();
                    GetComponent<PlayerMovement1>().canTurn = true;
                }

                isAttack1 = false;
                isAttack2 = false;

                Arrows = null;
            }

            if(!Bow.isCharging)
            {
                LeftHand.rotation = Quaternion.identity;
                RightHand.rotation = Quaternion.identity;
            }
        } 
    }

    private IEnumerator AbleToRelease()
    {
        yield return new WaitUntil(()=> animator.GetCurrentAnimatorStateInfo(1).normalizedTime>=0.2);
        animator.SetTrigger("Release");
        Bow.LetItGo();
        GetComponent<PlayerMovement1>().canTurn = true;
        LeftHand.rotation = Quaternion.identity;
        RightHand.rotation = Quaternion.identity;
    }
    public void ChargeArrow()
    {
        Bow.ChargeUp();
    }

    override public void GetWeaponsBack()
    {
        Transform Weapons = transform.root.Find("Weapons");
        Transform _bow = RightHand.GetChild(0).GetChild(0).transform;

        _bow.parent.localRotation = Quaternion.Euler(0f, 0f, 0);
        RightHand.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;

        _bow.parent = Weapons;
    }

    private void ArrowsPlace()
    {
        float currentAngle = -spread * (Arrows.Length - 1) / 2;

        for (int i = 0; i < Arrows.Length; i++)
        {
            Arrows[i].transform.localScale = transform.localScale;
            Arrows[i].transform.parent = LeftHand.GetChild(0);
            Arrows[i].transform.localPosition = new Vector3(0f,0f,0f);
            Arrows[i].transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
            
            currentAngle += spread;
        }
    }
}