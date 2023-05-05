using UnityEngine;
[CreateAssetMenu(fileName = "BodyArmorNull", menuName = "Inventory/Armor/BodyArmor")]
public class Armor : Item
{
    public GameObject _armorPrefab;
    public Sprite _handsSprite;
    public float armorRating;
}
