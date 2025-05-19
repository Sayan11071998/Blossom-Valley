using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ITimeTracker
{
    public static UIManager Instance { get; private set; }

    [Header("Inventory System")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private InventorySlot[] toolSlots;
    [SerializeField] private HandInventorySlot toolHandSlot;
    [SerializeField] private InventorySlot[] itemSlots;
    [SerializeField] private HandInventorySlot itemHandSlot;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    [Header("Status Bar")]
    [SerializeField] private Image toolEquipSlot;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI dateText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        RenderInventory();
        AssignSlotIndexes();

        TimeManager.Instance.RegisterTracker(this);
    }

    public void AssignSlotIndexes()
    {
        for (int i = 0; i < toolSlots.Length; i++)
        {
            toolSlots[i].AssignIndex(i);
            itemSlots[i].AssignIndex(i);
        }
    }

    public void RenderInventory()
    {
        ItemData[] inventoryToolSlots = InventoryManager.Instance.tools;
        ItemData[] inventoryItemSlots = InventoryManager.Instance.items;

        RenderInventoryPanel(inventoryToolSlots, toolSlots);
        RenderInventoryPanel(inventoryItemSlots, itemSlots);

        toolHandSlot.Display(InventoryManager.Instance.equippedTool);
        itemHandSlot.Display(InventoryManager.Instance.equippedItem);

        ItemData equippedTool = InventoryManager.Instance.equippedTool;

        if (equippedTool != null)
        {
            toolEquipSlot.sprite = equippedTool.thumbnail;
            toolEquipSlot.gameObject.SetActive(true);
            return;
        }

        toolEquipSlot.gameObject.SetActive(false);
    }

    private void RenderInventoryPanel(ItemData[] slots, InventorySlot[] uiSolts)
    {
        for (int i = 0; i < uiSolts.Length; i++)
        {
            uiSolts[i].Display(slots[i]);
        }
    }

    public void ToggleInventoryPanel()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        RenderInventory();
    }

    public void DisplayItemInfo(ItemData data)
    {
        if (data == null)
        {
            itemNameText.text = "";
            itemDescriptionText.text = "";
            return;
        }

        itemNameText.text = data.name;
        itemDescriptionText.text = data.description;
    }

    public void ClockUpdate(GameTimeStamp timeStamp)
    {
        int hours = timeStamp.hour;
        int minutes = timeStamp.minute;
        string prefix = "AM ";

        if (hours > 12)
        {
            prefix = "PM ";
            hours -= 12;
        }

        timeText.text = prefix + hours + ":" + minutes.ToString("00");

        int day = timeStamp.day;
        string season = timeStamp.season.ToString();
        string dayOfTheWeek = timeStamp.GetDayOfTheWeek().ToString();

        dateText.text = season + " " + day + " (" + dayOfTheWeek + ")";
    }
}