using UnityEngine;

/// <summary>
/// Entry point MonoBehaviour for the Inventory MVC system
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    // MVC Components
    private InventoryModel model;
    private InventoryController controller;
    private InventoryView view;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        // Initialize MVC components
        InitializeMVC();
    }

    private void InitializeMVC()
    {
        // Create Model
        model = new InventoryModel();

        // Get View component
        view = GetComponent<InventoryView>();
        if (view == null)
        {
            Debug.LogError("InventoryView component not found! Please add InventoryView component to this GameObject.");
            return;
        }

        // Initialize View with Model
        view.Initialize(model);

        // Create Controller
        controller = new InventoryController(model, view);
    }

    #region Public API - Delegating to MVC Components

    // Data Access Methods
    public ItemData GetItemFromString(string name)
    {
        return model.GetItemFromString(name);
    }

    public ItemData GetEquippedSlotItem(InventorySlot.InventoryType inventoryType)
    {
        return model.GetEquippedSlotItem(inventoryType);
    }

    public ItemSlotData GetEquippedSlot(InventorySlot.InventoryType inventoryType)
    {
        return model.GetEquippedSlot(inventoryType);
    }

    public ItemSlotData[] GetInventorySlots(InventorySlot.InventoryType inventoryType)
    {
        return model.GetInventorySlots(inventoryType);
    }

    public bool SlotEquipped(InventorySlot.InventoryType inventoryType)
    {
        return model.SlotEquipped(inventoryType);
    }

    public bool IsTool(ItemData item)
    {
        return model.IsTool(item);
    }

    // Controller Methods
    public void InventoryToHand(int slotIndex, InventorySlot.InventoryType inventoryType)
    {
        controller.InventoryToHand(slotIndex, inventoryType);
    }

    public void HandToInventory(InventorySlot.InventoryType inventoryType)
    {
        controller.HandToInventory(inventoryType);
    }

    public bool StackItemToInventory(ItemSlotData itemSlot, ItemSlotData[] inventoryArray)
    {
        return controller.StackItemToInventory(itemSlot, inventoryArray);
    }

    public void ShopToInventory(ItemSlotData itemSlotToMove)
    {
        controller.ShopToInventory(itemSlotToMove);
    }

    public void ConsumeItem(ItemSlotData itemSlot)
    {
        controller.ConsumeItem(itemSlot);
    }

    public void LoadInventory(ItemSlotData[] toolSlots, ItemSlotData equippedToolSlot, ItemSlotData[] itemSlots, ItemSlotData equippedItemSlot)
    {
        controller.LoadInventory(toolSlots, equippedToolSlot, itemSlots, equippedItemSlot);
    }

    public void EquipHandSlot(ItemData item)
    {
        controller.EquipHandSlot(item);
    }

    public void EquipHandSlot(ItemSlotData itemSlot)  
    {
        controller.EquipHandSlot(itemSlot);
    }

    // View Methods
    public void RenderHand()
    {
        view.RenderHand();
    }

    // Property for handPoint (for backward compatibility)
    public Transform handPoint 
    { 
        get { return view.handPoint; } 
        set { view.handPoint = value; } 
    }

    #endregion

    void Start()
    {
        // Empty - initialization handled in Awake
    }

    void Update()
    {
        // Empty - no update logic needed currently
    }
}