using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;

public class Staff : MonoBehaviour
{
    public float baseVelocity, damageMultiplier, destroyTimer;
    public Vector2 knockBack;
    [HideInInspector] public bool isAttacking1, isAttacking2;
    private LayerMask enemiesLayer;
    private Projectile magicBolt, magicKnock;

    [Header("Magic Spells")]
    [SerializeField] private GameObject MagicBolt, MagicKnock;

    private void Start()
    {
        enemiesLayer = (LayerMask)Variables.Object(transform.root.gameObject).Get("enemiesLayer");
    }

    // Update is called once per frame
    public GameObject SummonSpell()
    {
        GameObject result = Instantiate(MagicBolt);

        result.layer = gameObject.layer;
        result.GetComponentInChildren<SpriteRenderer>().sortingLayerName = transform.GetComponentInChildren<SpriteRenderer>().sortingLayerName;
        result.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
        magicBolt = result.GetComponent<Projectile>();

        return result;
    }
    public void LetItGo()
    {
        magicBolt.Fire(baseVelocity, destroyTimer+10, destroyTimer, damageMultiplier, enemiesLayer);

        magicBolt = null;
    }

    public void YouShallNotPass()
    {
        StartCoroutine(wait());
        /*
        Collider2D col = transform.root.GetComponent<Collider2D>();

        Vector2 position=col.bounds.center - new Vector3(0f,col.bounds.extents.y,0);
        GameObject spell1=Instantiate(MagicKnock,position,Quaternion.identity);

        spell1.GetComponent<Projectile>().Fire(0f,Mathf.Infinity,0f,Mathf.Floor(damageMultiplier*1.5f),enemiesLayer,int.MaxValue,knockBack);

        GameObject spell2 = Instantiate(MagicKnock, position, Quaternion.identity);

        spell2.transform.localScale = new Vector2(-spell2.transform.localScale.x, spell2.transform.localScale.y);

        spell2.GetComponent<Projectile>().Fire(0f, Mathf.Infinity, 0f, Mathf.Floor(damageMultiplier * 1.5f), enemiesLayer, int.MaxValue, new Vector2(-knockBack.x,knockBack.y));
        */
    }

    private IEnumerator wait()
    {
        Collider2D col = transform.root.GetComponent<Collider2D>();

        Vector2 position = col.bounds.center - new Vector3(0f, col.bounds.extents.y, 0);

        GameObject spell1 = Instantiate(MagicKnock, position, Quaternion.identity);
        GameObject spell2 = Instantiate(MagicKnock, position, Quaternion.identity);

        spell1.layer = transform.gameObject.layer;
        spell2.layer = transform.gameObject.layer;

        spell2.transform.localScale = new Vector2(-spell2.transform.localScale.x, spell2.transform.localScale.y);
        yield return new WaitForFixedUpdate();

        spell1.GetComponent<Projectile>().Fire(0f, Mathf.Infinity, 0f, Mathf.Floor(damageMultiplier * 1.5f), enemiesLayer, int.MaxValue, knockBack, true);
        spell2.GetComponent<Projectile>().Fire(0f, Mathf.Infinity, 0f, Mathf.Floor(damageMultiplier * 1.5f), enemiesLayer, int.MaxValue, new Vector2(-knockBack.x, knockBack.y), true);
    }

}
