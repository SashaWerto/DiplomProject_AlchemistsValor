using UnityEngine;
public class PlayerInput : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EquipmentManager _equipmentManager;
    [SerializeField] private CameraMover _camera;
    [SerializeField] private PlayerMovement _movement;
    [Header("References/UI")]
    [SerializeField] private UI_Element _inventoryElement;
    [Header("Key's Mapping")]
    [SerializeField] private KeyCode _toolChangeKey;
    [SerializeField] private KeyCode _InventoryKey;
    private float _inputX;
    private float _inputY;
    private bool _blockActions = false;
    private static PlayerInput _playerInput;
    public static PlayerInput Instance => _playerInput;
    private void Start()
    {
        _playerInput = this;
    }
    public float InputX { get => _inputX; set => _inputX = value; }
    public float InputY { get => _inputY; set => _inputY = value; }
    public bool BlockActions { get => _blockActions; set => _blockActions = value; }
    void Update()
    {
        _inputX = Input.GetAxisRaw("Horizontal");
        _inputY = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(_toolChangeKey))
        {
            if (_equipmentManager.WeaponHolder.gameObject.activeSelf)
                SwapToTool();
            else SwapToWeapon();
        }
        if(Input.GetKeyDown(_InventoryKey))
        {
            if (_inventoryElement.isActive)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
    }
    private void SwapToTool()
    {
        _equipmentManager.EnableTool();
    }
    private void SwapToWeapon()
    {
        _equipmentManager.EnableWeapon();
    }
    public void OpenInventory()
    {
        _inventoryElement.SetActive();
    }
    public void CloseInventory()
    {
        _inventoryElement.SetDisable();
        UI_Manager.Instance.CloseLastWindow();
    }
}
