[System.Flags]
public enum ItemType
{
    LightWeapon = 1 << 0,
    HeavyWeapon = 1 << 1,
    ArmorBody = 1 << 2,
    Helmet = 1 << 3,
    Resource = 1 << 4,
    Potion = 1 << 5,
    Food = 1 << 6,
    Pickaxe = 1 << 7,
    Axe = 1 << 8,
    Backpack = 1 << 9,
    ToolsGroup = Pickaxe | Axe,
    WeaponsGroup = LightWeapon | HeavyWeapon,
    ArmorGroup = ArmorBody | Helmet,
}
