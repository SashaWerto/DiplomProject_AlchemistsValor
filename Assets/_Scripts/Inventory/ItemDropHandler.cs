using UnityEngine;
using UnityEngine.EventSystems;
public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    [Header("References")]
    [SerializeField] private Cell _cell;
    public Cell RefCell { get => _cell; set => _cell = value; }
    public void OnDrop(PointerEventData eventData)
    {
        RectTransform cellRectTransform = eventData.pointerDrag.transform as RectTransform;
        if (RectTransformUtility.RectangleContainsScreenPoint(cellRectTransform, Input.mousePosition) && cellRectTransform.TryGetComponent<ItemDropHandler>(out var item))
        {
            /*
            if (item.RefCell.ItemInCell && _cell.ItemInCell.name == item.RefCell.ItemInCell.name)
            {
                for (int i = 0; i < item.RefCell.Count; i++)
                {
                    if (_cell.Count >= _cell.ItemInCell.maxStack)
                    {
                        break;
                    }
                    if(item.RefCell.Count <= 0)
                    {
                        break;
                    }
                    _cell.Count += 1;
                    item.RefCell.Count -= 1;
                }
                return;
            }
            */
            if (_cell.TypeOfSlot == 0)
            {
                if(item.RefCell.ItemInCell && _cell.ItemInCell)
                {
                    if(_cell.TypeOfSlot.HasFlag(item.RefCell.TypeOfSlot))
                    { 
                        TransferItemInCell(item);
                    }
                }
                else if(item.RefCell.ItemInCell && !_cell.ItemInCell)
                {
                    TransferItemInCell(item);
                }
                return;
            }
            if (_cell.TypeOfSlot.HasFlag(item.RefCell.ItemInCell.itemType))
            {
                TransferItemInCell(item);
                return;
            }
        }
        return;
    }
    public void TransferItemInCell(ItemDropHandler item)
    {
        Item transferItem = item.RefCell.ItemInCell;
        int count = item.RefCell.Count;
        float durability = item.RefCell.Durability;

        item.RefCell.ItemInCell = _cell.ItemInCell;
        item.RefCell.Count = _cell.Count;
        item.RefCell.Durability = _cell.Durability;
        item.RefCell.RefreshCell();
        PutItemInCell(transferItem, count, durability);
        EquipmentManager.Instance.RefreshCell(item.RefCell.TypeOfSlot);
    }
    public void PutItemInCell(Item transferItem, int count, float durability)
    {
        _cell.ItemInCell = transferItem;
        _cell.Count = count;
        _cell.Durability = durability;
        _cell.RefreshCell();
        EquipmentManager.Instance.RefreshCell(_cell.TypeOfSlot);
    }
}
