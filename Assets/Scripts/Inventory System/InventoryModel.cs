using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryModel
{
    private List<ItemData> itemIndex;
    private ItemSlotData[] toolSlots;
    private ItemSlotData equippedToolSlot;
    private ItemSlotData[] itemSlots;
    private ItemSlotData equippedItemSlot;

    public void InitializeFromView(ItemSlotData[] toolSlotsValue, ItemSlotData equippedToolSlotValue, ItemSlotData[] itemSlotsValue, ItemSlotData equippedItemSlotValue)
    {
        toolSlots = toolSlotsValue;
        equippedToolSlot = equippedToolSlotValue;
        itemSlots = itemSlotsValue;
        equippedItemSlot = equippedItemSlotValue;
    }

    public ItemData GetItemFromString(string name)
    {
        if (itemIndex == null)
            itemIndex = Resources.LoadAll<ItemData>("").ToList();

        return itemIndex.Find(i => i.name == name);
    }

    public ItemSlotData GetEquippedSlot(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
            return equippedItemSlot;

        return equippedToolSlot;
    }

    public ItemData GetEquippedSlotItem(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
            return equippedItemSlot.itemData;

        return equippedToolSlot.itemData;
    }

    public ItemSlotData[] GetInventorySlots(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
            return itemSlots;

        return toolSlots;
    }

    public bool SlotEquipped(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
            return !equippedItemSlot.IsEmpty();

        return !equippedToolSlot.IsEmpty();
    }

    public bool IsTool(ItemData item)
    {
        EquipmentData equipment = item as EquipmentData;

        if (equipment != null) return true;

        SeedData seed = item as SeedData;
        return seed != null;
    }

    public void LoadInventory(ItemSlotData[] toolSlotsToLoad, ItemSlotData equippedToolSlotToLoad, ItemSlotData[] itemSlotsToLoad, ItemSlotData equippedItemSlotToLoad)
    {
        toolSlots = toolSlotsToLoad;
        equippedToolSlot = equippedToolSlotToLoad;
        itemSlots = itemSlotsToLoad;
        equippedItemSlot = equippedItemSlotToLoad;
    }

    public void EquipHandSlot(ItemData item)
    {
        if (IsTool(item))
            equippedToolSlot = new ItemSlotData(item);
        else
            equippedItemSlot = new ItemSlotData(item);
    }

    public void EquipHandSlot(ItemSlotData itemSlot)
    {
        ItemData item = itemSlot.itemData;

        if (IsTool(item))
            equippedToolSlot = new ItemSlotData(itemSlot);
        else
            equippedItemSlot = new ItemSlotData(itemSlot);
    }
}