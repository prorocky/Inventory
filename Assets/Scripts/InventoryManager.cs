using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    // boolean to determine whether an item is being dragged/drop
    public static bool movingItem = false;
    // for now, temporarily an array of 36 items will be the inventory
    public InventorySlot[] invSlots;
    public GameObject inventoryItemPrefab;

    // Special references of Inventory Slots designated for equipped items
    [SerializeField] InventorySlot helmet, chest, boots, main, second;

    public void AddItem(Item item)
    {
        InventorySlot slot = FirstAvailableSlot();
        if (slot != null) 
        {
            SpawnNewItem(item, slot);
            return;
        }
        // for (int i = 0; i < invSlots.Length; i++) {
        //     InventorySlot slot = invSlots[i];
        //     ItemHandler itemInSlot = slot.GetComponentInChildren<ItemHandler>();
        //     if (itemInSlot == null) 
        //     {
        //         SpawnNewItem(item, slot);
        //         return;
        //     }
        // }
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newObj = Instantiate(inventoryItemPrefab, slot.transform);
        ItemHandler invItem = newObj.GetComponent<ItemHandler>();
        invItem.InitializeItem(item);
    }

    public void SortInventory()
    {
        var customSorter = new ItemComparer();
        // if desired, customSorter can be a different comparer if we wish to add sort by custom criteria
        // ex: sort by level, sort by name, sort by rarity, etc
        // implementation in InventorySlot.cs
        var sortedArr = invSlots.OrderBy(itm => itm, customSorter);

        for (int i = 0; i < sortedArr.Count(); i++) {
            SwapItems(invSlots[i], sortedArr.ElementAt(i), invSlots[i].GetComponentInChildren<ItemHandler>());
        }
    }

    // Given 2 InventorySlots and the item from the first slot, swap the items from the slots
    public void SwapItems(InventorySlot from, InventorySlot to, ItemHandler itemBeingDragged)
    {
        // print("swap is called");
        ItemHandler item = itemBeingDragged;
        ItemHandler itemToSwap = to.GetComponentInChildren<ItemHandler>();

        // check for "swapping" empty items
        if (item == null)
        {
            if (itemToSwap == null)
                return;
            itemToSwap.parentAfterDrag = from.transform;
            itemToSwap.transform.SetParent(from.transform);
            return;
        }
        if (itemToSwap == null)
        {
            item.parentAfterDrag = to.transform;
            item.transform.SetParent(to.transform);
            return;
        }

        itemToSwap.parentAfterDrag = from.transform;
        itemToSwap.transform.SetParent(from.transform);
        item.parentAfterDrag = to.transform;
        item.transform.SetParent(to.transform);
    }
    // this function iterates through the inventory slots and returns the available inventory slot or null if inventory is full
    public InventorySlot FirstAvailableSlot()
    {
        for (int i = 0; i < invSlots.Length; i++) {
            if (invSlots[i].transform.childCount == 0)
                return invSlots[i];
        }
        return null;
    }

    public void EquipItem(InventorySlot itemToEquip)
    {
        ItemHandler item = itemToEquip.GetComponentInChildren<ItemHandler>();

        if (itemToEquip._type != ItemType.ANY) {
            InventorySlot available = FirstAvailableSlot();
            if (available == null) {
                // show error message "Inventory is full"
                // play noise/animation/etc
                
            }
            SwapItems(itemToEquip, available, item);
            return;
            
        }

        switch (item._item.type)
        {
            case ItemType.WEAPON:
                // check if both slots are taken
                // if one is empty, occupy that slot
                if (main.transform.childCount == 0)
                {
                    item.parentAfterDrag = main.transform;
                    item.transform.SetParent(main.transform);
                    break;
                }
                if (second.transform.childCount == 0)
                {
                    item.parentAfterDrag = second.transform;
                    item.transform.SetParent(second.transform);
                    break;
                }
                // if none are empty, which one should be swapped?
                // for now nothing happens
                break;
            case ItemType.HELM:
                if (helmet.transform.childCount == 0)
                {
                    item.parentAfterDrag = helmet.transform;
                    item.transform.SetParent(helmet.transform);
                }
                else
                {
                    SwapItems(itemToEquip, helmet, item);
                }
                break;
            case ItemType.CHEST:
                if (chest.transform.childCount == 0)
                {
                    item.parentAfterDrag = chest.transform;
                    item.transform.SetParent(chest.transform);
                }
                else
                {
                    SwapItems(itemToEquip, chest, item);
                }
                break;
            case ItemType.BOOTS:
                if (boots.transform.childCount == 0)
                {
                    item.parentAfterDrag = boots.transform;
                    item.transform.SetParent(boots.transform);
                }
                else
                {
                    SwapItems(itemToEquip, boots, item);
                }
                break;
            default:
                break;
            
        }
    }
}
