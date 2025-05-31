using System.Collections.Generic;
using UnityEngine;
using BlossomValley.InventorySystem;
using BlossomValley.TimeSystem;
using BlossomValley.CalendarSystem;

namespace BlossomValley.UISystem
{
    public class UIManager : MonoBehaviour, ITimeTracker
    {
        public static UIManager Instance { get; private set; }

        public enum Tab { Inventory, Relationships, Animals }

        [SerializeField] private InteractBubble interactBubble;

        private UIView uiView;
        private UIController uiController;

        public Tab selectedTab => uiView.selectedTab;
        public ShopListingManager shopListingManager => uiView.shopListingManager;
        public CalendarUIListing calendar => uiView.calendar;

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;

            uiView = GetComponent<UIView>();
            uiController = new UIController(uiView);
        }

        private void Start()
        {
            uiController.RenderInventory();
            uiController.AssignSlotIndexes();
            uiController.RenderPlayerStats();
            uiController.DisplayItemInfo(null);
            TimeManager.Instance.RegisterTracker(this);
            DeactivateInteractPrompt();
        }

        public void TriggerNamingPrompt(string message, System.Action<string> onConfirmCallback) => uiController.TriggerNamingPrompt(message, onConfirmCallback);

        public void TriggerYesNoPrompt(string message, System.Action onYesCallback) => uiController.TriggerYesNoPrompt(message, onYesCallback);

        public void TriggerQuantityPrompt(string message, int maxQuantity, System.Action<int> onConfirmCallback) => uiController.TriggerQuantityPrompt(message, maxQuantity, onConfirmCallback);

        public void ToggleMenuPanel() => uiController.ToggleMenuPanel();

        public void OpenWindow(Tab windowToOpen) => uiController.OpenWindow(windowToOpen);

        public void FadeOutScreen() => uiController.FadeOutScreen();

        public void FadeInScreen() => uiController.FadeInScreen();

        public void OnFadeInComplete() => uiController.OnFadeInComplete();

        public void ResetFadeDefaults() => uiController.ResetFadeDefaults();

        public void RenderInventory() => uiController.RenderInventory();

        public void ToggleInventoryPanel() => uiController.ToggleInventoryPanel();

        public void DisplayItemInfo(ItemData data) => uiController.DisplayItemInfo(data);

        public void ClockUpdate(GameTimestamp timestamp) => uiController.ClockUpdate(timestamp);

        public void RenderPlayerStats() => uiController.RenderPlayerStats();

        public void OpenShop(List<ItemData> shopItems) => uiController.OpenShop(shopItems);

        public void ToggleRelationshipPanel() => uiController.ToggleRelationshipPanel();

        public void InteractPrompt(Transform item, string message, float offsetY, float offsetZ)
        {
            if (!interactBubble.gameObject.activeInHierarchy)
                interactBubble.gameObject.SetActive(true);

            interactBubble.transform.position = item.transform.position + new Vector3(0, offsetY, offsetZ);
            interactBubble.Display(message);
        }

        public void DeactivateInteractPrompt()
        {
            if (interactBubble.gameObject.activeInHierarchy)
                interactBubble.gameObject.SetActive(false);
        }
    }
}