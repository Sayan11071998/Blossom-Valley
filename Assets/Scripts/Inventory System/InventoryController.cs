public class InventoryController
{
    private InventoryModel inventoryModel;
    private InventoryView inventoryView;

    public InventoryController(InventoryModel inventoryModelToSet, InventoryView inventoryViewToSet)
    {
        inventoryModel = inventoryModelToSet;
        inventoryView = inventoryViewToSet;
    }

    public void InventoryToHand(int slotIndex, InventorySlot.InventoryType inventoryType)
    {
        ItemSlotData handToEquip = inventoryModel.GetEquippedSlot(InventorySlot.InventoryType.Tool);
        ItemSlotData[] inventoryToAlter = inventoryModel.GetInventorySlots(InventorySlot.InventoryType.Tool);

        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            handToEquip = inventoryModel.GetEquippedSlot(InventorySlot.InventoryType.Item);
            inventoryToAlter = inventoryModel.GetInventorySlots(InventorySlot.InventoryType.Item);
        }

        if (handToEquip.Stackable(inventoryToAlter[slotIndex]))
        {
            ItemSlotData slotToAlter = inventoryToAlter[slotIndex];
            handToEquip.AddQuantity(slotToAlter.quantity);
            slotToAlter.Empty();
        }
        else
        {
            ItemSlotData slotToEquip = new ItemSlotData(inventoryToAlter[slotIndex]);
            inventoryToAlter[slotIndex] = new ItemSlotData(handToEquip);

            if (slotToEquip.IsEmpty())
                handToEquip.Empty();
            else
                inventoryModel.EquipHandSlot(slotToEquip);
        }

        if (inventoryType == InventorySlot.InventoryType.Item)
            inventoryView.RenderHand();

        UIManager.Instance.RenderInventory();
    }

    public void HandToInventory(InventorySlot.InventoryType inventoryType)
    {
        ItemSlotData handSlot = inventoryModel.GetEquippedSlot(InventorySlot.InventoryType.Tool);
        ItemSlotData[] inventoryToAlter = inventoryModel.GetInventorySlots(InventorySlot.InventoryType.Tool);

        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            handSlot = inventoryModel.GetEquippedSlot(InventorySlot.InventoryType.Item);
            inventoryToAlter = inventoryModel.GetInventorySlots(InventorySlot.InventoryType.Item);
        }

        if (!StackItemToInventory(handSlot, inventoryToAlter))
        {
            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    inventoryToAlter[i] = new ItemSlotData(handSlot);
                    handSlot.Empty();
                    break;
                }
            }
        }

        if (inventoryType == InventorySlot.InventoryType.Item)
            inventoryView.RenderHand();

        UIManager.Instance.RenderInventory();
    }

    public bool StackItemToInventory(ItemSlotData itemSlot, ItemSlotData[] inventoryArray)
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

    public void ShopToInventory(ItemSlotData itemSlotToMove)
    {
        ItemSlotData[] inventoryToAlter = inventoryModel.IsTool(itemSlotToMove.itemData) ?
            inventoryModel.GetInventorySlots(InventorySlot.InventoryType.Tool) :
            inventoryModel.GetInventorySlots(InventorySlot.InventoryType.Item);

        if (!StackItemToInventory(itemSlotToMove, inventoryToAlter))
        {
            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    inventoryToAlter[i] = new ItemSlotData(itemSlotToMove);
                    break;
                }
            }
        }

        UIManager.Instance.RenderInventory();
        inventoryView.RenderHand();
    }

    public void ConsumeItem(ItemSlotData itemSlot)
    {
        if (itemSlot.IsEmpty()) return;

        itemSlot.Remove();
        inventoryView.RenderHand();
        UIManager.Instance.RenderInventory();
    }

    public void LoadInventory(ItemSlotData[] toolSlots, ItemSlotData equippedToolSlot, ItemSlotData[] itemSlots, ItemSlotData equippedItemSlot)
    {
        inventoryModel.LoadInventory(toolSlots, equippedToolSlot, itemSlots, equippedItemSlot);
        UIManager.Instance.RenderInventory();
        inventoryView.RenderHand();
    }

    public void EquipHandSlot(ItemData item) => inventoryModel.EquipHandSlot(item);

    public void EquipHandSlot(ItemSlotData itemSlot) => inventoryModel.EquipHandSlot(itemSlot);
}