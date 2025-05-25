[System.Serializable]
public class InventorySaveState
{
    public ItemSlotSaveData[] toolSlots;
    public ItemSlotSaveData[] itemSlots;
    public ItemSlotSaveData equippedItemSlot;
    public ItemSlotSaveData equippedToolSlot;

    public InventorySaveState(ItemSlotData[] toolSlots, ItemSlotData[] itemSlots, ItemSlotData equippedItemSlot, ItemSlotData equippedToolSlot)
    {
        this.toolSlots = ItemSlotData.SerializeArray(toolSlots);
        this.itemSlots = ItemSlotData.SerializeArray(itemSlots);
        this.equippedItemSlot = ItemSlotData.SerializeData(equippedItemSlot);
        this.equippedToolSlot = ItemSlotData.SerializeData(equippedToolSlot);
    }

    public static InventorySaveState Export() => InventoryManager.Instance?.GetView()?.ExportSaveState();

    public void LoadData() => InventoryManager.Instance?.GetView()?.LoadInventory(this);
}