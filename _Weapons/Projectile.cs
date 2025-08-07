using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private LayerMask enemiesLayer;
    private float antiGravityTime, destroyTime, currentTime, damageMultiplier, maxSpeed;
    private Vector2 knockBack;
    private int piercing = 0;
    private bool onTheMove, hasAnimator, continueWhileDead;
    private Animator animator;

    void Awake()
    {
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        animator= GetComponent<Animator>();
    }

    private void Start()
    {
        gameObject.tag = "Projectile";
        gameObject.layer = transform.root.gameObject.layer;

        hasAnimator = animator != null && animator.runtimeAnimatorController != null;
    }
    // Update is called once per frame
    void Update()
    {
        if(onTheMove)
        {
            currentTime += Time.deltaTime;

            if(currentTime >= destroyTime)
            {
                if (hasAnimator && animator.HasState(0, Animator.StringToHash("exit"))) StartCoroutine(DestructionSquence());
                else {
                    Destroy(gameObject);
                }
            }
            else if (currentTime > antiGravityTime) 
            {
                rb.isKinematic = false;
                Rotate();
            }

            if (rb.velocity.magnitude > maxSpeed)
            {
                float magnitude = rb.velocity.magnitude * 0.995f;
                magnitude = (magnitude < maxSpeed) ? maxSpeed : magnitude;
                rb.velocity = rb.velocity.normalized * magnitude;
            }
        }
    }

    public void Fire(float speed, float antiGravity, float destroy, float damage, LayerMask enemies, int nrPierces = 0, Vector2 knock = default, bool whileDead=false)
    {
        piercing = nrPierces;
        gameObject.GetComponent<Collider2D>().enabled = true;

        enemiesLayer = enemies;
        antiGravityTime = antiGravity;
        destroyTime = destroy;
        damageMultiplier = damage;
        maxSpeed = speed;
        knockBack = knock;
        continueWhileDead = whileDead;

        bool hasRootVelocity = transform.root.TryGetComponent<Rigidbody2D>(out Rigidbody2D baseRB);
        Vector2 baseVelocity = (hasRootVelocity) ? baseRB.velocity: Vector2.zero;
        currentTime = 0;

        transform.parent = null;
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.isKinematic = true;

        

        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;

        if (angle > 90 || angle <= -90)
        {
            angle = (Mathf.Sign(angle) * 180 - angle);
        }

        Vector2 newVelocity = new Vector2(Mathf.Cos(angle) * Mathf.Sign(transform.localScale.x), Mathf.Sin(angle) * Mathf.Sign(transform.localScale.x)) * speed;

        baseVelocity.x = (baseVelocity.x * newVelocity.x < 0) ? 0 : baseVelocity.x;
        baseVelocity.y = 0;

        rb.velocity = newVelocity + baseVelocity;

        Rotate();

        if (hasAnimator && animator.HasState(0, Animator.StringToHash("moving"))) animator.SetBool("moving", true);

        onTheMove = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isEnemy = (enemiesLayer.value & (1 << collision.gameObject.layer)) > 0;

        if (collision.gameObject.TryGetComponent<health>(out health health) && isEnemy) health.TakeDamage(damageMultiplier, gameObject);
        if (collision.gameObject.TryGetComponent<EnemyHP>(out EnemyHP healthEnemy) && isEnemy) healthEnemy.TakeDamage(damageMultiplier, gameObject);

        if (isEnemy && (healthEnemy || health))
        {

            if (collision.TryGetComponent<Rigidbody2D>(out Rigidbody2D collisionRB))
            {
                collisionRB.AddForce(knockBack, ForceMode2D.Impulse);
            }

            if (piercing <= 0)
            {
                if (hasAnimator && animator.HasState(0, Animator.StringToHash("exit"))) StartCoroutine(DestructionSquence());
                else Destroy(gameObject);
            }
            piercing -= 1;
        }
        else if (collision.TryGetComponent<Breakables>(out Breakables breakObj))
        {
            breakObj.Breaking();

            Debug.Log("Break it!");
            if (animator != null && animator.HasState(0, Animator.StringToHash("exit"))) StartCoroutine(DestructionSquence());
            else Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {

            if (hasAnimator && animator.HasState(0, Animator.StringToHash("exit"))) StartCoroutine(DestructionSquence());
            else
            {
                Destroy(gameObject);
            }
        }

    }

    private void Rotate()
    {
        Vector2 difference = rb.velocity.normalized;

        transform.right = Mathf.Sign(difference.x)*difference;

        if (transform.localScale.x * difference.x < 0)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    private IEnumerator DestructionSquence()
    {
        gameObject.GetComponent<Collider2D>().enabled = continueWhileDead;
        rb.velocity= Vector2.zero;
        animator.SetTrigger("dead");
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
