using UnityEngine;
using BlossomValley.Utilities;

public class InventoryManager : GenericMonoSingleton<InventoryManager>
{
    [Header("Inventory Data")]
    [SerializeField] private ItemSlotData[] toolSlots = new ItemSlotData[8];
    [SerializeField] private ItemSlotData equippedToolSlot = null;
    [SerializeField] private ItemSlotData[] itemSlots = new ItemSlotData[8];
    [SerializeField] private ItemSlotData equippedItemSlot = null;

    [Header("Hand Point")]
    public Transform handPoint;

    private InventoryModel model;
    private InventoryController controller;
    private InventoryView view;

    protected override void Awake()
    {
        base.Awake();
        InitializeInventorySystem();
    }

    private void Start()
    {
        view?.RenderHand();

        if (UIManager.Instance != null)
            UIManager.Instance.RenderInventory();
    }

    private void InitializeInventorySystem()
    {
        model = new InventoryModel(toolSlots, equippedToolSlot, itemSlots, equippedItemSlot);
        view = gameObject.AddComponent<InventoryView>();
        view.HandPoint = handPoint;
        view.Initialize(model);
        controller = new InventoryController(model, view);
    }

    public void InventoryToHand(int slotIndex, InventorySlot.InventoryType inventoryType) => controller?.HandleInventoryToHand(slotIndex, inventoryType);
    public void HandToInventory(InventorySlot.InventoryType inventoryType) => controller?.HandleHandToInventory(inventoryType);
    public void ShopToInventory(ItemSlotData itemSlotToMove) => controller?.HandleShopToInventory(itemSlotToMove);
    public void ConsumeItem(ItemSlotData itemSlot) => controller?.HandleItemConsumption(itemSlot);
    public void EquipHandSlot(ItemData item) => model?.EquipHandSlot(item);
    public void EquipHandSlot(ItemSlotData itemSlot) => model?.EquipHandSlot(itemSlot);

    public bool StackItemToInventory(ItemSlotData itemSlot, ItemSlotData[] inventoryArray)
    {
        controller?.HandleShopToInventory(itemSlot);
        return true;
    }

    public ItemData GetItemFromString(string name) => model?.GetItemFromString(name);
    public ItemData GetEquippedSlotItem(InventorySlot.InventoryType inventoryType) => model?.GetEquippedSlotItem(inventoryType);
    public ItemSlotData GetEquippedSlot(InventorySlot.InventoryType inventoryType) => model?.GetEquippedSlot(inventoryType);
    public ItemSlotData[] GetInventorySlots(InventorySlot.InventoryType inventoryType) => model?.GetInventorySlots(inventoryType);
    public bool SlotEquipped(InventorySlot.InventoryType inventoryType) => model?.IsSlotEquipped(inventoryType) ?? false;
    public bool IsTool(ItemData item) => model?.IsToolType(item) ?? false;

    public InventoryView GetView() => view;
    public void RenderHand() => view?.RenderHand();

    private void OnValidate()
    {
        ValidateInventorySlot(equippedToolSlot);
        ValidateInventorySlot(equippedItemSlot);
        ValidateInventorySlots(itemSlots);
        ValidateInventorySlots(toolSlots);
    }

    private void ValidateInventorySlot(ItemSlotData slot)
    {
        if (slot?.itemData != null && slot.quantity == 0)
            slot.quantity = 1;
    }

    private void ValidateInventorySlots(ItemSlotData[] array)
    {
        if (array == null) return;

        foreach (ItemSlotData slot in array)
            ValidateInventorySlot(slot);
    }
}