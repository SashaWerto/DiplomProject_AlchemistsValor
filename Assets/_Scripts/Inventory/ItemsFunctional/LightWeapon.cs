using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightWeapon : Weapon_Functional
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
        if(Input.GetButtonUp("Fire1"))
        {
            _animator.SetBool("Attack", false);
        }
    }
}
