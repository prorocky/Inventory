using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemInfo : MonoBehaviour, IPointerClickHandler
{
    // A list of all texts/images that need to be displayed
    private Image _icon;
    private TextMeshProUGUI _itemName, _levelValue, _currentDurability, _maxDurability;

    // Start is called before the first frame update
    void Start()
    {
        _icon = transform.Find("ItemIcon").gameObject.GetComponent<Image>();
        _itemName = transform.Find("ItemName").gameObject.GetComponent<TextMeshProUGUI>();
        _levelValue = transform.Find("LevelValue").gameObject.GetComponent<TextMeshProUGUI>();
        _currentDurability = transform.Find("CurrentDurability").gameObject.GetComponent<TextMeshProUGUI>();
        _maxDurability = transform.Find("MaxDurability").gameObject.GetComponent<TextMeshProUGUI>();

        _icon.sprite = GetComponentInParent<ItemHandler>()._item.icon;
        _itemName.text = GetComponentInParent<ItemHandler>()._item.itemName;
        _levelValue.text = GetComponentInParent<ItemHandler>()._item.level.ToString();
        _currentDurability.text = GetComponentInParent<ItemHandler>()._item.currentDurability.ToString();
        _maxDurability.text = GetComponentInParent<ItemHandler>()._item.maxDurability.ToString();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        ItemHandler item = eventData.pointerClick.GetComponentInParent<ItemHandler>();
        InventorySlot slot = item.parentAfterDrag.GetComponent<InventorySlot>();

        slot.ItemInfoClick(slot, item);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
