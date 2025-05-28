using UnityEngine;

/// <summary>
/// Pure C# Controller class for Inventory business logic
/// </summary>
public class InventoryController
{
    private InventoryModel model;
    private InventoryView view;

    public InventoryController(InventoryModel model, InventoryView view)
    {
        this.model = model;
        this.view = view;
    }

    // Handles movement of item from Inventory to Hand
    public void InventoryToHand(int slotIndex, InventorySlot.InventoryType inventoryType)
    {
        // The slot to equip (Tool by default)
        ItemSlotData handToEquip = model.GetEquippedSlot(InventorySlot.InventoryType.Tool);
        // The array to change
        ItemSlotData[] inventoryToAlter = model.GetInventorySlots(InventorySlot.InventoryType.Tool);

        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            // Change the slot to item
            handToEquip = model.GetEquippedSlot(InventorySlot.InventoryType.Item);
            inventoryToAlter = model.GetInventorySlots(InventorySlot.InventoryType.Item);
        }

        // Check if stackable
        if (handToEquip.Stackable(inventoryToAlter[slotIndex]))
        {
            ItemSlotData slotToAlter = inventoryToAlter[slotIndex];

            // Add to the hand slot
            handToEquip.AddQuantity(slotToAlter.quantity);

            // Empty the inventory slot
            slotToAlter.Empty();
        }
        else
        {
            // Not stackable
            // Cache the Inventory ItemSlotData
            ItemSlotData slotToEquip = new ItemSlotData(inventoryToAlter[slotIndex]);

            // Change the inventory slot to the hands
            inventoryToAlter[slotIndex] = new ItemSlotData(handToEquip);

            // Check if the slot to equip is empty
            if (slotToEquip.IsEmpty())
            {
                // Empty the hand instead
                handToEquip.Empty();
            }
            else
            {
                model.EquipHandSlot(slotToEquip);
            }
        }

        // Update the changes in the scene
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            view.RenderHand();
        }

        // Update the changes to the UI
        UIManager.Instance.RenderInventory();
    }

    // Handles movement of item from Hand to Inventory
    public void HandToInventory(InventorySlot.InventoryType inventoryType)
    {
        // The slot to move from (Tool by default)
        ItemSlotData handSlot = model.GetEquippedSlot(InventorySlot.InventoryType.Tool);
        // The array to change
        ItemSlotData[] inventoryToAlter = model.GetInventorySlots(InventorySlot.InventoryType.Tool);

        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            handSlot = model.GetEquippedSlot(InventorySlot.InventoryType.Item);
            inventoryToAlter = model.GetInventorySlots(InventorySlot.InventoryType.Item);
        }

        // Try stacking the hand slot. 
        // Check if the operation failed
        if (!StackItemToInventory(handSlot, inventoryToAlter))
        {
            // Find an empty slot to put the item in 
            // Iterate through each inventory slot and find an empty slot
            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    // Send the equipped item over to its new slot
                    inventoryToAlter[i] = new ItemSlotData(handSlot);
                    // Remove the item from the hand
                    handSlot.Empty();
                    break;
                }
            }
        }

        // Update the changes in the scene
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            view.RenderHand();
        }

        // Update the changes to the UI
        UIManager.Instance.RenderInventory();
    }

    // Iterate through each of the items in the inventory to see if it can be stacked
    // Will perform the operation if found, returns false if unsuccessful
    public bool StackItemToInventory(ItemSlotData itemSlot, ItemSlotData[] inventoryArray)
    {
        for (int i = 0; i < inventoryArray.Length; i++)
        {
            if (inventoryArray[i].Stackable(itemSlot))
            {
                // Add to the inventory slot's stack
                inventoryArray[i].AddQuantity(itemSlot.quantity);
                // Empty the item slot
                itemSlot.Empty();
                return true;
            }
        }

        // Can't find any slot that can be stacked
        return false;
    }

    // Handles movement of item from Shop to Inventory
    public void ShopToInventory(ItemSlotData itemSlotToMove)
    {
        // The inventory array to change
        ItemSlotData[] inventoryToAlter = model.IsTool(itemSlotToMove.itemData) ? 
            model.GetInventorySlots(InventorySlot.InventoryType.Tool) : 
            model.GetInventorySlots(InventorySlot.InventoryType.Item);

        // Try stacking the hand slot. 
        // Check if the operation failed
        if (!StackItemToInventory(itemSlotToMove, inventoryToAlter))
        {
            // Find an empty slot to put the item in 
            // Iterate through each inventory slot and find an empty slot
            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    // Send the equipped item over to its new slot
                    inventoryToAlter[i] = new ItemSlotData(itemSlotToMove);
                    break;
                }
            }
        }

        // Update the changes to the UI
        UIManager.Instance.RenderInventory();
        view.RenderHand();
    }

    public void ConsumeItem(ItemSlotData itemSlot)
    {
        if (itemSlot.IsEmpty())
        {
            Debug.LogError("There is nothing to consume!");
            return;
        }

        // Use up one of the item slots
        itemSlot.Remove();
        // Refresh inventory
        view.RenderHand();
        UIManager.Instance.RenderInventory();
    }

    public void LoadInventory(ItemSlotData[] toolSlots, ItemSlotData equippedToolSlot, ItemSlotData[] itemSlots, ItemSlotData equippedItemSlot)
    {
        model.LoadInventory(toolSlots, equippedToolSlot, itemSlots, equippedItemSlot);

        // Update the changes in the UI and scene
        UIManager.Instance.RenderInventory();
        view.RenderHand();
    }

    public void EquipHandSlot(ItemData item)
    {
        model.EquipHandSlot(item);
    }

    public void EquipHandSlot(ItemSlotData itemSlot)
    {
        model.EquipHandSlot(itemSlot);
    }
}