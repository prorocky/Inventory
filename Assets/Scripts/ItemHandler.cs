using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;


public class ItemHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    public Image _image;

    public Item _item;
    public Transform parentAfterDrag;

    // Variables for displaying ItemInfo
    public GameObject _itemInfoPanel;
    public bool isHovering;
    private float hoverTime = 0.75f;
    private float timer;


    private void Start() {
        _image = GetComponent<Image>();
        InitializeItem(_item);
        _itemInfoPanel = transform.Find("ItemInfo").gameObject;
    }

    public void InitializeItem(Item item) {
        _item = item;
        _image.sprite = item.icon;
        parentAfterDrag = transform.parent;
    }


    // Drag and Drop
    public void OnBeginDrag(PointerEventData eventData)
    {
        turnOffItemInfo();
        _image.raycastTarget = false;
        InventoryManager.movingItem = true;
        
        if (transform.parent.name != "Canvas")
            parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // check if dropped on inventory slot

        _image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        InventoryManager.movingItem = false;
    }

    private void Update()
    {
        if (isHovering && !InventoryManager.movingItem)
        {
            timer += Time.deltaTime;

            if (timer >= hoverTime)
            {
                // turn on ItemInfo after hovering for hoverTime
                if (!_itemInfoPanel.activeSelf)
                {
                    turnOnItemInfo();
                }
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        timer = 0f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        timer = 0f;
        turnOffItemInfo();
    }

    public void toggleItemInfo() {
        _itemInfoPanel.SetActive(!_itemInfoPanel.activeSelf);
        if (_itemInfoPanel.activeSelf)
            transform.SetParent(transform.root);
        else
            transform.SetParent(parentAfterDrag);
    }

    public void turnOffItemInfo() {
        if (_itemInfoPanel.activeSelf)
        {
            transform.SetParent(parentAfterDrag);
            _itemInfoPanel.SetActive(false);
        }
    }

    public void turnOnItemInfo() {
        if (!_itemInfoPanel.activeSelf)
        {
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            _itemInfoPanel.SetActive(true);
        }
    }

}
