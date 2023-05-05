using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ToolNull", menuName = "Inventory/Tool")]
public class Tool : Item
{
    [Header("References")]
    public GameObject toolPrefab;
    [Header("Preferences")]
    public float staminaCost;
    public float stun;
    public float gatherDamage;
    public float damage;
}
