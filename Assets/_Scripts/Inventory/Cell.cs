using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Cell : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _textCount;
    [SerializeField] private Image _durabilityBar;
    [SerializeField] private GameObject _iconBG;
    [SerializeField] private GameObject _destroyedIcon;
    [Header("Slot Preferences")]
    [SerializeField] private ItemType _slotType = 0;
    [SerializeField] private Item _item;
    private bool _blocked;
    private bool _equiped;
    private int _count;
    private float _durability;
    public Image IconImage { get => _iconImage; set => _iconImage = value; }
    public Item ItemInCell { get => _item; set => _item = value; }
    public int Count { get => _count; set => _count = value; }
    public float Durability { get => _durability; set => _durability = value; }
    public bool Blocked { get => _blocked; set => _blocked = value; }
    public bool Equiped { get => _equiped; set => _equiped = value; }
    public ItemType TypeOfSlot { get => _slotType; set => _slotType = value; }
    private void Start()
    {
        RefreshCell();
    }
    public void RefreshCell()
    {
        if (_item && Count <= 0)
        {
            bool isEquipment = false;
            if (_item.maxDurability > 0)
                isEquipment = true;
            _item = null;
            if (isEquipment)
            {
                EquipmentManager.Instance.RefreshCell(TypeOfSlot);
            }
        }
        if (_item)
        {
            _iconImage.sprite = _item.icon;
            _iconImage.color = new Color(_iconImage.color.r, _iconImage.color.g, _iconImage.color.b, 255);
            _textCount.gameObject.SetActive(true);
            _textCount.text = $"{_count}/{_item.maxStack}";
            if(_iconBG)
            {
                _iconBG.SetActive(false);
            }
            if (_item.isStackable)
            {
                _textCount.gameObject.SetActive(true);
            }
            else
            {
                _textCount.gameObject.SetActive(false);
            }
            if (_item.maxDurability > 0)
            {
                _durabilityBar.transform.parent.gameObject.SetActive(true);
                RefreshDurabilityBar();
            }
            else
            {
                _durabilityBar.transform.parent.gameObject.SetActive(false);
                _destroyedIcon.SetActive(false);
            }
        }
        else
        {
            _iconImage.sprite = null;
            _iconImage.color = new Color(_iconImage.color.r, _iconImage.color.g, _iconImage.color.b, 0);
            _textCount.gameObject.SetActive(false);
            _destroyedIcon.SetActive(false);
            _durabilityBar.transform.parent.gameObject.SetActive(false);
            if (_iconBG)
            {
                _iconBG.SetActive(true);
            }
        }
    }
    public void RefreshDurabilityBar()
    {
        _durabilityBar.fillAmount = _durability / _item.maxDurability;
        if (_durability <= 0)
        {
            _destroyedIcon.SetActive(true);
            EquipmentManager.Instance.RefreshCell(TypeOfSlot);
        }
        else
        {
            _destroyedIcon.SetActive(false);
        }
    }
    public void DropItem()
    {
        if (_item)
        {
            LootManager.Instance.DropItem(Inventory.Instance.ItemDropPoint.position, _item, _count, false, true, _durability);
            _count = 0;
            _item = null;
            RefreshCell();
            EquipmentManager.Instance.RefreshCell(TypeOfSlot);
        }
    }
    public void SetEmpty()
    {
        _item = null;
        _count = 0;
        RefreshCell();
        EquipmentManager.Instance.RefreshCell(TypeOfSlot);
    }
}
