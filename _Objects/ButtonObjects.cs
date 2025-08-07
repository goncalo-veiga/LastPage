using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtonObjects : MonoBehaviour
{
    [SerializeField] private ButtonActivatedObjects ActivatableObject;
    [SerializeField] private Sprite[] States;
    [SerializeField] private float enterTime,exitTime;
    protected bool isActive, onChange;
    private SpriteRenderer sprt;
    protected Collider2D col;
    private int step, endValue, i;
    private float currTime, timeMax;

    protected virtual void Start()
    {
        col= GetComponent<Collider2D>();
        sprt= GetComponent<SpriteRenderer>();
        sprt.sprite = States[0];
    }

    protected virtual void OnActivation()
    {
        step = 1; i = 1; endValue = States.Length; timeMax=enterTime;

        if (isActive)
        {
            step = -1;
            i = endValue-2;
            endValue = -1;
            
            timeMax= exitTime;
        }

        currTime = 0f;
        onChange = true;

    }

    protected virtual void Update()
    {
        if (onChange)
        {
            if (currTime > timeMax)
            {
                sprt.sprite = States[i];
                i += step;

                currTime = 0f;

                if (i == endValue)
                {
                    onChange = false;
                    isActive = !isActive;
                    ActivatableObject.Change();
                }
            }

            currTime += Time.deltaTime;
        }
    }

}
