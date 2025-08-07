using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Progress;

public class Shield : MonoBehaviour
{
    // Start is called before the first frame update
    private LayerMask enemiesLayer;
    [SerializeField] private Vector2 strength;
    public int shieldResistance;
    [HideInInspector] public bool isBlocking;

    private EdgeCollider2D box;
    void Start()
    {
        enemiesLayer = (LayerMask)Variables.Object(transform.root.gameObject).Get("enemiesLayer");
        isBlocking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isAttack= (enemiesLayer.value & (1 << collision.gameObject.layer)) > 0;

        if (isAttack)
        {
            bool isFacing = Math.Sign(transform.root.localScale.x) == Math.Sign((collision.transform.position - transform.position).x);

            if (collision.gameObject.CompareTag("Projectile")) Destroy(collision.gameObject);

            else if(isFacing && collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                rb.velocity = transform.root.GetComponent<Rigidbody2D>().velocity;
                rb.AddForce(new Vector2(strength.x * Mathf.Sign(transform.localScale.x), strength.y), ForceMode2D.Impulse);
            }

        }
    }

    public bool isBlockable(GameObject enemy)
    {
        bool isAttack = (enemiesLayer.value & (1 << enemy.layer)) > 0;
        bool isFacing = Math.Sign(transform.root.localScale.x) == Math.Sign((enemy.transform.position - transform.position).x);

        return isAttack && isFacing;
    }

    // Update is called once per frame
    public void SwitchBlocking()
    {
        isBlocking = !isBlocking;

        if (box==null)
        {
            box = gameObject.AddComponent<EdgeCollider2D>();
            Sprite spr = gameObject.GetComponentInChildren<SpriteRenderer>().sprite;

            float units = 1 / spr.pixelsPerUnit;
            float edgeRadius = Math.Min(spr.rect.width, spr.rect.height) / 2 * units;

            Vector2 delta = new(Math.Max(0, spr.rect.width * units / 2 - edgeRadius), Math.Max(0, spr.rect.height * units / 2 - edgeRadius));

            Vector2[] points = box.points;
            Vector2 center = new Vector2(spr.rect.width, spr.rect.height) / 2 * units - units * spr.pivot;

            points.SetValue(center + delta, 0);
            points.SetValue(center - delta, 1);

            box.points = points;
            box.edgeRadius = edgeRadius * transform.root.localScale.y;
        }
        else
        {
            Destroy(box);
            box = null;
        }

    }
}
