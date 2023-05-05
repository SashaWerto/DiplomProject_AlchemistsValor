using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemsPackNull", menuName = "Inventory/ItemsPack")]
public class ItemsPack : ScriptableObject
{
    public List<ItemForPackHolder> _items;
}
[System.Serializable]
public class ItemForPackHolder
{
    public Item item;
    [Range(0, 99)]
    public int minCount = 1;
    [Range(1, 99)]
    public int maxCount = 1;
}
