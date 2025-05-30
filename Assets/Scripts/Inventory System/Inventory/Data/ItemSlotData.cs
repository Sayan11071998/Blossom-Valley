using System;

namespace BlossomValley.InventorySystem
{
    [System.Serializable]
    public class ItemSlotData
    {
        public ItemData itemData;
        public int quantity;

        public ItemSlotData(ItemData itemDataValue, int quantityValue)
        {
            itemData = itemDataValue;
            quantity = quantityValue;

            ValidateQuantity();
        }

        public ItemSlotData(ItemData itemData)
        {
            this.itemData = itemData;
            quantity = 1;
            ValidateQuantity();
        }

        public ItemSlotData(ItemSlotData slotToClone)
        {
            itemData = slotToClone.itemData;
            quantity = slotToClone.quantity;
        }

        public void AddQuantity() => AddQuantity(1);

        public void AddQuantity(int amountToAdd) => quantity += amountToAdd;

        public void Remove()
        {
            quantity--;
            ValidateQuantity();
        }

        public bool Stackable(ItemSlotData slotToCompare) => slotToCompare.itemData == itemData;

        private void ValidateQuantity()
        {
            if (quantity <= 0 || itemData == null)
                Empty();
        }

        public void Empty()
        {
            itemData = null;
            quantity = 0;
        }

        public bool IsEmpty() => itemData == null;

        public static ItemSlotSaveData SerializeData(ItemSlotData itemSlot) => new ItemSlotSaveData(itemSlot);

        public static ItemSlotData DeserializeData(ItemSlotSaveData itemSaveSlot)
        {
            ItemData item = InventoryManager.Instance.GetItemFromString(itemSaveSlot.itemID);
            return new ItemSlotData(item, itemSaveSlot.quantity);
        }

        public static ItemSlotSaveData[] SerializeArray(ItemSlotData[] array) => Array.ConvertAll(array, new Converter<ItemSlotData, ItemSlotSaveData>(SerializeData));

        public static ItemSlotData[] DeserializeArray(ItemSlotSaveData[] array) => Array.ConvertAll(array, new Converter<ItemSlotSaveData, ItemSlotData>(DeserializeData));
    }
}