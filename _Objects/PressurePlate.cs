using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : ButtonObjects
{
    private bool isPressured;
    [SerializeField] private LayerMask mask;
    [SerializeField] private bool layerToggle, projectileToggle;

    protected override void Update()
    {
        base.Update();

        if (isPressured != BeingPressured() && !onChange)
        {
            OnActivation();
            isPressured = !isPressured;
        }
    }

    private bool BeingPressured()
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position,col.bounds.extents*2,0f,Vector2.up, col.bounds.extents.y*2+0.1f);

        foreach(RaycastHit2D hit in hits)
        {
            bool isProjectile = hit.collider.GetComponent<Projectile>() != null;
            if (layerToggle)
            {
                bool isWeight = (mask.value & (1 << hit.collider.gameObject.layer)) > 0;
                if (hit.collider.GetComponent<Movables>() != null || isWeight && !isProjectile) return true;
            }

            if (projectileToggle)
            {
                if(isProjectile) return true;
            }
            
        }
        return false;
    }
}
