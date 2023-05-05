using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EquipmentManager _equipmentManager;
    [SerializeField] private Health _health;
    [Header("Preferences")]
    private float _weaponDamage;
    private float _toolGatherDamage;
    private float _stunEffect;
    private float _helmetArmor;
    private float _bodyArmor;
    private float _allArmor;
    private float _allDamage;
    public float Health { get => _health.CurrentHealth; set => _health.CurrentHealth = value; } 
    public float WeaponDamage { get => _weaponDamage; set => _weaponDamage = value; } 
    public float ToolGatherDamage { get => _toolGatherDamage; set => _toolGatherDamage = value; } 
    public float StunEffect { get => _stunEffect; set => _stunEffect = value; } 
    public float HelmetArmor { get => _helmetArmor; set => _helmetArmor = value; } 
    public float BodyArmor { get => _bodyArmor; set => _bodyArmor = value; } 
    public float AllArmor { get => _allArmor; set => _allArmor = value; } 
    public float AllDamage { get => _allDamage; set => _allDamage = value; } 
    private void Start()
    {
        RefreshParameters();
    }
    public void GetDamage(float damage)
    {

    }
    public void RefreshParameters()
    {
        _allArmor = _bodyArmor + _helmetArmor;
        _allDamage = _weaponDamage;
    }
}
