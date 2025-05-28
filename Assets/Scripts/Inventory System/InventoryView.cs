using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [Header("Tools")]
    [SerializeField] private ItemSlotData[] toolSlots = new ItemSlotData[8];
    [SerializeField] private ItemSlotData equippedToolSlot = null;

    [Header("Items")]
    [SerializeField] private ItemSlotData[] itemSlots = new ItemSlotData[8];
    [SerializeField] private ItemSlotData equippedItemSlot = null;

    [Header("Player Hand Point")]
    [SerializeField] private Transform handPoint;

    private InventoryModel model;

    public void Initialize(InventoryModel modelToInitialize)
    {
        model = modelToInitialize;
        modelToInitialize.InitializeFromView(toolSlots, equippedToolSlot, itemSlots, equippedItemSlot);
    }

    public void RenderHand()
    {
        if (handPoint.childCount > 0)
            Destroy(handPoint.GetChild(0).gameObject);

        if (model.SlotEquipped(InventorySlot.InventoryType.Item))
            Instantiate(model.GetEquippedSlotItem(InventorySlot.InventoryType.Item).gameModel, handPoint);
    }

    private void OnValidate()
    {
        ValidateInventorySlot(equippedToolSlot);
        ValidateInventorySlot(equippedItemSlot);
        ValidateInventorySlots(itemSlots);
        ValidateInventorySlots(toolSlots);
    }

    void ValidateInventorySlot(ItemSlotData slot)
    {
        if (slot.itemData != null && slot.quantity == 0)
            slot.quantity = 1;
    }

    void ValidateInventorySlots(ItemSlotData[] array)
    {
        foreach (ItemSlotData slot in array)
            ValidateInventorySlot(slot);
    }
}