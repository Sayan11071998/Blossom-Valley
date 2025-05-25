using UnityEngine;

public class InventoryView : MonoBehaviour
{
    public Transform handPoint;

    private InventoryModel model;

    public void Initialize(InventoryModel inventoryModel)
    {
        model = inventoryModel;
        model.OnInventoryChanged += OnInventoryChanged;
        RenderHand();
    }

    private void OnDestroy()
    {
        if (model != null)
            model.OnInventoryChanged -= OnInventoryChanged;
    }

    private void OnInventoryChanged()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.RenderInventory();
    }

    public void RenderHand()
    {
        if (handPoint == null) return;

        ClearHandPoint();

        if (model == null) return;

        if (model.IsSlotEquipped(InventorySlot.InventoryType.Item))
        {
            ItemData equippedItem = model.GetEquippedSlotItem(InventorySlot.InventoryType.Item);

            if (equippedItem != null && equippedItem.gameModel != null)
            {
                Instantiate(equippedItem.gameModel, handPoint);
            }
        }
    }

    private void ClearHandPoint()
    {
        if (handPoint.childCount > 0)
        {
            for (int i = handPoint.childCount - 1; i >= 0; i--)
            {
                if (Application.isPlaying)
                    Destroy(handPoint.GetChild(i).gameObject);
                else
                    DestroyImmediate(handPoint.GetChild(i).gameObject);
            }
        }
    }

    public InventorySaveState ExportSaveState()
    {
        if (model == null) return null;

        ItemSlotData[] toolSlots = model.GetInventorySlots(InventorySlot.InventoryType.Tool);
        ItemSlotData[] itemSlots = model.GetInventorySlots(InventorySlot.InventoryType.Item);
        ItemSlotData equippedToolSlot = model.GetEquippedSlot(InventorySlot.InventoryType.Tool);
        ItemSlotData equippedItemSlot = model.GetEquippedSlot(InventorySlot.InventoryType.Item);

        return new InventorySaveState(toolSlots, itemSlots, equippedItemSlot, equippedToolSlot);
    }

    public void LoadInventory(InventorySaveState saveState)
    {
        if (model == null || saveState == null) return;

        ItemSlotData[] toolSlots = ItemSlotData.DeserializeArray(saveState.toolSlots);
        ItemSlotData equippedToolSlot = ItemSlotData.DeserializeData(saveState.equippedToolSlot);
        ItemSlotData[] itemSlots = ItemSlotData.DeserializeArray(saveState.itemSlots);
        ItemSlotData equippedItemSlot = ItemSlotData.DeserializeData(saveState.equippedItemSlot);

        model.LoadInventoryData(toolSlots, equippedToolSlot, itemSlots, equippedItemSlot);
    }
}