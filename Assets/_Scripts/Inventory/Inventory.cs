using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Inventory : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Cell> _cells;
    [SerializeField] private Transform _inventoryPanel;
    [SerializeField] private Transform _itemDropPoint;
    [Header("Additional Cell's")]
    [SerializeField] private List<Cell> _additionalCells;
    [SerializeField] private Transform _cellsContent;
    [SerializeField] private GameObject _cellPrefab;
    private static Inventory _inventory;
    public List<Cell> Cells { get => _cells; set => _cells = value; }
    public Transform InventoryPanel { get => _inventoryPanel; set => _inventoryPanel = value; }
    public Transform ItemDropPoint { get => _itemDropPoint; set => _itemDropPoint = value; }
    public static Inventory Instance => _inventory;
    private void Start()
    {
        _inventory = this;
    }
    public void AddCells(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var cellObj = Instantiate(_cellPrefab, _cellsContent);
            if (cellObj.TryGetComponent<Cell>(out var cell))
            {
                _additionalCells.Add(cell);
                _cells.Add(cell);
            }
        }
    }
    public void RemoveCells()
    {
        if (_additionalCells.Count <= 0)
            return;
        List<GameObject> cellsObj = new List<GameObject>();
        foreach (Cell cell in _additionalCells)
        {
            if (cell.ItemInCell)
            {
                cell.DropItem();
            }
            cellsObj.Add(cell.gameObject);
            _cells.Remove(cell);           
        }
        foreach (GameObject cell in cellsObj)
        {
            Destroy(cell);
        }
        _additionalCells.Clear();
    }
    public void AddItem(ItemHolder itemHolder)
    {           
        if(itemHolder.Equiped)
        {
            foreach(var cell in _cells)
            {
                if(cell.TypeOfSlot.HasFlag(itemHolder.ItemInHolder.itemType) && !cell.ItemInCell)
                {
                    AddItemInCell(cell, itemHolder);
                    EquipmentManager.Instance.RefreshCell(cell.TypeOfSlot);
                    return;
                }
            }
        }
        foreach(var cell in _cells)
        {
            if(!cell.Blocked)
            {
                if (cell.ItemInCell && cell.ItemInCell.name == itemHolder.ItemInHolder.name && cell.Count < cell.ItemInCell.maxStack && itemHolder.ItemInHolder.isStackable)
                {
                    AddItemInStack(cell, itemHolder);
                    break;
                }
                else if (!cell.ItemInCell && cell.TypeOfSlot == 0)
                {
                    AddItemInCell(cell, itemHolder);
                    break;
                }
                else itemHolder.gameObject.transform.position = _itemDropPoint.position;
            }           
        }        
    }
    private void AddItemInStack(Cell cell, ItemHolder itemHolder)
    {
        int checkAllCount = itemHolder.Count + cell.Count;
        if (checkAllCount < cell.ItemInCell.maxStack)
        {
            cell.Count += itemHolder.Count;
            Destroy(itemHolder.gameObject);
            cell.RefreshCell();
            return;
        }
        if (cell.Count < cell.ItemInCell.maxStack)
        {
            for (int y = 0; y < checkAllCount; y++)
            {
                cell.Count ++;
                itemHolder.Count --;
                if (itemHolder.Count <= 0)
                {
                    Destroy(itemHolder.gameObject);
                    break;
                }
                if (cell.Count >= cell.ItemInCell.maxStack)
                {
                    AddItem(itemHolder);
                    break;
                }
            }
        }
        cell.RefreshCell();
    }
    private void AddItemInCell(Cell cell, ItemHolder itemHolder)
    {
        cell.ItemInCell = itemHolder.ItemInHolder;
        cell.Count = itemHolder.Count;
        cell.Durability = itemHolder.Durability;
        cell.RefreshCell();
        Destroy(itemHolder.gameObject);
    }
}