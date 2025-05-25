public class InventoryController
{
    private InventoryModel inventoryModel;
    private InventoryView inventoryView;

    public InventoryController(InventoryModel modelToSet, InventoryView viewToSet)
    {
        inventoryModel = modelToSet;
        inventoryView = viewToSet;
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