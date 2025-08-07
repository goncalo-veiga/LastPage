using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MagicAttack : MonoBehaviour
{
    [Header("Magic Ball")]
    [SerializeField] private GameObject MagicBall;
    [SerializeField] private float MagicDamage;
    [SerializeField] private float MagicSpeed;
    [SerializeField] private float ReleaseTime, SpawnTime, PeaceTime;
    public int numberOfBalls, upgradeNumberOfBalls;
    [Header("Dash Slash")]
    [SerializeField] private float SlashDamage;
    [SerializeField] private float SlashTime, StartDashTime, DashTime;

    private bool doneSlash, ndDash;

    [Header("Hands")]
    [SerializeField] private float HandTime;
    [SerializeField] private float HandDamage;

    private BossHands LeftHand, RightHand;
    private Vector2 leftPos, rightPos;
    private float stepHandsTime;

    [SerializeField] private Transform Player;
    
    private float currentTimer, size, maxDistance;
    private int ball = 0, releaseBall;
    private Projectile[] magicBalls;

    [HideInInspector] public bool isAttackFinnished;
    [HideInInspector] public int attackNumber;
    private bool canAttack;

    private EnemyHP Health;
    private RoomTp Tp;
    private Rigidbody2D rb;
    private bool OnRampage;

    // Start is called before the first frame update
    void Start()
    {
        isAttackFinnished=true;
        Health=GetComponent<EnemyHP>();
        Tp=GetComponent<RoomTp>();
        rb = GetComponent<Rigidbody2D>();

        RightHand = transform.GetChild(1).GetComponent<BossHands>();
        LeftHand = transform.GetChild(0).GetComponent<BossHands>();

        RightHand.col.enabled = false;
        LeftHand.col.enabled = false;

        RightHand.damageMultiplier = HandDamage;
        LeftHand.damageMultiplier = HandDamage;

        leftPos = LeftHand.transform.localPosition;
        rightPos = RightHand.transform.localPosition;

        RightHand.Player = Player;
        LeftHand.Player = Player;

        GetComponent<Collider2D>().enabled = false;


        size = Mathf.Abs(transform.localScale.x);
        maxDistance = -size * (numberOfBalls - 1) / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackNumber == 0 && !isAttackFinnished) AttackMagicBallsLoop();

        if (isAttackFinnished && currentTimer>=PeaceTime)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            LeftHand.transform.localPosition = leftPos;
            RightHand.transform.localPosition = rightPos;



            float help = 1 - Health.currentHealth / Health.startingHealth;
            OnRampage = help >= 2f / 3f;
            help = (help > 2f / 3f) ? 3 : help*3+2;

            attackNumber = Random.Range(0, (int)help);


            if (!GetComponent<Collider2D>().enabled)
            {
                GetComponent<Collider2D>().enabled = true;
                attackNumber = 0;
            }

            isAttackFinnished = false;

            if (OnRampage)
            {
                numberOfBalls = upgradeNumberOfBalls;
                maxDistance = -size * (numberOfBalls - 1) / 2;
            }


            int dir = (int)(Random.value * 2);
            Vector2 pos = (attackNumber == 1) ? Tp.bottomLeft + (Tp.xShift + dir * (-2 * Tp.xShift + Tp.width)) * Vector2.right : default;

            Tp.Teleport(pos, (attackNumber == 0) ? -maxDistance + size / 4 : -1);

            if (attackNumber == 0) AttackMagicBallsSetup();
            else if (attackNumber == 1) AttackDashSlashSetup(1 - dir * 2);
            else if (attackNumber == 2) AttackHandSetup();
        }
        
    }

    private void FixedUpdate()
    {
        currentTimer += Time.fixedDeltaTime;

        if (attackNumber == 1 && !isAttackFinnished) AttackDashSlashLoop();
        else if (attackNumber == 2 && !isAttackFinnished) AttackHandLoop();
    }

    private void AttackMagicBallsSetup()
    {
        magicBalls = new Projectile[numberOfBalls];
        currentTimer = SpawnTime;

        releaseBall = 0;
        ball = 0;
    }

    private void AttackMagicBallsLoop()
    {
        if (currentTimer >= SpawnTime && ball < numberOfBalls)
        {
            magicBalls[ball] = Instantiate(MagicBall, (Vector2)transform.position + Vector2.right * (size * ball + maxDistance) + Vector2.up*GetComponent<Collider2D>().bounds.extents.y, Quaternion.identity, transform).GetComponent<Projectile>();
            magicBalls[ball].gameObject.layer=gameObject.layer;
            magicBalls[ball].transform.localScale =new Vector2(0.5f, 0.5f);
            currentTimer = 0f;
            ball += 1;
        }


        if (currentTimer >= ReleaseTime && ball == numberOfBalls && releaseBall < numberOfBalls)
        {
            Vector2 difference = Player.position - magicBalls[releaseBall].transform.position;
            magicBalls[releaseBall].transform.right = difference;

            magicBalls[releaseBall].Fire(MagicSpeed, Mathf.Infinity, Mathf.Infinity, MagicDamage, 1 <<Player.gameObject.layer);
            currentTimer = 0f;
            releaseBall += 1;

            isAttackFinnished = releaseBall==numberOfBalls;
        }

    }

    private void AttackDashSlashSetup(int dir)
    {
        currentTimer = 0f;
        doneSlash = false;

        Vector2 scale = transform.localScale;

        scale.x *= dir;

        transform.localScale = scale;
        canAttack = true;
        ndDash = false;

    }

    private void AttackDashSlashLoop()
    {
        if(currentTimer >= StartDashTime && !doneSlash)
        {
            rb.velocity = Vector2.right * Mathf.Sign(transform.localScale.x)*(Tp.width-2*Tp.xShift)/DashTime;
            doneSlash=true;
        }

        if (currentTimer >= DashTime+StartDashTime )
        {
            rb.velocity=Vector2.zero;
        }

        if (OnRampage && currentTimer >= DashTime + StartDashTime && currentTimer <= 2*DashTime + StartDashTime)
        {
            if (!ndDash)
            {
                ndDash = true;
                doneSlash = false;

                Vector2 scale = transform.localScale;

                scale.x *= -1;

                transform.localScale = scale;

            }
            
            rb.velocity = Vector2.right * Mathf.Sign(transform.localScale.x) * (Tp.width-2*Tp.xShift) / DashTime;
        }

        if (currentTimer >= SlashTime)
        {
            isAttackFinnished = true;
            canAttack = false;
        }
    }

    private void AttackHandSetup()
    {
        currentTimer = 0f;
        stepHandsTime = Mathf.PI / HandTime;

        LeftHand.col.enabled = true;
        RightHand.col.enabled = true;

        LeftHand.transform.position = Tp.bottomLeft+Vector2.up*Tp.height;
        RightHand.transform.position = Tp.bottomLeft + Vector2.up * Tp.height + Vector2.right * Tp.width;

        Vector2 scale = LeftHand.transform.localScale;
        scale.x *= -1;
        LeftHand.transform.localScale = scale;
        RightHand.transform.localScale = scale;

        Quaternion quaternion = Quaternion.Euler(0, 0, 90);

        LeftHand.transform.localRotation = quaternion;

        quaternion.z *= -1;

        RightHand.transform.localRotation = quaternion;
    }

    private void AttackHandLoop()
    {
        float multiplier = (OnRampage) ? 2 : 1;



        LeftHand.transform.position = Tp.bottomLeft + Vector2.up * Tp.height + new Vector2(Mathf.Sin(stepHandsTime*currentTimer)*Tp.width/2*multiplier,-Tp.height/HandTime*currentTimer);

        RightHand.transform.position = Tp.bottomLeft + Vector2.up * Tp.height + Vector2.right * Tp.width + new Vector2(-Mathf.Sin(stepHandsTime * currentTimer) * Tp.width / 2 * multiplier, -Tp.height / HandTime * currentTimer);

        if (currentTimer > HandTime / 2)
        {
            Vector2 scale = LeftHand.transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            LeftHand.transform.localScale =scale;
            RightHand.transform.localScale=scale;


            Quaternion quaternion = Quaternion.Euler(0, 0, -90);

            LeftHand.transform.localRotation = quaternion;

            quaternion.z *= -1;

            RightHand.transform.localRotation = quaternion;
        }

        if (currentTimer >= HandTime)
        {
            isAttackFinnished = true;
            LeftHand.transform.localPosition = leftPos;
            RightHand.transform.localPosition = rightPos;

            LeftHand.col.enabled = false;
            RightHand.col.enabled = false;

            LeftHand.transform.localRotation = Quaternion.identity;
            RightHand.transform.localRotation = Quaternion.identity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canAttack && collision.transform == Player.transform) Player.GetComponent<health>().TakeDamage(SlashDamage,gameObject);
    }
}
