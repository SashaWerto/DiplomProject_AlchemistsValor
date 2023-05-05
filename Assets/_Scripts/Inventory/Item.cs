using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemNull", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("Visual")]
    public Sprite icon;
    public string label;
    public string description;
    [Header("Visual/Mesh")]
    public Mesh mesh;
    public Material meshMaterial;
    [Header("Preferences")]
    public bool isStackable;
    public int maxStack = 64;
    [Header("Parameters")]
    public float maxDurability;
    [Header("Recipe")]
    public List<Recipe> recipe;
    public int craftCount;
    [Header("Types")]
    public ItemType itemType = ItemType.Resource;
}
[System.Serializable]
public class Recipe
{
    public Item item;
    [Range(1, 99)]
    public int count = 1;
}
