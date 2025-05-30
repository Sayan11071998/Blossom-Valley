using BlossomValley.UISystem;
using UnityEngine;

namespace BlossomValley.InventorySystem
{
    public class InventoryManager : MonoBehaviour
    {
        public static InventoryManager Instance { get; private set; }

        private InventoryModel inventoryModel;
        private InventoryController inventoryController;
        private InventoryView inventoryView;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;

            InitializeMVC();
        }

        private void InitializeMVC()
        {
            inventoryModel = new InventoryModel();
            inventoryView = GetComponent<InventoryView>();

            if (inventoryView == null) return;

            inventoryView.Initialize(inventoryModel);
            inventoryController = new InventoryController(inventoryModel, inventoryView);
        }

        public ItemData GetItemFromString(string name) => inventoryModel.GetItemFromString(name);

        public ItemData GetEquippedSlotItem(InventorySlot.InventoryType inventoryType) => inventoryModel.GetEquippedSlotItem(inventoryType);

        public ItemSlotData GetEquippedSlot(InventorySlot.InventoryType inventoryType) => inventoryModel.GetEquippedSlot(inventoryType);

        public ItemSlotData[] GetInventorySlots(InventorySlot.InventoryType inventoryType) => inventoryModel.GetInventorySlots(inventoryType);

        public bool SlotEquipped(InventorySlot.InventoryType inventoryType) => inventoryModel.SlotEquipped(inventoryType);

        public bool IsTool(ItemData item) => inventoryModel.IsTool(item);

        public void InventoryToHand(int slotIndex, InventorySlot.InventoryType inventoryType) => inventoryController.InventoryToHand(slotIndex, inventoryType);

        public void HandToInventory(InventorySlot.InventoryType inventoryType) => inventoryController.HandToInventory(inventoryType);

        public bool StackItemToInventory(ItemSlotData itemSlot, ItemSlotData[] inventoryArray) => inventoryController.StackItemToInventory(itemSlot, inventoryArray);

        public void ShopToInventory(ItemSlotData itemSlotToMove) => inventoryController.ShopToInventory(itemSlotToMove);

        public void ConsumeItem(ItemSlotData itemSlot) => inventoryController.ConsumeItem(itemSlot);

        public void LoadInventory(ItemSlotData[] toolSlots, ItemSlotData equippedToolSlot, ItemSlotData[] itemSlots, ItemSlotData equippedItemSlot) => inventoryController.LoadInventory(toolSlots, equippedToolSlot, itemSlots, equippedItemSlot);

        public void EquipHandSlot(ItemData item) => inventoryController.EquipHandSlot(item);

        public void EquipHandSlot(ItemSlotData itemSlot) => inventoryController.EquipHandSlot(itemSlot);

        public void RenderHand() => inventoryView.RenderHand();
    }
}