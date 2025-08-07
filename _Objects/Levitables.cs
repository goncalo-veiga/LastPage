using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Levitables : MonoBehaviour
{
    [SerializeField] private float floatingTime, distance, velocity, shakingSpeed, shakingAmmount, shakingStart;
    [SerializeField] private Vector2 normalMove;
    [SerializeField] private LayerMask wallLayer;
    private float floatingTimer, walkingTimer;
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sprt;
    private Vector2 startPos;
    
    // Start is called before the first frame update

    private void Start()
    {
        floatingTimer = floatingTime;
        walkingTimer = distance / velocity;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sprt = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (walkingTimer<distance/velocity)
        {
            transform.position += Time.fixedDeltaTime * velocity * (Vector3)normalMove;

            walkingTimer += Time.fixedDeltaTime;

            if (walkingTimer >= distance / velocity) startPos=transform.position;
        }

    }

    private void Update()
    {
        if (floatingTimer==0)
        {
            rb.bodyType = RigidbodyType2D.Static;
            if(TryGetComponent<Movables>(out Movables movable)) movable.enabled = false;
        }

        if (floatingTimer<floatingTime)
        {
            floatingTimer += Time.deltaTime;

            if (floatingTime - floatingTimer < shakingStart)
            {
                Vector2 newPos = startPos;

                newPos.x += Mathf.Sin(Time.time * shakingSpeed) * shakingAmmount;

                transform.position= newPos;
            }

            if (floatingTimer >= floatingTime) 
            { 
                transform.position = startPos;
                rb.bodyType = RigidbodyType2D.Dynamic;

                if (TryGetComponent<Movables>(out Movables movable)) movable.enabled = true;
            }
        }
    }

    public void Levitate()
    {
        if (GroundCheck())
        {
            rb = GetComponent<Rigidbody2D>();
            walkingTimer = 0f;
            floatingTimer = 0f;
        }
    }

    private bool GroundCheck()
    {
        Vector2 position = transform.position;

        RaycastHit2D[] raycastHits = Physics2D.BoxCastAll(position, col.bounds.extents * 2, 0, Vector2.down, 0.05f, wallLayer);

        foreach (RaycastHit2D rayHit in raycastHits)
        {
            if (rayHit.collider.transform == transform)
            {
                raycastHits = raycastHits.Except(new RaycastHit2D[] { rayHit }).ToArray();
                break;
            }
        }

        return raycastHits.Length > 0;
    }
}
