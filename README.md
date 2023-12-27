# Inventory
Prototype for an Inventory System for (now scrapped) game 

## How to Use
Click on Inventory button to open UI. To close, click anywhere on the dark overlay that is not the UI.
Basic features are listed below with more to come

## Features
### Drag and Drop
Allows for dragging and dropping of items within any open inventory slot.
Dragging and dropping an item to a slot with an item will swap their places

### Double Click to Equip
If you double click on an item in your inventory, it will automatically be equipped into the equipped items location.
If an item is already equipped, it will unequip and swap places with the item that was double clicked.

### Hover for Info
If you hover your mouse over items, a small overlay will appear displaying the icon, names, and stats of the item (for now name, level, and durability)

### Sort
Sort button will sort items in inventory. By default, the items are sorted by type (weapon, helmet, chest, boots, consumable), level, then durability.
There is a commented out section for when item rarity is implemented.
Additionally, it would be simple to choose how to sort (durability before level or level before type etc). Instructions are commented at the top of InventorySlot.cs
