using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class InventoryModel
{
    private ItemSlotData[] toolSlots = new ItemSlotData[8];
    private ItemSlotData[] itemSlots = new ItemSlotData[8];
    private ItemSlotData equippedToolSlot = new ItemSlotData(null);
    private ItemSlotData equippedItemSlot = new ItemSlotData(null);

    private List<ItemData> itemIndex;

    public event Action OnInventoryChanged;

    // Properties
    public ItemSlotData[] ToolSlots => toolSlots;
    public ItemSlotData[] ItemSlots => itemSlots;
    public ItemSlotData EquippedToolSlot => equippedToolSlot;
    public ItemSlotData EquippedItemSlot => equippedItemSlot;

    public InventoryModel()
    {
        InitializeSlots();
    }

    public InventoryModel(ItemSlotData[] toolSlots, ItemSlotData equippedToolSlot, 
                         ItemSlotData[] itemSlots, ItemSlotData equippedItemSlot)
    {
        this.toolSlots = toolSlots ?? new ItemSlotData[8];
        this.itemSlots = itemSlots ?? new ItemSlotData[8];
        this.equippedToolSlot = equippedToolSlot ?? new ItemSlotData(null);
        this.equippedItemSlot = equippedItemSlot ?? new ItemSlotData(null);
        
        InitializeSlots();
    }

    private void InitializeSlots()
    {
        for (int i = 0; i < toolSlots.Length; i++)
        {
            if (toolSlots[i] == null)
                toolSlots[i] = new ItemSlotData(null);
        }

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i] == null)
                itemSlots[i] = new ItemSlotData(null);
        }

        if (equippedToolSlot == null)
            equippedToolSlot = new ItemSlotData(null);
        if (equippedItemSlot == null)
            equippedItemSlot = new ItemSlotData(null);
    }

    public ItemData GetItemFromString(string name)
    {
        if (itemIndex == null)
        {
            itemIndex = Resources.LoadAll<ItemData>("").ToList();
        }
        return itemIndex.Find(i => i.name == name);
    }

    public void LoadInventoryData(ItemSlotData[] toolSlots, ItemSlotData equippedToolSlot, 
                                 ItemSlotData[] itemSlots, ItemSlotData equippedItemSlot)
    {
        this.toolSlots = toolSlots ?? new ItemSlotData[8];
        this.equippedToolSlot = equippedToolSlot ?? new ItemSlotData(null);
        this.itemSlots = itemSlots ?? new ItemSlotData[8];
        this.equippedItemSlot = equippedItemSlot ?? new ItemSlotData(null);

        InitializeSlots();
        NotifyInventoryChanged();
    }

    public ItemSlotData[] GetInventorySlots(InventorySlot.InventoryType inventoryType)
    {
        return inventoryType == InventorySlot.InventoryType.Item ? itemSlots : toolSlots;
    }

    public ItemSlotData GetEquippedSlot(InventorySlot.InventoryType inventoryType)
    {
        return inventoryType == InventorySlot.InventoryType.Item ? equippedItemSlot : equippedToolSlot;
    }

    public ItemData GetEquippedSlotItem(InventorySlot.InventoryType inventoryType)
    {
        return GetEquippedSlot(inventoryType).itemData;
    }

    public bool IsSlotEquipped(InventorySlot.InventoryType inventoryType)
    {
        return !GetEquippedSlot(inventoryType).IsEmpty();
    }

    public void EquipHandSlot(ItemData item)
    {
        if (IsToolType(item))
        {
            equippedToolSlot = new ItemSlotData(item);
        }
        else
        {
            equippedItemSlot = new ItemSlotData(item);
        }
        NotifyInventoryChanged();
    }

    public void EquipHandSlot(ItemSlotData itemSlot)
    {
        ItemData item = itemSlot.itemData;
        if (IsToolType(item))
        {
            equippedToolSlot = new ItemSlotData(itemSlot);
        }
        else
        {
            equippedItemSlot = new ItemSlotData(itemSlot);
        }
        NotifyInventoryChanged();
    }

    public bool IsToolType(ItemData item)
    {
        if (item == null) return false;
        
        EquipmentData equipment = item as EquipmentData;
        if (equipment != null) return true;

        SeedData seed = item as SeedData;
        return seed != null;
    }

    private void NotifyInventoryChanged()
    {
        OnInventoryChanged?.Invoke();
    }

    public void TriggerInventoryChanged()
    {
        NotifyInventoryChanged();
    }
}