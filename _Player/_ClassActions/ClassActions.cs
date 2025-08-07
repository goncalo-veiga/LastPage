using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClassActions : MonoBehaviour
{
    protected Animator animator;
    protected Transform RightHand;
    protected Transform LeftHand;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        RightHand = transform.Find("RightHand");
        LeftHand = transform.Find("LeftHand");
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    public virtual void Attack1()
    {
    }

    public virtual void Attack2()
    {
    }

    public virtual void OtherAction()
    {
    }

    public virtual void UpdateAction()
    {
    }

    public virtual void GetWeaponsBack()
    {
    }

    protected void Rotate(Transform Hand, Vector2 difference = default)
    {
        difference = (difference != default) ? difference : Camera.main.ScreenToWorldPoint(Input.mousePosition) - Hand.position;
        Vector2 prev = transform.localScale;

        difference = difference.normalized;
        
        if (prev.x * difference.x < 0)
        {
            transform.localScale = new Vector2(-prev.x, prev.y);
        }

        if (difference.x<0) difference=-difference;
        Hand.right = difference;
    }
}
