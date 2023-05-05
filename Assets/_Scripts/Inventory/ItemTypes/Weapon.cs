using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "WeaponNull", menuName = "Inventory/Weapon")]
public class Weapon : Item
{
    [Header("References")]
    public GameObject weaponPrefab;
    [Header("Preferences")]
    public float damage;
    public float staminaCost;
    public float stun;
    public float force;
}
