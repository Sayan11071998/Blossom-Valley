using UnityEngine;
using BlossomValley.InventorySystem;

namespace BlossomValley.AnimalSystem
{
    public class Feedbox : InteractableObject
    {
        public int id;
        public GameObject displayFeed;

        private bool containsFeed;

        public override void Pickup()
        {
            if (CanFeed())
                FeedAnimal();
        }

        private void FeedAnimal()
        {
            InventoryManager.Instance.ConsumeItem(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item));
            SetFeedState(true);
            FindAnyObjectByType<AnimalFeedManager>().FeedAnimal(id);

        }

        public void SetFeedState(bool feed)
        {
            containsFeed = feed;
            displayFeed.SetActive(feed);
        }

        private bool CanFeed()
        {
            ItemData handSlotItem = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Item);

            if (handSlotItem == null || containsFeed) return false;
            if (handSlotItem.name != item.name) return false;

            return true;
        }
    }
}