using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StashCellsManager : MonoBehaviour
{
    [SerializeField] private List<Cell> _cells;
    private Stash _stash;
    public Stash AttachedStash { get => _stash; set => _stash = value; }

    public void AddItem(ItemHolder itemHolder)
    {
        foreach (var cell in _cells)
        {
            if (!cell.Blocked)
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
                cell.Count++;
                itemHolder.Count--;
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
