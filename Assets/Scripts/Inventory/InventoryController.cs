public class InventoryController
{
    private readonly InventoryModel model;
    private readonly InventoryView view;

    public InventoryController(InventoryModel model, InventoryView view)
    {
        this.model = model;
        this.view = view;
    }

    public void HandleItemPickup(ItemData item)
    {
        model.EquipHandSlot(item);
        view.RenderHand(); // Controller triggers view update
    }

    public void HandleInventoryToHand(int slotIndex, InventorySlot.InventoryType inventoryType)
    {
        ItemSlotData handSlot = model.GetEquippedSlot(inventoryType);
        ItemSlotData[] inventoryArray = model.GetInventorySlots(inventoryType);

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
                model.EquipHandSlot(slotToEquip);
        }

        model.TriggerInventoryChanged();
        
        // Only render hand if dealing with items (matching original behavior)
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            view.RenderHand();
        }
    }

    public void HandleHandToInventory(InventorySlot.InventoryType inventoryType)
    {
        ItemSlotData handSlot = model.GetEquippedSlot(inventoryType);
        ItemSlotData[] inventoryArray = model.GetInventorySlots(inventoryType);

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

        model.TriggerInventoryChanged();
        
        // Only render hand if dealing with items (matching original behavior)
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            view.RenderHand();
        }
    }

    public void HandleShopToInventory(ItemSlotData itemSlotToMove)
    {
        ItemSlotData[] inventoryArray = model.IsToolType(itemSlotToMove.itemData) ? model.ToolSlots : model.ItemSlots;

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

        model.TriggerInventoryChanged();
        view.RenderHand(); // Always render hand after shop operations
    }

    public void HandleItemConsumption(ItemSlotData itemSlot)
    {
        if (itemSlot.IsEmpty()) return;

        itemSlot.Remove();
        model.TriggerInventoryChanged();
        view.RenderHand(); // Render hand after consuming items
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