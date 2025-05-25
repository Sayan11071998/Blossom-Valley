using UnityEngine;

public class InventoryView : MonoBehaviour
{
    private Transform handPoint;
    private InventoryController controller;

    public Transform HandPoint
    {
        get => handPoint;
        set => handPoint = value;
    }

    public void Initialize(InventoryController inventoryController)
    {
        controller = inventoryController;
        controller.OnInventoryChanged += OnInventoryChanged;
        RenderHand();
    }

    private void OnDestroy()
    {
        if (controller != null)
            controller.OnInventoryChanged -= OnInventoryChanged;
    }

    private void OnInventoryChanged()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.RenderInventory();
    }

    public void RenderHand()
    {
        if (handPoint == null || controller == null) return;

        ClearHandPoint();

        if (controller.IsSlotEquipped(InventorySlot.InventoryType.Item))
        {
            ItemData equippedItem = controller.GetEquippedSlotItem(InventorySlot.InventoryType.Item);

            if (equippedItem != null && equippedItem.gameModel != null)
                Instantiate(equippedItem.gameModel, handPoint);
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

    public InventorySaveState ExportSaveState() => controller?.HandleExportSaveState();

    public void LoadInventory(InventorySaveState saveState) => controller?.HandleLoadInventory(saveState);
}