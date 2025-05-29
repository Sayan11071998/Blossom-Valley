using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    [Header("Screen Management")]
    [SerializeField] public GameObject menuScreen;
    [SerializeField] public UIManager.Tab selectedTab;

    [Header("Status Bar")]
    [SerializeField] public Image toolEquipSlot;
    [SerializeField] public TextMeshProUGUI toolQuantityText;
    [SerializeField] public Text timeText;
    [SerializeField] public Text dateText;

    [Header("Inventory System")]
    [SerializeField] public GameObject inventoryPanel;
    [SerializeField] public HandInventorySlot toolHandSlot;
    [SerializeField] public InventorySlot[] toolSlots;
    [SerializeField] public HandInventorySlot itemHandSlot;
    [SerializeField] public InventorySlot[] itemSlots;

    [Header("Item info box")]
    [SerializeField] public GameObject itemInfoBox;
    [SerializeField] public Text itemNameText;
    [SerializeField] public Text itemDescriptionText;

    [Header("Screen Transitions")]
    [SerializeField] public GameObject fadeIn;
    [SerializeField] public GameObject fadeOut;

    [Header("Prompts")]
    [SerializeField] public YesNoPrompt yesNoPrompt;
    [SerializeField] public NamingPrompt namingPrompt;

    [Header("Player Stats")]
    [SerializeField] public Text moneyText;

    [Header("Shop")]
    [SerializeField] public ShopListingManager shopListingManager;

    [Header("Relationships")]
    [SerializeField] public RelationshipListingManager relationshipListingManager;
    [SerializeField] public AnimalListingManager animalRelationshipListingManager;
}