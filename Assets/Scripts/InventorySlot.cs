using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

// ----------------------------------------------------------------------------------------------------------------------------------------------------
// potentially create multiple types of comparer classes so that items can be sorted differently, simply create a new comparer and properly implement
// ----------------------------------------------------------------------------------------------------------------------------------------------------

public class ItemComparer : IComparer<InventorySlot>
{
    public int Compare(InventorySlot x, InventorySlot y)
    {
        if (x == null || y == null)
            throw new ArgumentNullException();
        
        // grab items to compare
        ItemHandler xitem = x.GetComponentInChildren<ItemHandler>();
        ItemHandler yitem = y.GetComponentInChildren<ItemHandler>();

        // if any are null i.e. empty slots, go at the end
        if (xitem == null)
            if (yitem == null)
                return 0; // If both items are null they are equal technically
            else
                return 1; // Null items (empty slots) go at the end
        if (yitem == null) return -1; // Null items (empty slots) go at the end

        // Note:    The reason I have the CompareTo is these if statements is because if two items have the same level,
        //          it should then move on to the next criteria for sorting rather than returning as "equal"

        // when ordering by type, "smaller" type (enum value) should appear first
        if (xitem._item.type != yitem._item.type) {
            return xitem._item.type.CompareTo(yitem._item.type);
        }

        // when ordering by level, rarity, or durability, multiply by -1 so that the higher level/rarity/durability item appears first

        // ----------------------------------------------------------------------------------------------------------------------------------------------------
        // NOTE: WHEN ITEM RARITY IS IMPLEMENTED, SORT BY RARITY HERE BEFORE LEVEL OR AFTER LEVEL
        // ----------------------------------------------------------------------------------------------------------------------------------------------------
        // if (xitem._item.rarity != yitem._item.rarity) {
        //     return xitem._item.rarity.CompareTo(yitem._item.rarity) * -1;
        // }

        if (xitem._item.level != yitem._item.level) {
            return xitem._item.level.CompareTo(yitem._item.level) * -1;
        }
        return xitem._item.currentDurability.CompareTo(yitem._item.currentDurability) * -1;

    }
}

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    // default/empty inventory slot is set to ANY, once an item occupies the slot, _type will reflect the type of item occupying the slot
    public ItemType _type = ItemType.ANY;
    InventoryManager invManager;   

    private void Start() {
        invManager = GameObject.FindObjectOfType<InventoryManager>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (InventoryManager.movingItem)
        {
            ItemHandler item = eventData.pointerDrag.GetComponent<ItemHandler>();

            if (_type == ItemType.ANY || item._item.type == _type) {
                // dragging onto empty slot
                // ADD A CONDITION SO THAT TRANSFORM MUST BE AN INVNENTORY SLOT

                if (transform.childCount == 0) {
                    item.parentAfterDrag = transform;
                }
                // dragging onto slot with item
                else
                {
                    InventorySlot from = item.parentAfterDrag.GetComponent<InventorySlot>();
                    InventorySlot to = eventData.pointerEnter.GetComponent<ItemHandler>().parentAfterDrag.GetComponent<InventorySlot>();

                    // had to add item as a parameter because when item becomes dragged, it gets unparented immediately
                    invManager.SwapItems(from, to, item);
                }
            }
        }
    }

    // variables to be used to determine a double or single click
    private float lastClickTime = 0f;
    private bool isDoubleClick = false;
    private float doubleClickThreshold = 0.25f;

    public void OnPointerClick(PointerEventData eventData)
    {
        InventorySlot itemSlot = eventData.pointerClick.GetComponent<InventorySlot>();
        ItemHandler item = itemSlot.GetComponentInChildren<ItemHandler>();
        if (item != null) 
        {
            item.turnOffItemInfo();
        }
        if (Time.time - lastClickTime < doubleClickThreshold)
        {
            isDoubleClick = true;
            if (itemSlot.transform.childCount > 0)
                invManager.EquipItem(itemSlot);
        }
        else
        {
            isDoubleClick = false;

        }
        lastClickTime = Time.time;
    }

    public void ItemInfoClick(InventorySlot itemSlot, ItemHandler item)
    {
        if (item != null) 
        {
            item.turnOffItemInfo();
            item.isHovering = false;
        }
        if (Time.time - lastClickTime < doubleClickThreshold)
        {
            isDoubleClick = true;
            item.isHovering = false;
            item.turnOffItemInfo();
            if (itemSlot.transform.childCount > 0)
                invManager.EquipItem(itemSlot);
        }
        else
        {
            isDoubleClick = false;

        }
        lastClickTime = Time.time;
    }
}
