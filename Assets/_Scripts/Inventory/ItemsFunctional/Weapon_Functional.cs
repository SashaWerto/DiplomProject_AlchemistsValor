using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Functional : ItemFunctional
{
    public override void Attack()
    {
        base.Attack();
        foreach (RaycastHit hit in _hitsRaycast)
        {
            if (hit.collider.TryGetComponent<IDamageable>(out var enemy))
            {
                enemy.GetDamage(_stats.AllDamage, _stats.StunEffect);
                _hitCollider = hit.collider;
            }
        }
    }
}
