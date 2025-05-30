using BlossomValley.BuyingSellingSystem;
using BlossomValley.GameStrings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BlossomValley.InventorySystem;

public class ShopListingManager : ListingManager<ItemData>
{
    [Header("Confirmation Screen")]
    [SerializeField] private GameObject confirmationScreen;
    [SerializeField] private TextMeshProUGUI confirmationPrompt;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private TextMeshProUGUI costCalculationText;
    [SerializeField] private Button purchaseButton;

    private ItemData itemToBuy;
    private int quantity;

    protected override void DisplayListing(ItemData listingItem, GameObject listingGameObject) => listingGameObject.GetComponent<ShopListing>().Display(listingItem);

    public void OpenConfirmationScreen(ItemData item)
    {
        itemToBuy = item;
        quantity = 1;
        RenderConfirmationScreen();
    }

    public void RenderConfirmationScreen()
    {
        confirmationScreen.SetActive(true);
        confirmationPrompt.text = string.Format(GameString.BuyConfirmationPrompt, itemToBuy.name);
        quantityText.text = GameString.MultiplyString + quantity;
        int cost = itemToBuy.cost * quantity;
        PlayerModel playerModel = FindAnyObjectByType<PlayerView>().PlayerModel;
        int playerMoneyLeft = playerModel.Money - cost;

        if (playerMoneyLeft < 0)
        {
            costCalculationText.text = GameString.InsufficientFunds;
            purchaseButton.interactable = false;
            return;
        }
        purchaseButton.interactable = true;
        costCalculationText.text = string.Format(GameString.CostCalculation, playerModel.Money, playerMoneyLeft);
    }

    public void AddQuantity()
    {
        quantity++;
        RenderConfirmationScreen();
    }

    public void SubtractQuantity()
    {
        if (quantity > 1)
            quantity--;

        RenderConfirmationScreen();
    }

    public void ConfirmPurchase()
    {
        Shop.Purchase(itemToBuy, quantity);
        confirmationScreen.SetActive(false);
    }

    public void CancelPurchase() => confirmationScreen.SetActive(false);
}