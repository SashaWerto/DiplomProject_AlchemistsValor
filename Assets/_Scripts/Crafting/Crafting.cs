using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Item> _openedItems = new List<Item>();
    [SerializeField] private Transform _dropPoint;
    private static Crafting _crafting;
    public static Crafting Instance => _crafting;
    public List<Item> OpenedRecipes { get => _openedItems; set => _openedItems = value; }
    private void Awake()
    {
        _crafting = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TryCraft(_openedItems[0]);
        }
    }
    public void TryCraft(Item item)
    {
        var recipe = item.recipe;
        List<ItemForCraftHolder> allFoundedItems = new List<ItemForCraftHolder>();
        List<Cell> findedCells = new List<Cell>();
        int requiredItemsCount = recipe.Count;
        for (int i = 0; i < recipe.Count; i++)
        {
            bool itemFinded = false;
            foreach (Cell cell in Inventory.Instance.Cells)
            {
                if (cell.ItemInCell && recipe[i].item.name == cell.ItemInCell.name)
                {
                    foreach (ItemForCraftHolder itemForCraft in allFoundedItems)
                    {
                        if (cell.ItemInCell.name == itemForCraft.item.name)
                        {
                            itemForCraft.count += cell.Count;
                            itemFinded = true;
                            findedCells.Add(cell);
                        }
                    }
                    if (!itemFinded)
                    {
                        var itemHolder = new ItemForCraftHolder();
                        itemHolder.item = cell.ItemInCell;
                        itemHolder.count = cell.Count;
                        allFoundedItems.Add(itemHolder);
                        findedCells.Add(cell);
                    }
                }
            }
        }
        for (int i = 0; i < recipe.Count; i++)
        {
            foreach (ItemForCraftHolder itemForCraft in allFoundedItems)
            {
                if (recipe[i].item.name == itemForCraft.item.name && itemForCraft.count >= recipe[i].count)
                {
                    requiredItemsCount -= 1;
                    itemForCraft.count -= recipe[i].count;
                }
            }
        }
        if (requiredItemsCount <= 0)
        {
            Craft(item);
            SubstractItems(findedCells, allFoundedItems);
        }
    }
    public void SubstractItems(List<Cell> cells, List<ItemForCraftHolder> craftHolders)
    {
        foreach(Cell cell in cells)
        {
            cell.SetEmpty();
        }
        foreach (ItemForCraftHolder itemForCraft in craftHolders)
        {
            LootManager.Instance.DropItem(_dropPoint.position, itemForCraft.item, itemForCraft.count, false, false, 0, true);
        }
    }
    public void Craft(Item item)
    {
        LootManager.Instance.DropItem(_dropPoint.position, item, item.craftCount, false, false, 0, true);
    }
    public List<ItemForCraftHolder> GetRecipe(Item item)
    {
        var recipe = item.recipe;
        List<ItemForCraftHolder> allFoundedItems = new List<ItemForCraftHolder>();
        for (int i = 0; i < recipe.Count; i++)
        {
            bool itemFinded = false;
            foreach (Cell cell in Inventory.Instance.Cells)
            {
                if (cell.ItemInCell && recipe[i].item.name == cell.ItemInCell.name)
                {
                    foreach (ItemForCraftHolder itemForCraft in allFoundedItems)
                    {
                        if (cell.ItemInCell.name == itemForCraft.item.name)
                        {
                            itemForCraft.count += cell.Count;
                            itemFinded = true;
                        }
                    }
                    if (!itemFinded)
                    {
                        var itemHolder = new ItemForCraftHolder();
                        itemHolder.item = cell.ItemInCell;
                        itemHolder.count = cell.Count;
                        allFoundedItems.Add(itemHolder);
                    }
                }
            }
        }
        return allFoundedItems;
    }
}
public class ItemForCraftHolder
{
    public Cell cell;
    public Item item;
    public int count;
}