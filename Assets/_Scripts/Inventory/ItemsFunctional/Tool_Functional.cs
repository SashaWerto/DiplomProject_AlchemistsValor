using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_Functional : ItemFunctional
{
    public override void Update()
    {
        if (PlayerInput.Instance.BlockActions)
        {
            _animator.SetBool("Attack", false);
            return;
        }
        base.Update();
        if (Broken)
            return;
        if (Input.GetButtonDown("Fire1"))
        {
            _animator.SetBool("Attack", true);
        }
        if (Input.GetButtonUp("Fire1"))
        {
            _animator.SetBool("Attack", false);
        }
    }
    public override void Attack()
    {
        base.Attack();     
        foreach (RaycastHit hit in _hitsRaycast)
        {        
            if (hit.collider.TryGetComponent<IDamageable>(out var enemy))
            {
                enemy.GetDamage(_stats.AllDamage);
            }
            if (hit.collider.TryGetComponent<IGatherable>(out var resourceObject))
            {
                hit.collider.TryGetComponent<ResourceGatherable>(out var resource);
                if(resource.TypeOfItemWhatCanDamage.HasFlag(EquipedCell.ItemInCell.itemType))
                {
                    resourceObject.GetGatherDamage(_stats.ToolGatherDamage);
                    _hitCollider = hit.collider;
                }            
            }
        }
    }
}
