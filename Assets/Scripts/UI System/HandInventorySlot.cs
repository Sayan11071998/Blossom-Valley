using UnityEngine.EventSystems;
using BlossomValley.InventorySystem;

namespace BlossomValley.UISystem
{
    public class HandInventorySlot : InventorySlot
    {
        public override void OnPointerClick(PointerEventData eventData) => InventoryManager.Instance.HandToInventory(inventoryType);
    }
}