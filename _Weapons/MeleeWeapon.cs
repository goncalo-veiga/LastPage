using System;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public int damageMultiplier;
    private GameObject[] hitEnemies;
    [HideInInspector] public bool isAttacking;
    private LayerMask enemiesLayer;

    private void Start()
    {
        enemiesLayer = (LayerMask)Variables.Object(transform.root.gameObject).Get("enemiesLayer");
        hitEnemies = new GameObject[] { };
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isEnemy = (enemiesLayer.value & (1 << collision.gameObject.layer)) > 0;

        health health = collision.gameObject.GetComponent<health>();
        EnemyHP EneHP = collision.gameObject.GetComponent<EnemyHP>();

        if (isEnemy && (health!=null || EneHP!=null) && !hitEnemies.Contains(collision.gameObject))
        {
            hitEnemies=hitEnemies.Append(collision.gameObject).ToArray();
            if (health != null) health.TakeDamage(damageMultiplier, transform.root.gameObject);
            if(EneHP!=null) EneHP.TakeDamage(damageMultiplier, transform.root.gameObject);
        }
        else if (collision.TryGetComponent<Breakables>(out Breakables breakObj))
        {
            breakObj.Breaking();
            Debug.Log("Break it!");
        }
    }

    public void ClearEnemies()
    {
        int i = 0;
        foreach (GameObject enemy in hitEnemies)
        {
            i++;
            Debug.Log("Cleared " + i + ": " + enemy.name);
        }

        hitEnemies =new GameObject[] { };
    }

    public void UpdateAttacking(float ratio)
    {
        isAttacking = !isAttacking;

        if (isAttacking)
        {
            EdgeCollider2D collider = gameObject.AddComponent<EdgeCollider2D>();
            Sprite spr = transform.Find("Sprite").GetComponentInChildren<SpriteRenderer>().sprite;

            float units = 1 / spr.pixelsPerUnit;
            float edgeRadius = Math.Min(spr.rect.width, spr.rect.height) / 2 * units;

            Vector2 delta = new(Math.Max(0, spr.rect.width * units / 2 - edgeRadius), Math.Max(0, spr.rect.height * units / 2 - edgeRadius));

            Vector2[] points = collider.points;
            Vector2 center = new Vector2(spr.rect.width, spr.rect.height) / 2 * units - units * spr.pivot;

            points.SetValue(center + delta, 0);
            points.SetValue(center - delta, 1);

            collider.points = points;
            collider.edgeRadius = edgeRadius * ratio;

            collider.isTrigger = true;
        }

        else
        {
            Destroy(gameObject.GetComponent<EdgeCollider2D>());
            ClearEnemies();
        }
    }
}
