using System.Collections.Generic;
using BlossomValley.GameStrings;

public class ShippingBin : InteractableObject
{
    public static int hourToShip = 18;
    public static List<ItemSlotData> itemsToShip = new List<ItemSlotData>();

    public override void Pickup()
    {
        ItemData handSlotItem = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Item);

        if (handSlotItem == null) return;

        UIManager.Instance.TriggerYesNoPrompt(string.Format(GameString.SellPrompt, handSlotItem.name), PlaceItemsInShippingBin);
    }

    private void PlaceItemsInShippingBin()
    {
        ItemSlotData handSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item);
        itemsToShip.Add(new ItemSlotData(handSlot));
        handSlot.Empty();
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