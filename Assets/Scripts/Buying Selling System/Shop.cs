using System.Collections.Generic;
using UnityEngine;
using BlossomValley.DialogueSystem;
using BlossomValley.InventorySystem;

namespace BlossomValley.BuyingSellingSystem
{
    public class Shop : InteractableObject
    {
        [Header("Shop Items")]
        [SerializeField] private List<ItemData> shopItems;

        [Header("Dialogues")]
        [SerializeField] private List<DialogueLine> dialogueOnShopOpen;

        public static void Purchase(ItemData item, int quantity)
        {
            int totalCost = item.cost * quantity;
            PlayerModel playerModel = FindAnyObjectByType<PlayerView>().PlayerModel;

            if (playerModel.Money >= totalCost)
            {
                playerModel.Spend(totalCost);

                ItemSlotData purchasedItem = new ItemSlotData(item, quantity);
                InventoryManager.Instance.ShopToInventory(purchasedItem);
            }
        }

        public override void Pickup() => DialogueManager.Instance.StartDialogue(dialogueOnShopOpen, OpenShop);

        private void OpenShop() => UIManager.Instance.OpenShop(shopItems);
    }
}