using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InventoryData : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveInventoryData();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadInventoryData();
        }
    }
    public void SaveInventoryData()
    {
        
        List<Item> listOfItems = new List<Item>();
        List<int> listOfCount = new List<int>();
        List<float> listOfDurability = new List<float>();
        List<bool> listOfEquiped = new List<bool>();
        foreach(Cell cell in _inventory.Cells)
        {
            if (cell.ItemInCell)
            {
                listOfItems.Add(cell.ItemInCell);
                listOfCount.Add(cell.Count);
                listOfDurability.Add(cell.Durability);
                listOfEquiped.Add(cell.Equiped);
            }              
        }
        PlayerPrefsExtra.SetList("ItemsInInventory", listOfItems);
        PlayerPrefsExtra.SetList("CountOfStack", listOfCount);
        PlayerPrefsExtra.SetList("DurabilityOfItem", listOfDurability);
        PlayerPrefsExtra.SetList("EquipedItems", listOfEquiped);
        PlayerPrefs.Save();
        
    }
    public void LoadInventoryData()
    {
        
        List<Item> loadedList = PlayerPrefsExtra.GetList("ItemsInInventory", new List<Item>());
        List<int> listOfCount = PlayerPrefsExtra.GetList("CountOfStack", new List<int>());
        List<float> listOfDurability = PlayerPrefsExtra.GetList("DurabilityOfItem", new List<float>());
        List<bool> listOfEquiped = PlayerPrefsExtra.GetList("EquipedItems", new List<bool>());
        for (int i = 0; i < loadedList.Count; i++)
        {
            var itemHolder = new GameObject().AddComponent<ItemHolder>();
            itemHolder.ItemInHolder = loadedList[i];
            itemHolder.Count = listOfCount[i];
            itemHolder.Durability = listOfDurability[i];
            itemHolder.Equiped = listOfEquiped[i];
            _inventory.AddItem(itemHolder);
            Destroy(itemHolder);
        }
        
    }
}
