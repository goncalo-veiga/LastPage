using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;
using System;
using System.Linq;

public class Movables : MonoBehaviour
{
    // Start is called before the first frame update
    public bool pickable, pushable, pullable;
    public float speedFactor;

    private float mass, gravity, drag;
    private Rigidbody2D rb;
    private Collider2D col;
    [SerializeField] private LayerMask wallLayer;
    [HideInInspector] public bool isGrabed;

    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        mass = rb.mass;
        gravity = rb.gravityScale;
        drag = rb.drag;
        isGrabed = false;
        isGrounded = false;
    }

    private void Update()
    {
        isGrounded = GroundCheck();

        if (!pickable && isGrabed && (!isGrounded || !PossibleMove())) isGrabed= false;
    }

    public void Grabbing(Transform parent, Vector2 normal, float distance)
    {
        isGrabed = true;
        transform.position = transform.position;// + distance * normal.x * Vector3.right;


        if (pickable)
        {

            transform.position = new Vector2(transform.position.x + distance * normal.x, parent.position.y);
        }


        Destroy(gameObject.GetComponent<Rigidbody2D>());

        transform.parent = parent;
    }

    public void SettingDown(float height=0)
    {
        isGrabed = false;

        Vector2 velocity = GetComponentInParent<Rigidbody2D>().velocity;
        transform.parent = null;

        rb=gameObject.AddComponent<Rigidbody2D>();
        rb.mass = mass;
        rb.gravityScale = gravity;
        rb.drag= drag;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        if (pickable)
        {
            if(height!=0) transform.position = transform.position + Vector3.down * (height - col.bounds.extents.y);
            rb.velocity = velocity;
        }

        if (isGrounded) rb.bodyType = RigidbodyType2D.Static;
    }

    private bool GroundCheck()
    {
        Vector2 position = transform.position;
        
        RaycastHit2D[] raycastHits = Physics2D.BoxCastAll(position, col.bounds.extents*2,0,Vector2.down,0.05f, wallLayer);

        foreach(RaycastHit2D rayHit in raycastHits)
        {
            if (rayHit.collider.transform == transform)
            {
                raycastHits = raycastHits.Except(new RaycastHit2D[] { rayHit }).ToArray();
                break;
            }
        }

        bool result = raycastHits.Length > 0;

        if (!isGrabed && !isGrounded && result) rb.bodyType = RigidbodyType2D.Static;
        if (!isGrabed && isGrounded && !result) rb.bodyType = RigidbodyType2D.Dynamic;

        return result;
    }

    private bool PossibleMove()
    {
        if (!pushable && Input.GetAxisRaw("Horizontal") * transform.parent.localScale.x > 0) return false;
        else if(!pickable && Input.GetKeyDown(KeyCode.Space)) return false;
        return true;
    }
}
