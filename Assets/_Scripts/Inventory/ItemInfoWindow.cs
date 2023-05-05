using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoWindow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _infoWindow;
    [SerializeField] private TextMeshProUGUI _label;
    [SerializeField] private TextMeshProUGUI _description;
    public void OpenItemInfo(Item item)
    {
        /*
        switch(item.itemType.HasFlag(ItemType.ToolsGroup))
        {
            case 0:
                break;
        }
        */
    }
}
