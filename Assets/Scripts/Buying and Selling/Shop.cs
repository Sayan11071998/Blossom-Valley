using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : InteractableObject
{
    public List<ItemData> shopItems;

    [Header("Dialogues")]
    public List<DialogueLine> dialogueOnShopOpen;

    public static void Purchase(ItemData item, int quantity)
    {
        int totalCost = item.cost * quantity; 
        PlayerModel playerModel = FindAnyObjectByType<PlayerView>().PlayerModel;

        if (playerModel.Money >= totalCost)
        {
            // Deduct from the player's money
            playerModel.Spend(totalCost);
            // Create an ItemSlotData for the purchased item
            ItemSlotData purchasedItem = new ItemSlotData(item, quantity);

            // Send it to the player's inventory
            InventoryManager.Instance.ShopToInventory(purchasedItem); 
        }
    }

    public override void Pickup()
    {
        DialogueManager.Instance.StartDialogue(dialogueOnShopOpen, OpenShop);
    }

    void OpenShop()
    {
        UIManager.Instance.OpenShop(shopItems);
    }
}