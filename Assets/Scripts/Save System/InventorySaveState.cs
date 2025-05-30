using BlossomValley.InventorySystem;

namespace BlossomValley.SaveSystem
{
    [System.Serializable]
    public class InventorySaveState
    {
        public ItemSlotSaveData[] toolSlots;
        public ItemSlotSaveData[] itemSlots;

        public ItemSlotSaveData equippedItemSlot;
        public ItemSlotSaveData equippedToolSlot;

        public InventorySaveState(ItemSlotData[] toolSlotsValue, ItemSlotData[] itemSlotsValue, ItemSlotData equippedItemSlotValue, ItemSlotData equippedToolSlotValue)
        {
            toolSlots = ItemSlotData.SerializeArray(toolSlotsValue);
            itemSlots = ItemSlotData.SerializeArray(itemSlotsValue);
            equippedItemSlot = ItemSlotData.SerializeData(equippedItemSlotValue);
            equippedToolSlot = ItemSlotData.SerializeData(equippedToolSlotValue);
        }

        public static InventorySaveState Export()
        {
            ItemSlotData[] toolSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Tool);
            ItemSlotData[] itemSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Item);

            ItemSlotData equippedToolSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool);
            ItemSlotData equippedItemSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item);
            return new InventorySaveState(toolSlots, itemSlots, equippedItemSlot, equippedToolSlot);
        }

        public void LoadData()
        {
            ItemSlotData[] toolSlots = ItemSlotData.DeserializeArray(this.toolSlots);
            ItemSlotData equippedToolSlot = ItemSlotData.DeserializeData(this.equippedToolSlot);
            ItemSlotData[] itemSlots = ItemSlotData.DeserializeArray(this.itemSlots);
            ItemSlotData equippedItemSlot = ItemSlotData.DeserializeData(this.equippedItemSlot);
            InventoryManager.Instance.LoadInventory(toolSlots, equippedToolSlot, itemSlots, equippedItemSlot);
        }
    }
}