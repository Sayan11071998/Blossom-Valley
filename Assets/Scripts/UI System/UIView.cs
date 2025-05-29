using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    [Header("Screen Management")]
    public GameObject menuScreen;
    public UIManager.Tab selectedTab;

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
}