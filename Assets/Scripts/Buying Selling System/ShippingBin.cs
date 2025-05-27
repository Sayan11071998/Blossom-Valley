using System.Collections.Generic;
using BlossomValley.GameStrings;

public class ShippingBin : InteractableObject
{
    public static int hourToShip = 18;
    public static List<ItemSlotData> itemsToShip = new List<ItemSlotData>();

    public override void Pickup()
    {
        ItemSlotData handSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item);
        
        if (handSlot == null || handSlot.itemData == null || handSlot.quantity <= 0) return;

        // Use quantity prompt if there's more than 1 item, otherwise use simple prompt
        if (handSlot.quantity > 1)
        {
            string message = string.Format("How many {0} would you like to ship?", handSlot.itemData.name);
            UIManager.Instance.TriggerQuantityPrompt(message, handSlot.quantity, PlaceItemsInShippingBin);
        }
        else
        {
            string message = string.Format(GameString.SellPrompt, handSlot.itemData.name);
            UIManager.Instance.TriggerYesNoPrompt(message, () => PlaceItemsInShippingBin(1));
        }
    }

    private void PlaceItemsInShippingBin(int quantityToShip)
    {
        ItemSlotData handSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item);
        
        if (handSlot == null || handSlot.itemData == null || handSlot.quantity < quantityToShip) return;

        // Create a new ItemSlotData with the specified quantity to ship
        ItemSlotData itemsToAdd = new ItemSlotData(handSlot.itemData, quantityToShip);
        itemsToShip.Add(itemsToAdd);
        
        // Reduce the quantity in hand by the amount being shipped
        handSlot.quantity -= quantityToShip;
        
        // If no items left in hand, empty the slot
        if (handSlot.quantity <= 0)
        {
            handSlot.Empty();
        }
        
        InventoryManager.Instance.RenderHand();
    }

    public static void ShipItems()
    {
        int moneyToReceive = TallyItems(itemsToShip);

        PlayerModel playerModel = FindAnyObjectByType<PlayerView>().PlayerModel;
        playerModel.Earn(moneyToReceive);

        itemsToShip.Clear();
    }

    private static int TallyItems(List<ItemSlotData> items)
    {
        int total = 0;

        foreach (ItemSlotData item in items)
            total += item.quantity * item.itemData.cost;

        return total;
    }
}