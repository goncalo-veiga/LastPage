using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerActions : ClassActions
{
    private Collider2D col;
    private Movables movableObj;

    
    private ClassActions currentClass;
    public static int currentClassIndex { get; private set; }
    private Dictionary<String, Tuple<Vector3, Quaternion>> _weaponsInfo= new Dictionary<String, Tuple<Vector3, Quaternion>>();
    private bool onDodge;

    [field: SerializeField] public LayerMask movableLayers { get; private set; }

    [Space(20), Header("Class Properties")]
    [SerializeField] private RuntimeAnimatorController[] ClassAnimators;
    [SerializeField] private String[] Classes;

    [Space(20),Header("General Unlocked Abilities:")]
    public bool Dodge;
    public bool Grab;
    [Header("Class Specific Unlocked Abilities:")]
    public bool[] canBeClass = new bool[3];
    public bool[] canAttack1 = new bool[3], canAttack2 = new bool[3], canOtherAction = new bool[3];

    [field: SerializeField, Space(20), Header("General Cooldowns")] public float dodgeTime { get; private set; }
    [field: SerializeField] public float changeClassTime { get; private set; }

    [field: SerializeField, Header("Class Specific Cooldowns")] public float[] attack1Time { get; private set; }
    [field: SerializeField] public float[] attack2Time { get; private set; }
    [field: SerializeField] public float[] otherActionTime { get; private set; }

    [HideInInspector] public float dodgeTimer { get; private set; }
    [HideInInspector] public float changeClassTimer {get;private set;}
    [HideInInspector] public float[] attack1Timer { get; private set; }
    [HideInInspector] public float[] attack2Timer { get; private set; }
    [HideInInspector] public float[] otherActionTimer { get; private set; }

    protected override void Awake()
    {
        foreach (Transform child in transform.Find("Weapons")) 
        {
            _weaponsInfo.Add(child.name, new Tuple<Vector3, Quaternion>(child.localPosition,child.localRotation));
        }

        base.Awake();
        col = GetComponent<Collider2D>();

        currentClassIndex = 0;
        currentClass = (ClassActions)gameObject.AddComponent(Type.GetType(Classes[currentClassIndex]));

    }

    protected override void Start()
    {
        base.Start();
        animator.runtimeAnimatorController = ClassAnimators[currentClassIndex];
        animator.Rebind();
        animator.Update(0f);

        dodgeTimer= dodgeTime;
        changeClassTimer= changeClassTime;

        attack1Timer= new float[3];
        attack2Timer= new float[3];
        otherActionTimer= new float[3];

        for(int i = 0; i < 3; i++)
        {
            attack1Timer[i]= attack1Time[i];
            attack2Timer[i]= attack2Time[i];
            otherActionTimer[i]= otherActionTime[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        dodgeTimer += Time.deltaTime;
        changeClassTimer += Time.deltaTime;
        
        for (int i=0;i<3;i++)
        {
            attack1Timer[i]+= Time.deltaTime;
            attack2Timer[i]+= Time.deltaTime;
            otherActionTimer[i]+= Time.deltaTime;
        }



        int newIndex = currentClassIndex;

        if (!animator.GetBool("unstopable")) 
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack1[currentClassIndex] && attack1Timer[currentClassIndex] >= attack1Time[currentClassIndex])
            {
                animator.SetTrigger("Attack1");
                attack1Timer[currentClassIndex] = 0;
            }

            else if (Input.GetKeyDown(KeyCode.Mouse1) && canAttack2[currentClassIndex] && attack2Timer[currentClassIndex] >= attack2Time[currentClassIndex])
            {
                animator.SetTrigger("Attack2");
                attack2Timer[currentClassIndex] = 0;
            }

            else if (Input.GetKeyDown(KeyCode.R) && canOtherAction[currentClassIndex])
            {
                currentClass.OtherAction();
                otherActionTimer[currentClassIndex] = 0;
            }

            else if (Input.GetKeyDown(KeyCode.LeftShift) && Dodge && dodgeTimer >= dodgeTime)
            {
                animator.SetTrigger("Dodge");
                dodgeTimer = 0;
            }

            else if (Input.GetKeyDown(KeyCode.E) && Grab) Grabbing();

            else if (Input.GetKeyDown(KeyCode.Mouse4) || Input.GetAxis("Mouse ScrollWheel") > 0f && changeClassTimer >= changeClassTime)
            {
                newIndex = Array.FindIndex(canBeClass, (currentClassIndex + 1) % canBeClass.Length, c => c);
                newIndex = (newIndex==-1) ? Array.FindIndex(canBeClass, c => c) : newIndex;

            }

            else if (Input.GetKeyDown(KeyCode.Mouse3) || Input.GetAxis("Mouse ScrollWheel") < 0f && changeClassTimer >= changeClassTime)
            {
                Array.Reverse(canBeClass);

                newIndex = Array.FindIndex(canBeClass, (canBeClass.Length - currentClassIndex) % canBeClass.Length, c => c) ;
                newIndex = (newIndex == -1) ? Array.FindIndex(canBeClass, c => c) : newIndex;
                newIndex = canBeClass.Length - newIndex - 1;

                Array.Reverse(canBeClass);
            }

            else if (Input.GetKeyDown(KeyCode.Alpha1) && canBeClass[0] && changeClassTimer >= changeClassTime) newIndex = 0;

            else if (Input.GetKeyDown(KeyCode.Alpha2) && canBeClass[1] && changeClassTimer >= changeClassTime) newIndex = 1;

            else if (Input.GetKeyDown(KeyCode.Alpha3) && canBeClass[2] && changeClassTimer >= changeClassTime) newIndex = 2;

            if (newIndex != currentClassIndex)
            {
                animator.SetTrigger("ExitClass");
                currentClassIndex = newIndex;
                changeClassTimer = 0;
            }
        }

        UpdateAction();

    }

    public override void UpdateAction()
    {
        currentClass.UpdateAction();

        if(movableObj != null && animator.GetCurrentAnimatorStateInfo(1).IsName("grab"))
        {
            if (movableObj.isGrabed) animator.SetBool("unstopable", true);

            if (Input.GetKeyDown(KeyCode.E) || !movableObj.isGrabed)
            {
                movableObj.SettingDown(col.bounds.extents.y);
                //transform.GetComponent<PlayerMovement>().speed /= movableObj.GetComponent<Movables>().speedFactor;
                GetComponent<PlayerMovement1>().canTurn = true;

                movableObj = null;
                animator.SetTrigger("Release");
            }
        }

        if (!(onDodge == animator.GetCurrentAnimatorStateInfo(1).IsName("dodge")))
        {
            onDodge = !onDodge;

            if (onDodge) 
            {
                StartCoroutine(GetComponent<health>().Invunerability(1, animator.GetCurrentAnimatorStateInfo(1).length, Color.gray, Color.gray));
            }
            

            animator.SetBool("unstopable", onDodge);

            GetComponent<PlayerMovement1>().onDash = onDodge;
        }

        if (animator.GetCurrentAnimatorStateInfo(1).IsName("exitClass")) animator.SetBool("unstopable", true);
    }

    public void getBack()
    {
        animator.SetBool("unstopable", false);
    }

    public void Stop()
    {
        animator.SetBool("unstopable", true);
    }

    public void Grabbing()
    {
        int dir = Math.Sign(transform.localScale.x);
        Vector2 checkPos = transform.position + new Vector3(col.bounds.extents.x * dir, -col.bounds.extents.y / 2, 0);
        RaycastHit2D[] raycastHits = Physics2D.RaycastAll(checkPos, Vector2.right * dir, 0.1f, movableLayers);


        foreach (RaycastHit2D raycastHit in raycastHits)
        {
            if (raycastHit && raycastHit.collider.TryGetComponent <Movables>(out Movables movable) && animator.GetBool("grounded"))
            {
                movableObj = movable;

                movableObj.Grabbing(transform, raycastHit.normal, raycastHit.distance);

                animator.SetTrigger("Grabbing");

                Debug.Log("Grabbing: " + raycastHit.collider.name);

                //transform.GetComponent<PlayerMovement>().speed *= movableObj.GetComponent<Movables>().speedFactor;
                GetComponent<PlayerMovement1>().canTurn = movableObj.pickable;

                break;
            }
        }
    }

    public void ChangeClass()
    {
        animator.runtimeAnimatorController = null;

        currentClass.GetWeaponsBack();

        foreach (Transform child in transform.Find("Weapons"))
        {
            child.localPosition = _weaponsInfo[child.name].Item1;
            child.localRotation = _weaponsInfo[child.name].Item2;
            if (child.name != "Shield")
            {
                SpriteRenderer sprt = child.Find("Sprite").GetComponent<SpriteRenderer>();
                sprt.sortingOrder = -3;
                Color colour = sprt.color;
                colour.a = 0;
                sprt.color = colour;
            }
        }

        currentClass = (ClassActions)gameObject.AddComponent(Type.GetType(Classes[currentClassIndex]));
        

        animator.runtimeAnimatorController = ClassAnimators[currentClassIndex];


        animator.Rebind();
        animator.Update(0f);

        foreach (ClassActions action in GetComponents<ClassActions>())
        {

            if (action is not PlayerActions && action!=currentClass) Destroy(action);
        }
    }

}