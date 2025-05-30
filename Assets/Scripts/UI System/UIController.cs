using System.Collections.Generic;
using UnityEngine;
using BlossomValley.AnimalSystem;
using BlossomValley.CharacterSystem;

public class UIController
{
    private UIView uiView;

    public UIController(UIView viewToSet) => uiView = viewToSet;

    public void TriggerNamingPrompt(string message, System.Action<string> onConfirmCallback)
    {
        if (uiView.namingPrompt.gameObject.activeSelf)
        {
            uiView.namingPrompt.QueuePromptAction(() => TriggerNamingPrompt(message, onConfirmCallback));
            return;
        }

        uiView.namingPrompt.gameObject.SetActive(true);
        uiView.namingPrompt.CreatePrompt(message, onConfirmCallback);
    }

    public void TriggerYesNoPrompt(string message, System.Action onYesCallback)
    {
        uiView.yesNoPrompt.gameObject.SetActive(true);
        uiView.yesNoPrompt.CreatePrompt(message, onYesCallback);
    }

    public void TriggerQuantityPrompt(string message, int maxQuantity, System.Action<int> onConfirmCallback)
    {
        if (uiView.yesNoPrompt.gameObject.activeSelf) return;

        uiView.yesNoPrompt.gameObject.SetActive(true);
        uiView.yesNoPrompt.CreateQuantityPrompt(message, maxQuantity, onConfirmCallback);
    }

    public void ToggleMenuPanel()
    {
        uiView.menuScreen.SetActive(!uiView.menuScreen.activeSelf);
        OpenWindow(uiView.selectedTab);
        TabBehaviour.onTabStateChange?.Invoke();
    }

    public void OpenWindow(UIManager.Tab windowToOpen)
    {
        uiView.relationshipListingManager.gameObject.SetActive(false);
        uiView.inventoryPanel.SetActive(false);
        uiView.animalRelationshipListingManager.gameObject.SetActive(false);

        switch (windowToOpen)
        {
            case UIManager.Tab.Inventory:
                uiView.inventoryPanel.SetActive(true);
                RenderInventory();
                break;
            case UIManager.Tab.Relationships:
                uiView.relationshipListingManager.gameObject.SetActive(true);
                uiView.relationshipListingManager.Render(RelationshipStats.relationships);
                break;
            case UIManager.Tab.Animals:
                uiView.animalRelationshipListingManager.gameObject.SetActive(true);
                uiView.animalRelationshipListingManager.Render(AnimalStats.animalRelationships);
                break;
        }
        uiView.selectedTab = windowToOpen;
    }

    public void FadeOutScreen() => uiView.fadeOut.SetActive(true);

    public void FadeInScreen() => uiView.fadeIn.SetActive(true);

    public void OnFadeInComplete() => uiView.fadeIn.SetActive(false);

    public void ResetFadeDefaults()
    {
        uiView.fadeOut.SetActive(false);
        uiView.fadeIn.SetActive(true);
    }

    public void AssignSlotIndexes()
    {
        for (int i = 0; i < uiView.toolSlots.Length; i++)
        {
            uiView.toolSlots[i].AssignIndex(i);
            uiView.itemSlots[i].AssignIndex(i);
        }
    }

    public void RenderInventory()
    {
        ItemSlotData[] inventoryToolSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Tool);
        ItemSlotData[] inventoryItemSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Item);
        RenderInventoryPanel(inventoryToolSlots, uiView.toolSlots);
        RenderInventoryPanel(inventoryItemSlots, uiView.itemSlots);
        uiView.toolHandSlot.Display(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool));
        uiView.itemHandSlot.Display(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item));
        ItemData equippedTool = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Tool);
        uiView.toolQuantityText.text = "";
        if (equippedTool != null)
        {
            uiView.toolEquipSlot.sprite = equippedTool.thumbnail;
            uiView.toolEquipSlot.gameObject.SetActive(true);
            int quantity = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool).quantity;

            if (quantity > 1)
                uiView.toolQuantityText.text = quantity.ToString();

            return;
        }
        uiView.toolEquipSlot.gameObject.SetActive(false);
    }

    void RenderInventoryPanel(ItemSlotData[] slots, InventorySlot[] uiSlots)
    {
        for (int i = 0; i < uiSlots.Length; i++)
            uiSlots[i].Display(slots[i]);
    }

    public void ToggleInventoryPanel()
    {
        uiView.inventoryPanel.SetActive(!uiView.inventoryPanel.activeSelf);
        RenderInventory();
    }

    public void DisplayItemInfo(ItemData data)
    {
        if (data == null)
        {
            uiView.itemNameText.text = "";
            uiView.itemDescriptionText.text = "";
            uiView.itemInfoBox.SetActive(false);
            return;
        }
        uiView.itemInfoBox.SetActive(true);
        uiView.itemNameText.text = data.name;
        uiView.itemDescriptionText.text = data.description;
    }

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
        uiView.timeText.text = prefix + hours + ":" + minutes.ToString("00");
        int day = timestamp.day;
        string season = timestamp.season.ToString();
        string dayOfTheWeek = timestamp.GetDayOfTheWeek().ToString();
        uiView.dateText.text = season + " " + day + " (" + dayOfTheWeek + ")";
    }

    public void RenderPlayerStats()
    {
        PlayerModel playerModel = Object.FindAnyObjectByType<PlayerView>().PlayerModel;
        uiView.moneyText.text = playerModel.Money + PlayerModel.CURRENCY;
    }

    public void OpenShop(List<ItemData> shopItems)
    {
        uiView.shopListingManager.gameObject.SetActive(true);
        uiView.shopListingManager.Render(shopItems);
    }

    public void ToggleRelationshipPanel()
    {
        GameObject panel = uiView.relationshipListingManager.gameObject;
        panel.SetActive(!panel.activeSelf);
        if (panel.activeSelf)
            uiView.relationshipListingManager.Render(RelationshipStats.relationships);
    }
}