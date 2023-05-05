using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _lootExample;
    public static LootManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    /// <summary>
    /// Метод DropItem() создает объект предмета на указанной позиции,
    /// на который может быть наложена сила при появлении чтобы сдвинуть объект
    /// с точки появления через bool applyForce.
    /// </summary>
    public void DropItem(Vector3 pos, Item item, int count = 1, bool applyForce = false, bool isDirty = false, float durability = 0f, bool addInInventory = false)
    {       
        while(count > 0)
        {
            var itemHolderObj = Instantiate(_lootExample, pos, Quaternion.identity);
            itemHolderObj.TryGetComponent<ItemHolder>(out var itemHolder);
            itemHolder.ItemInHolder = item;
            itemHolder.isDirty = isDirty;
            if(durability != 0)
                itemHolder.Durability = durability;
            if(count > item.maxStack)
            {
                count -= item.maxStack;
                itemHolder.Count = item.maxStack;
            }
            else
            {
                itemHolder.Count = count;
                count = 0;
            }
            if (applyForce)
            {
                if (itemHolderObj.TryGetComponent<Rigidbody2D>(out var rigidbody))
                    rigidbody.AddForce(new Vector2(Random.Range(-1, 1), Random.Range(-1, 1) * 8f), ForceMode2D.Impulse);
            }
            if(!isDirty)
            {
                itemHolder.Durability = item.maxDurability;
            }
            if (addInInventory)
                Inventory.Instance.AddItem(itemHolder);
        }        
    }
    public void AddOutline()
    {

    }
}
