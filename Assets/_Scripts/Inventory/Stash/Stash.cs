using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stash : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ItemsPack _itemsPack;
    [SerializeField] private GameObject _stashUiPrefab;
    [Header("Preferences")]
    [SerializeField] private string _stashName;
    private GameObject _stashUiObj;
    private bool _initiated;
    private bool _isOpened;
    public void Initiate()
    {
        _stashUiObj = Instantiate(_stashUiPrefab, UI_Manager.Instance.StashHolderUi);
        if(_stashUiObj.TryGetComponent<StashCellsManager>(out var stashManager))
        {
            stashManager.AttachedStash = this;
            for (int i = 0; i < _itemsPack._items.Count; i++)
            {
                GameObject itemHolderObj = new GameObject("=ItemHolder/Stash=");
                var itemHolder = itemHolderObj.AddComponent<ItemHolder>();
                int randomCount = Random.Range(_itemsPack._items[i].minCount, _itemsPack._items[i].maxCount);
                if(randomCount > _itemsPack._items[i].item.maxStack)
                {
                    randomCount = _itemsPack._items[i].item.maxStack;
                }
                if(randomCount == 0)
                {
                    Destroy(itemHolderObj);
                    return;
                }
                itemHolder.ItemInHolder = _itemsPack._items[i].item;
                itemHolder.Count = randomCount;
                itemHolder.Durability = _itemsPack._items[i].item.maxDurability;
                stashManager.AddItem(itemHolder);
            }
        }
    }
    public void OnInteract()
    {
        _isOpened = !_isOpened;
        if(!_initiated)
        {
            Initiate();
            _initiated = true;
        }
        if (_isOpened)
            Close();
        {
            Open();
        }      
    }
    public void Open()
    {
        _stashUiObj.SetActive(true);
        UI_Manager.Instance.LastWindow = _stashUiObj;
        PlayerInput.Instance.OpenInventory();
    }
    public void Close()
    {
        _stashUiObj.SetActive(false);      
    }
}
