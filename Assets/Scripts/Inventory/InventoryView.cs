using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [Header("Hand Point")]
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
        {
            model.OnInventoryChanged -= OnInventoryChanged;
        }
    }

    private void OnInventoryChanged()
    {
        RenderHand();
        if (UIManager.Instance != null)
        {
            UIManager.Instance.RenderInventory();
        }
    }

    public void RenderHand()
    {
        if (handPoint == null) return;

        // Clear existing items
        if (handPoint.childCount > 0)
        {
            for (int i = handPoint.childCount - 1; i >= 0; i--)
            {
                if (Application.isPlaying)
                {
                    Destroy(handPoint.GetChild(i).gameObject);
                }
                else
                {
                    DestroyImmediate(handPoint.GetChild(i).gameObject);
                }
            }
        }

        // Render equipped item
        if (model != null && model.IsSlotEquipped(InventorySlot.InventoryType.Item))
        {
            ItemData equippedItem = model.GetEquippedSlotItem(InventorySlot.InventoryType.Item);
            if (equippedItem != null && equippedItem.gameModel != null)
            {
                Instantiate(equippedItem.gameModel, handPoint);
            }
        }
    }
}