using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Pure C# Model class for Inventory data management
/// </summary>
public class InventoryModel
{
    private List<ItemData> _itemIndex;

    // References to the view's serialized fields
    private ItemSlotData[] toolSlots;
    private ItemSlotData equippedToolSlot;
    private ItemSlotData[] itemSlots;
    private ItemSlotData equippedItemSlot;

    // Initialize with references from the view
    public void InitializeFromView(ItemSlotData[] toolSlots, ItemSlotData equippedToolSlot, ItemSlotData[] itemSlots, ItemSlotData equippedItemSlot)
    {
        this.toolSlots = toolSlots;
        this.equippedToolSlot = equippedToolSlot;
        this.itemSlots = itemSlots;
        this.equippedItemSlot = equippedItemSlot;
    }

    // Data Access Methods
    public ItemData GetItemFromString(string name)
    {
        if (_itemIndex == null)
        {
            _itemIndex = Resources.LoadAll<ItemData>("").ToList();
        }
        return _itemIndex.Find(i => i.name == name);
    }

    // Get function for the slots (ItemSlotData)
    public ItemSlotData GetEquippedSlot(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return equippedItemSlot;
        }
        return equippedToolSlot;
    }

    // Get the slot item (ItemData) 
    public ItemData GetEquippedSlotItem(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return equippedItemSlot.itemData;
        }
        return equippedToolSlot.itemData;
    }

    // Get function for the inventory slots
    public ItemSlotData[] GetInventorySlots(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return itemSlots;
        }
        return toolSlots;
    }

    // Check if a hand slot has an item
    public bool SlotEquipped(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return !equippedItemSlot.IsEmpty();
        }
        return !equippedToolSlot.IsEmpty();
    }

    // Check if the item is a tool
    public bool IsTool(ItemData item)
    {
        // Is it equipment? 
        // Try to cast it as equipment first
        EquipmentData equipment = item as EquipmentData;
        if (equipment != null)
        {
            return true;
        }

        // Is it a seed?
        // Try to cast it as a seed
        SeedData seed = item as SeedData;
        // If the seed is not null it is a seed 
        return seed != null;
    }

    // Load the inventory from a save 
    public void LoadInventory(ItemSlotData[] toolSlots, ItemSlotData equippedToolSlot, ItemSlotData[] itemSlots, ItemSlotData equippedItemSlot)
    {
        this.toolSlots = toolSlots;
        this.equippedToolSlot = equippedToolSlot;
        this.itemSlots = itemSlots;
        this.equippedItemSlot = equippedItemSlot;
    }

    // Equip the hand slot with an ItemData (Will overwrite the slot)
    public void EquipHandSlot(ItemData item)
    {
        if (IsTool(item))
        {
            equippedToolSlot = new ItemSlotData(item);
        }
        else
        {
            equippedItemSlot = new ItemSlotData(item);
        }
    }

    // Equip the hand slot with an ItemSlotData (Will overwrite the slot)
    public void EquipHandSlot(ItemSlotData itemSlot)
    {
        // Get the item data from the slot 
        ItemData item = itemSlot.itemData;

        if (IsTool(item))
        {
            equippedToolSlot = new ItemSlotData(itemSlot);
        }
        else
        {
            equippedItemSlot = new ItemSlotData(itemSlot);
        }
    }
}