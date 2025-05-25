using UnityEngine;
using BlossomValley.Utilities;

public class InventoryManager : GenericMonoSingleton<InventoryManager>
{
    public static InventoryManager Instance { get; private set; }

    [Header("Tools")]
    [SerializeField] private ItemSlotData[] toolSlots = new ItemSlotData[8];
    [SerializeField] private ItemSlotData equippedToolSlot = null;

    [Header("Items")]
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

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InitializeInventorySystem();
    }

    private void Start()
    {
        if (view != null)
            view.RenderHand();

        if (UIManager.Instance != null)
            UIManager.Instance.RenderInventory();
    }

    private void InitializeInventorySystem()
    {
        model = new InventoryModel(toolSlots, equippedToolSlot, itemSlots, equippedItemSlot);
        view = gameObject.AddComponent<InventoryView>();
        view.handPoint = handPoint;
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
        if (model.IsToolType(itemSlot.itemData))
            controller.HandleShopToInventory(itemSlot);
        else
            controller.HandleShopToInventory(itemSlot);

        return true;
    }

    public ItemData GetItemFromString(string name) => model?.GetItemFromString(name);

    public ItemData GetEquippedSlotItem(InventorySlot.InventoryType inventoryType) => model?.GetEquippedSlotItem(inventoryType);

    public ItemSlotData GetEquippedSlot(InventorySlot.InventoryType inventoryType) => model?.GetEquippedSlot(inventoryType);

    public ItemSlotData[] GetInventorySlots(InventorySlot.InventoryType inventoryType) => model?.GetInventorySlots(inventoryType);

    public bool SlotEquipped(InventorySlot.InventoryType inventoryType) => model?.IsSlotEquipped(inventoryType) ?? false;

    public bool IsTool(ItemData item) => model?.IsToolType(item) ?? false;

    public void LoadInventory(ItemSlotData[] toolSlots, ItemSlotData equippedToolSlot, ItemSlotData[] itemSlots, ItemSlotData equippedItemSlot) => model?.LoadInventoryData(toolSlots, equippedToolSlot, itemSlots, equippedItemSlot);

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