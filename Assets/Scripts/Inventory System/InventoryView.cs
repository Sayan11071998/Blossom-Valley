using UnityEngine;

/// <summary>
/// MonoBehaviour View class for Inventory visual representation
/// </summary>
public class InventoryView : MonoBehaviour
{
    [Header("Tools")]
    //Tool Slots
    [SerializeField]
    private ItemSlotData[] toolSlots = new ItemSlotData[8];
    //Tool in the player's hand
    [SerializeField]
    private ItemSlotData equippedToolSlot = null;

    [Header("Items")]
    //Item Slots
    [SerializeField]
    private ItemSlotData[] itemSlots = new ItemSlotData[8];
    //Item in the player's hand
    [SerializeField]
    private ItemSlotData equippedItemSlot = null;

    //The transform for the player to hold items in the scene
    public Transform handPoint;

    private InventoryModel model;

    public void Initialize(InventoryModel model)
    {
        this.model = model;
        // Pass the serialized data to the model
        model.InitializeFromView(toolSlots, equippedToolSlot, itemSlots, equippedItemSlot);
    }

    // Render the player's equipped item in the scene
    public void RenderHand()
    {
        // Reset objects on the hand
        if (handPoint.childCount > 0)
        {
            Destroy(handPoint.GetChild(0).gameObject);
        }

        // Check if the player has anything equipped
        if (model.SlotEquipped(InventorySlot.InventoryType.Item))
        {
            // Instantiate the game model on the player's hand and put it on the scene
            Instantiate(model.GetEquippedSlotItem(InventorySlot.InventoryType.Item).gameModel, handPoint);
        }
    }

    // Validation Methods for Inspector
    private void OnValidate()
    {
        //Validate the hand slots
        ValidateInventorySlot(equippedToolSlot);
        ValidateInventorySlot(equippedItemSlot);

        //Validate the slots in the inventoryy
        ValidateInventorySlots(itemSlots);
        ValidateInventorySlots(toolSlots);
    }

    //When giving the itemData value in the inspector, automatically set the quantity to 1 
    void ValidateInventorySlot(ItemSlotData slot)
    {
        if (slot.itemData != null && slot.quantity == 0)
        {
            slot.quantity = 1;
        }
    }

    //Validate arrays
    void ValidateInventorySlots(ItemSlotData[] array)
    {
        foreach (ItemSlotData slot in array)
        {
            ValidateInventorySlot(slot);
        }
    }
}