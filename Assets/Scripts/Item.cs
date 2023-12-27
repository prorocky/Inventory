using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    WEAPON,
    HELM,
    CHEST,
    BOOTS,
    CONSUMABLE,
    // ANY is specifically used for inventory slots, it will determine whether "any" item can be put into the slot
    ANY = 99
}

public enum ItemRarity {
    COMMON,
    UNCOMMON,
    RARE,
    EPIC,
    UNIQUE,
    LEGENDARY
}

[CreateAssetMenu(fileName ="New Item", menuName = "Item/Create")]
public class Item : ScriptableObject
{

    public ItemType type;
    public Sprite icon;
    public ItemRarity rarity;
    public int id;
    public string itemName;
    public int level;
    public int currentDurability;
    public int maxDurability;
    
}