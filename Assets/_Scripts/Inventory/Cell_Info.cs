using UnityEngine.EventSystems;
using UnityEngine;

public class Cell_Info : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private Cell _cell;
    [SerializeField] private ItemInfoWindow _infoWindow;
    private void Start()
    {
        _infoWindow = FindObjectOfType<ItemInfoWindow>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {       
    }
}
