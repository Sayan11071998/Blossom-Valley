using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ITimeTracker
{
    public static UIManager Instance { get; private set; }

    [Header("Screen Management")]
    public GameObject menuScreen;
    public enum Tab { Inventory, Relationships, Animals }
    public Tab selectedTab;

    [Header("Status Bar")]
    public Image toolEquipSlot;
    public Text toolQuantityText;
    public Text timeText;
    public Text dateText;

    [Header("Inventory System")]
    public GameObject inventoryPanel;
    public HandInventorySlot toolHandSlot;
    public InventorySlot[] toolSlots;
    public HandInventorySlot itemHandSlot;
    public InventorySlot[] itemSlots;

    [Header("Item info box")]
    public GameObject itemInfoBox;
    public Text itemNameText;
    public Text itemDescriptionText;

    [Header("Screen Transitions")]
    public GameObject fadeIn;
    public GameObject fadeOut;

    [Header("Prompts")]
    public YesNoPrompt yesNoPrompt;
    public NamingPrompt namingPrompt;

    [Header("Player Stats")]
    public Text moneyText;

    [Header("Shop")]
    public ShopListingManager shopListingManager;

    [Header("Relationships")]
    public RelationshipListingManager relationshipListingManager;
    public AnimalListingManager animalRelationshipListingManager;

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
        RenderPlayerStats();
        DisplayItemInfo(null);
        TimeManager.Instance.RegisterTracker(this);
    }

    #region Prompts
    public void TriggerNamingPrompt(string message, System.Action<string> onConfirmCallback)
    {
        if (namingPrompt.gameObject.activeSelf)
        {
            namingPrompt.QueuePromptAction(() => TriggerNamingPrompt(message, onConfirmCallback));
            return; 
        }
        namingPrompt.gameObject.SetActive(true);
        namingPrompt.CreatePrompt(message, onConfirmCallback); 
    }

    public void TriggerYesNoPrompt(string message, System.Action onYesCallback)
    {
        yesNoPrompt.gameObject.SetActive(true);
        yesNoPrompt.CreatePrompt(message, onYesCallback); 
    }
    #endregion

    #region Tab Management
    public void ToggleMenuPanel()
    {
        menuScreen.SetActive(!menuScreen.activeSelf);
        OpenWindow(selectedTab);
        TabBehaviour.onTabStateChange?.Invoke(); 
    }

    public void OpenWindow(Tab windowToOpen)
    {
        relationshipListingManager.gameObject.SetActive(false);
        inventoryPanel.SetActive(false);
        animalRelationshipListingManager.gameObject.SetActive(false);

        switch (windowToOpen)
        {
            case Tab.Inventory:
                inventoryPanel.SetActive(true);
                RenderInventory(); 
                break;
            case Tab.Relationships:
                relationshipListingManager.gameObject.SetActive(true);
                relationshipListingManager.Render(RelationshipStats.relationships); 
                break;
            case Tab.Animals:
                animalRelationshipListingManager.gameObject.SetActive(true);
                animalRelationshipListingManager.Render(AnimalStats.animalRelationships);
                break;
        }
        selectedTab = windowToOpen; 
    }
    #endregion

    #region Fadein Fadeout Transitions
    public void FadeOutScreen()
    {
        fadeOut.SetActive(true);
    }

    public void FadeInScreen()
    {
        fadeIn.SetActive(true); 
    }

    public void OnFadeInComplete()
    {
        fadeIn.SetActive(false); 
    }

    public void ResetFadeDefaults()
    {
        fadeOut.SetActive(false);
        fadeIn.SetActive(true);
    }
    #endregion

    #region Inventory
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
        ItemSlotData[] inventoryToolSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Tool);
        ItemSlotData[] inventoryItemSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Item);
        RenderInventoryPanel(inventoryToolSlots, toolSlots);
        RenderInventoryPanel(inventoryItemSlots, itemSlots);
        toolHandSlot.Display(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool));
        itemHandSlot.Display(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item));
        ItemData equippedTool = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Tool);
        toolQuantityText.text = "";
        if (equippedTool != null)
        {
            toolEquipSlot.sprite = equippedTool.thumbnail;
            toolEquipSlot.gameObject.SetActive(true);
            int quantity = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool).quantity;
            if (quantity > 1)
            {
                toolQuantityText.text = quantity.ToString();
            }
            return;
        }
        toolEquipSlot.gameObject.SetActive(false);
    }

    void RenderInventoryPanel(ItemSlotData[] slots, InventorySlot[] uiSlots)
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            uiSlots[i].Display(slots[i]);
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
            itemInfoBox.SetActive(false); 
            return;
        }
        itemInfoBox.SetActive(true); 
        itemNameText.text = data.name;
        itemDescriptionText.text = data.description; 
    }
    #endregion

    #region Time
    public void ClockUpdate(GameTimestamp timestamp)
    {
        int hours = timestamp.hour;
        int minutes = timestamp.minute; 
        string prefix = "AM ";
        if (hours > 12)
        {
            prefix = "PM ";
            hours = hours - 12;
        }
        timeText.text = prefix + hours + ":" + minutes.ToString("00");
        int day = timestamp.day;
        string season = timestamp.season.ToString();
        string dayOfTheWeek = timestamp.GetDayOfTheWeek().ToString();
        dateText.text = season + " " + day + " (" + dayOfTheWeek +")";
    }
    #endregion

    public void RenderPlayerStats()
    {
        PlayerModel playerModel = FindAnyObjectByType<PlayerView>().PlayerModel;
        moneyText.text = playerModel.Money + PlayerModel.CURRENCY; 
    }

    public void OpenShop(List<ItemData> shopItems)
    {
        shopListingManager.gameObject.SetActive(true);
        shopListingManager.Render(shopItems); 
    }

    public void ToggleRelationshipPanel()
    {
        GameObject panel = relationshipListingManager.gameObject;
        panel.SetActive(!panel.activeSelf);
        if (panel.activeSelf)
        {
            relationshipListingManager.Render(RelationshipStats.relationships);
        }        
    }
}