using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingWindow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image _itemIcon;
    [SerializeField] private RectTransform _recipesContent;
    [SerializeField] private RectTransform _itemsContent;
    [SerializeField] private GameObject _recipePrefab;
    [SerializeField] private Button _craftButton;
    [SerializeField] private Text _labelText;
    [Header("References/ResourceBoard")]
    [SerializeField] private GameObject _resourceBoard;
    [SerializeField] private Text _resourceDescription;
    [Header("References/ToolBoard")]
    [SerializeField] private GameObject _toolBoard;
    [SerializeField] private Text _toolDamageText;
    [SerializeField] private Text _toolStaminaText;
    [SerializeField] private Text _toolForceText;
    [SerializeField] private Text _toolDurabilityText;
    [Header("References/WeaponBoard")]
    [SerializeField] private GameObject _weaponBoard;
    [SerializeField] private Text _weaponDamageText;
    [SerializeField] private Text _weaponStaminaText;
    [SerializeField] private Text _weaponStunText;
    [SerializeField] private Text _weaponDurability;
    private List<GameObject> _createdCells = new List<GameObject>();
    private List<GameObject> _createdRequiredItems = new List<GameObject>();
    private void Start()
    {
        RefreshRecipeList();
    }

    public void RefreshRecipeList()
    {
        foreach (GameObject cell in _createdCells)
        {
            Destroy(cell);
        }
        _createdCells.Clear();       
        for (int i = 0; i < Crafting.Instance.OpenedRecipes.Count; i++)
        {
            Item item = Crafting.Instance.OpenedRecipes[i];
            GameObject recipeCell = Instantiate(_recipePrefab, _recipesContent);
            recipeCell.TryGetComponent<Recipe_Cell>(out var recipe);
            recipe.Icon.sprite = item.icon;
            recipe.CountText.text = $"{item.craftCount}";
            recipe.ButtonCell.onClick.AddListener(() => { var param = item; ShowRecipe(param); });
            _createdCells.Add(recipeCell);
        }
    }
    public void ShowRecipe(Item item)
    {
        foreach (GameObject cell in _createdRequiredItems)
        {
            Destroy(cell);
        }
        _createdRequiredItems.Clear();
        for (int i = 0; i < item.recipe.Count; i++)
        {
            GameObject requiredItem = Instantiate(_recipePrefab, _itemsContent);
            requiredItem.TryGetComponent<Recipe_Cell>(out var recipe);
            recipe.ButtonCell.enabled = false;
            recipe.Icon.sprite = item.recipe[i].item.icon;
            recipe.CountText.text = $"{item.recipe[i].count}";
            _createdRequiredItems.Add(requiredItem);
        }
        _itemIcon.sprite = item.icon;
        _labelText.text = item.label;
        switch (item.itemType)
        {
            case ItemType.Resource:
                EnableResourceDescription(item);
                break;
            case var t when ItemType.ToolsGroup.HasFlag(t):
                EnableToolDescription(item);
                break;
            case var t when ItemType.WeaponsGroup.HasFlag(t):
                EnableWeaponDescription(item);
                break;
        }
        _craftButton.onClick.RemoveAllListeners();
        _craftButton.onClick.AddListener(() => { var param = item; Crafting.Instance.TryCraft(param); });
    }
    public void EnableResourceDescription(Item item)
    {
        _resourceBoard.SetActive(true);
        _toolBoard.SetActive(false);
        _weaponBoard.SetActive(false);
        _resourceDescription.text = item.description;
    }
    public void EnableToolDescription(Item item)
    {
        _resourceBoard.SetActive(false);
        _toolBoard.SetActive(true);
        _weaponBoard.SetActive(false);
        Tool tool = item as Tool;
        _toolDamageText.text = $"Урон - {tool.damage}";
        _toolStaminaText.text = $"Добыча - {tool.gatherDamage}";
        _toolForceText.text = $"Сила - {tool.staminaCost}";
        _toolDurabilityText.text = $"Прочн. - {tool.maxDurability}";
    }
    public void EnableWeaponDescription(Item item)
    {
        _resourceBoard.SetActive(false);
        _toolBoard.SetActive(false);
        _weaponBoard.SetActive(true);
        Weapon weapon = item as Weapon;
        _weaponDamageText.text = $"Урон - {weapon.damage}";
        _weaponStaminaText.text = $"Затрат сил - {weapon.staminaCost}";
        _weaponStunText.text = $"Стан - {weapon.stun}";
        _weaponDurability.text = $"Прочн. - {weapon.maxDurability}";
    }
}
