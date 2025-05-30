using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using BlossomValley.InventorySystem;
using BlossomValley.PlayerSystem;

namespace BlossomValley.UISystem
{
    public class ShopListing : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Image itemThumbnail;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI costText;

        private ItemData itemData;

        public void Display(ItemData itemData)
        {
            this.itemData = itemData;
            itemThumbnail.sprite = itemData.thumbnail;
            nameText.text = itemData.name;
            costText.text = itemData.cost + PlayerModel.CURRENCY;
        }

        public void OnPointerClick(PointerEventData eventData) => UIManager.Instance.shopListingManager.OpenConfirmationScreen(itemData);

        public void OnPointerEnter(PointerEventData eventData) => UIManager.Instance.DisplayItemInfo(itemData);

        public void OnPointerExit(PointerEventData eventData) => UIManager.Instance.DisplayItemInfo(null);
    }
}