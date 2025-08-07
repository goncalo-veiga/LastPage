using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Animations;
using UnityEngine;

public class Bow : MonoBehaviour
{
    public float baseVelocity, damageMultiplier, destroyTimer, antiGravityTimer;
    public bool doPierced = false;
    private LayerMask enemiesLayer;
    private int chargingLvl;
    private Projectile[] arrows;

    [Header("Arrows")]
    [SerializeField] private GameObject Arrow;
    //[SerializeField] private AnimatorController FireArrowAnimator;
    public int MaxNrSpecialArrows;
    private int nrFireArrows = 0;

    private void Start()
    {
        enemiesLayer = (LayerMask)Variables.Object(transform.root.gameObject).Get("enemiesLayer");
    }

    public bool isCharging {  get; private set; }

    // Update is called once per frame
    public GameObject[] StartCharging(int nrArrows)
    {
        arrows=new Projectile[nrArrows];
        GameObject[] result = new GameObject[nrArrows];
        for (int i = 0; i < nrArrows; i++)
        {
            result[i] = Instantiate(Arrow);
            result[i].gameObject.layer = gameObject.layer;
            result[i].GetComponentInChildren<SpriteRenderer>().sortingLayerName = transform.GetComponentInChildren<SpriteRenderer>().sortingLayerName;
            result[i].GetComponentInChildren<SpriteRenderer>().sortingOrder = transform.GetComponentInChildren<SpriteRenderer>().sortingOrder + 2;
            arrows[i] = result[i].GetComponent<Projectile>();

            if (nrFireArrows > 0)
            {
                result[i].transform.GetChild(1).GetComponent<SpriteRenderer>().sortingLayerName = transform.GetComponentInChildren<SpriteRenderer>().sortingLayerName;
                result[i].transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder = transform.GetComponentInChildren<SpriteRenderer>().sortingOrder + 3;

                //result[i].GetComponent<Animator>().runtimeAnimatorController = FireArrowAnimator;
                result[i].AddComponent<FireWeapon>();

                nrFireArrows--;
            }
        }

        chargingLvl = 0;

        isCharging = true;

        return result;
    }
    public void LetItGo()
    {
        foreach (Projectile arrow in arrows)
        {

            arrow.Fire(baseVelocity*(1+(float)chargingLvl/2), antiGravityTimer * chargingLvl, destroyTimer, damageMultiplier, enemiesLayer, (doPierced) ? chargingLvl:0);
        }

        arrows = null;

        isCharging = false;
    }

    public void ChargeUp()
    {
        chargingLvl=Mathf.Min(chargingLvl+1, 2);
    }

    public void SetFireArrows()
    {
        nrFireArrows = MaxNrSpecialArrows;
    }

}
