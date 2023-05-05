using System.Collections.Generic;
using UnityEngine;
public class EquipmentManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterStats _characterStats;
    [SerializeField] private Inventory _inventory;
    [Header("References/Player GFX")]
    [SerializeField] private Transform _weaponHolder;
    [SerializeField] private Transform _toolHolder;
    [Header("Cells")]
    [SerializeField] private List<Cell> _equipableCells;
    private ItemFunctional _weaponFunctional;
    private ItemFunctional _toolFunctional;
    private GameObject _weaponPrefab;
    private GameObject _toolPrefab;
    private Backpack _equipedBackpack;
    private static EquipmentManager _equipmentManager;
    public static EquipmentManager Instance => _equipmentManager;
    public Transform WeaponHolder { get => _weaponHolder; set => _weaponHolder = value; }
    public Transform ToolHolder { get => _toolHolder; set => _toolHolder = value; }
    private void Start()
    {
        _equipmentManager = this;
        EnableWeapon();
    }
    public void RefreshCell(ItemType cellType)
    {
        foreach (Cell cell in _equipableCells)
        {
            if(cell.TypeOfSlot == cellType)
            {
                if (cell.ItemInCell)
                {
                    EquipItem(_equipableCells.IndexOf(cell));
                }
                else UnequipItem(cell);
            }
        }
    }
    public void EquipItem(int id)
    {
        var cellType = _equipableCells[id].TypeOfSlot;
        switch(cellType)
        {
            case ItemType.WeaponsGroup:
                if (_weaponPrefab)
                    UnequipItem(_equipableCells[id]);
                var weapon = _equipableCells[id].ItemInCell as Weapon;
                _weaponPrefab = Instantiate(weapon.weaponPrefab, _weaponHolder);
                _weaponFunctional = _weaponPrefab.GetComponent<ItemFunctional>();
                _characterStats.WeaponDamage = weapon.damage;
                _characterStats.StunEffect = weapon.stun;
                SetCharacterStatsForTool(_weaponFunctional, _equipableCells[id]);
                _characterStats.RefreshParameters();
                break;
            case ItemType.ToolsGroup:
                if (_toolPrefab)
                    UnequipItem(_equipableCells[id]);
                var tool = _equipableCells[id].ItemInCell as Tool;
                _toolPrefab = Instantiate(tool.toolPrefab, _toolHolder);
                _toolFunctional = _toolPrefab.GetComponent<ItemFunctional>();
                if (_equipableCells[id].Durability > 0 || !_toolFunctional.BrokenVariant)
                    _toolFunctional.SetFull();
                else
                    _toolFunctional.SetBroken();
                _characterStats.WeaponDamage = tool.damage;
                _characterStats.ToolGatherDamage = tool.gatherDamage;
                SetCharacterStatsForTool(_toolFunctional, _equipableCells[id]);
                _characterStats.RefreshParameters();
                break;
            case ItemType.ArmorBody:
                UnequipArmor();
                var armor = _equipableCells[id].ItemInCell as Armor;
                _characterStats.BodyArmor = armor.armorRating;
                _characterStats.RefreshParameters();
                break;
            case ItemType.Helmet:
                UnequipHelmet();
                var helmet = _equipableCells[id].ItemInCell as Helmet;
                _characterStats.HelmetArmor = helmet.armorRating;
                _characterStats.RefreshParameters();
                break;
            case ItemType.Backpack:
                if(!_equipedBackpack)
                {
                    UnequipBackpack();
                    _equipedBackpack = _equipableCells[id].ItemInCell as Backpack;
                    _inventory.AddCells(_equipedBackpack.cellsCount);
                }                                  
                break;
        }
        _equipableCells[id].Equiped = true;
    }
    private void SetCharacterStatsForTool(ItemFunctional itemFunctional, Cell cell)
    {
        itemFunctional.SetItem(_characterStats, cell);
    }
    public void EnableWeapon()
    {
        _weaponHolder.gameObject.SetActive(true);
        _toolHolder.gameObject.SetActive(false);
    }
    public void EnableTool()
    {
        _weaponHolder.gameObject.SetActive(false);
        _toolHolder.gameObject.SetActive(true);
    }
    public void UnequipItem(Cell cell)
    {
        cell.Equiped = false;
        var cellType = cell.TypeOfSlot;
        switch (cellType)
        {
            case ItemType.WeaponsGroup:
                UnequipWeapon();
                break;
            case ItemType.ArmorBody:
                UnequipArmor();
                break;
            case ItemType.ToolsGroup:
                UnequipTool();
                break;
            case ItemType.Helmet:
                UnequipHelmet();
                break;
            case ItemType.Backpack:
                UnequipBackpack();
                break;
        }
        _characterStats.RefreshParameters();
    }
    private void UnequipTool()
    {
        Destroy(_toolPrefab);
        _characterStats.ToolGatherDamage = 0;
        _toolFunctional = null;
    }
    private void UnequipWeapon()
    {
        Destroy(_weaponPrefab);
        _characterStats.WeaponDamage = 0;
        _characterStats.StunEffect = 0;
        _weaponFunctional = null;
    }
    private void UnequipArmor()
    {
        _characterStats.BodyArmor = 0;
        _characterStats.RefreshParameters();
    }
    private void UnequipBackpack()
    {
        if(_equipedBackpack) _inventory.RemoveCells();
        _equipedBackpack = null;
    }
    private void UnequipHelmet()
    {
        _characterStats.HelmetArmor = 0;
        _characterStats.RefreshParameters();
    }
}
