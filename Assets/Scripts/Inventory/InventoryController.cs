using System;

public class InventoryController
{
    private InventoryModel inventoryModel;
    private InventoryView inventoryView;

    public event Action OnInventoryChanged;

    public InventoryController(InventoryModel modelToSet, InventoryView viewToSet)
    {
        inventoryModel = modelToSet;
        inventoryView = viewToSet;

        inventoryModel.OnInventoryChanged += HandleInventoryChanged;
    }

    public void Cleanup()
    {
        if (inventoryModel != null)
            inventoryModel.OnInventoryChanged -= HandleInventoryChanged;
    }

    private void HandleInventoryChanged()
    {
        OnInventoryChanged?.Invoke();
        if (UIManager.Instance != null)
            UIManager.Instance.RenderInventory();
    }

    public void HandleItemPickup(ItemData item)
    {
        inventoryModel.EquipHandSlot(item);
        inventoryView.RenderHand();
    }

    public void HandleInventoryToHand(int slotIndex, InventorySlot.InventoryType inventoryType)
    {
        ItemSlotData handSlot = inventoryModel.GetEquippedSlot(inventoryType);
        ItemSlotData[] inventoryArray = inventoryModel.GetInventorySlots(inventoryType);

        if (handSlot.Stackable(inventoryArray[slotIndex]))
        {
            ItemSlotData slotToAlter = inventoryArray[slotIndex];
            handSlot.AddQuantity(slotToAlter.quantity);
            slotToAlter.Empty();
        }
        else
        {
            ItemSlotData slotToEquip = new ItemSlotData(inventoryArray[slotIndex]);
            inventoryArray[slotIndex] = new ItemSlotData(handSlot);

            if (slotToEquip.IsEmpty())
                handSlot.Empty();
            else
                inventoryModel.EquipHandSlot(slotToEquip);
        }

        inventoryModel.TriggerInventoryChanged();

        if (inventoryType == InventorySlot.InventoryType.Item)
            inventoryView.RenderHand();
    }

    public void HandleHandToInventory(InventorySlot.InventoryType inventoryType)
    {
        ItemSlotData handSlot = inventoryModel.GetEquippedSlot(inventoryType);
        ItemSlotData[] inventoryArray = inventoryModel.GetInventorySlots(inventoryType);

        if (!TryStackItemToInventory(handSlot, inventoryArray))
        {
            for (int i = 0; i < inventoryArray.Length; i++)
            {
                if (inventoryArray[i].IsEmpty())
                {
                    inventoryArray[i] = new ItemSlotData(handSlot);
                    handSlot.Empty();
                    break;
                }
            }
        }

        inventoryModel.TriggerInventoryChanged();

        if (inventoryType == InventorySlot.InventoryType.Item)
            inventoryView.RenderHand();
    }

    public void HandleShopToInventory(ItemSlotData itemSlotToMove)
    {
        ItemSlotData[] inventoryArray = inventoryModel.IsToolType(itemSlotToMove.itemData) ? inventoryModel.ToolSlots : inventoryModel.ItemSlots;

        if (!TryStackItemToInventory(itemSlotToMove, inventoryArray))
        {
            for (int i = 0; i < inventoryArray.Length; i++)
            {
                if (inventoryArray[i].IsEmpty())
                {
                    inventoryArray[i] = new ItemSlotData(itemSlotToMove);
                    break;
                }
            }
        }

        inventoryModel.TriggerInventoryChanged();
        inventoryView.RenderHand();
    }

    public void HandleItemConsumption(ItemSlotData itemSlot)
    {
        if (itemSlot.IsEmpty()) return;

        itemSlot.Remove();
        inventoryModel.TriggerInventoryChanged();
        inventoryView.RenderHand();
    }

    public void HandleLoadInventory(InventorySaveState saveState)
    {
        if (saveState == null) return;

        ItemSlotData[] toolSlots = ItemSlotData.DeserializeArray(saveState.toolSlots);
        ItemSlotData equippedToolSlot = ItemSlotData.DeserializeData(saveState.equippedToolSlot);
        ItemSlotData[] itemSlots = ItemSlotData.DeserializeArray(saveState.itemSlots);
        ItemSlotData equippedItemSlot = ItemSlotData.DeserializeData(saveState.equippedItemSlot);

        inventoryModel.LoadInventoryData(toolSlots, equippedToolSlot, itemSlots, equippedItemSlot);
        inventoryView.RenderHand();
    }

    public InventorySaveState HandleExportSaveState()
    {
        ItemSlotData[] toolSlots = inventoryModel.GetInventorySlots(InventorySlot.InventoryType.Tool);
        ItemSlotData[] itemSlots = inventoryModel.GetInventorySlots(InventorySlot.InventoryType.Item);
        ItemSlotData equippedToolSlot = inventoryModel.GetEquippedSlot(InventorySlot.InventoryType.Tool);
        ItemSlotData equippedItemSlot = inventoryModel.GetEquippedSlot(InventorySlot.InventoryType.Item);

        return new InventorySaveState(toolSlots, itemSlots, equippedItemSlot, equippedToolSlot);
    }

    public bool IsSlotEquipped(InventorySlot.InventoryType inventoryType) => inventoryModel.IsSlotEquipped(inventoryType);

    public ItemData GetEquippedSlotItem(InventorySlot.InventoryType inventoryType) => inventoryModel.GetEquippedSlotItem(inventoryType);

    public ItemData GetItemFromString(string name) => inventoryModel.GetItemFromString(name);
    public ItemSlotData GetEquippedSlot(InventorySlot.InventoryType inventoryType) => inventoryModel.GetEquippedSlot(inventoryType);
    public ItemSlotData[] GetInventorySlots(InventorySlot.InventoryType inventoryType) => inventoryModel.GetInventorySlots(inventoryType);
    public bool IsTool(ItemData item) => inventoryModel.IsToolType(item);

    private bool TryStackItemToInventory(ItemSlotData itemSlot, ItemSlotData[] inventoryArray)
    {
        for (int i = 0; i < inventoryArray.Length; i++)
        {
            if (inventoryArray[i].Stackable(itemSlot))
            {
                inventoryArray[i].AddQuantity(itemSlot.quantity);
                itemSlot.Empty();
                return true;
            }
        }
        return false;
    }
}