using System;

[Serializable]
public class ItemSlotData
{
    public ItemData itemData;
    public int quantity;

    public ItemSlotData(ItemData itemData, int quantity = 1)
    {
        this.itemData = itemData;
        this.quantity = quantity;
        ValidateQuantity();
    }

    public ItemSlotData(ItemSlotData slotToClone)
    {
        if (slotToClone != null)
        {
            itemData = slotToClone.itemData;
            quantity = slotToClone.quantity;
        }
        else
        {
            itemData = null;
            quantity = 0;
        }
    }

    public void AddQuantity(int amountToAdd = 1) => quantity += amountToAdd;

    public void Remove()
    {
        quantity--;
        ValidateQuantity();
    }

    public bool Stackable(ItemSlotData slotToCompare) => slotToCompare != null && slotToCompare.itemData == itemData && itemData != null;

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
        if (InventoryManager.Instance == null)
            return new ItemSlotData(null);

        ItemData item = InventoryManager.Instance.GetItemFromString(itemSaveSlot.itemID);
        return new ItemSlotData(item, itemSaveSlot.quantity);
    }

    public static ItemSlotSaveData[] SerializeArray(ItemSlotData[] array) => Array.ConvertAll(array, SerializeData);

    public static ItemSlotData[] DeserializeArray(ItemSlotSaveData[] array) => Array.ConvertAll(array, DeserializeData);
}