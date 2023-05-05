using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    [Header("References")]
    [SerializeField] private RectTransform _cellRectTransform;
    [SerializeField] private Image _ItemImage;
    [SerializeField] private ItemDropHandler _itemDropHandler;
    [SerializeField] private GameObject _dropIcon;
    private Canvas _canvas;
    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvas = gameObject.AddComponent<Canvas>();
        _canvas.overrideSorting = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        if (eventData.hovered.Count <= 0 && _itemDropHandler.RefCell.ItemInCell)
            _dropIcon.SetActive(true);
        else _dropIcon.SetActive(false);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(_canvas);
        _dropIcon.SetActive(false);
        transform.localPosition = Vector3.zero;
        if (eventData.hovered.Count <= 0 && _itemDropHandler.RefCell.ItemInCell)
        {
            _itemDropHandler.RefCell.DropItem();
            return;
        }        
    }
}
