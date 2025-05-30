using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BlossomValley.InventorySystem;

namespace BlossomValley.UISystem
{
    public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public enum InventoryType
        {
            Item, Tool
        }

        [SerializeField] private Image itemDisplayImage;
        [SerializeField] private TextMeshProUGUI quantityText;
        [SerializeField] public InventoryType inventoryType;

        private ItemData itemToDisplay;
        private int quantity;
        private int slotIndex;

        public void Display(ItemSlotData itemSlot)
        {
            itemToDisplay = itemSlot.itemData;
            quantity = itemSlot.quantity;

            quantityText.text = "";

            if (itemToDisplay != null)
            {
                itemDisplayImage.sprite = itemToDisplay.thumbnail;

                if (quantity > 1)
                    quantityText.text = quantity.ToString();

                itemDisplayImage.gameObject.SetActive(true);
                return;
            }

            itemDisplayImage.gameObject.SetActive(false);
        }

        public virtual void OnPointerClick(PointerEventData eventData) => InventoryManager.Instance.InventoryToHand(slotIndex, inventoryType);

        public void AssignIndex(int slotIndexValue) => slotIndex = slotIndexValue;

        public void OnPointerEnter(PointerEventData eventData) => UIManager.Instance.DisplayItemInfo(itemToDisplay);

        public void OnPointerExit(PointerEventData eventData) => UIManager.Instance.DisplayItemInfo(null);
    }
}