using BlossomValley.UISystem;
using UnityEngine;
using UnityEngine.Events;

namespace BlossomValley.InventorySystem
{
    public class InteractableObject : MonoBehaviour
    {
        [SerializeField] protected string interactText = "Interact";
        [SerializeField] protected float offsetY = 1.5f;
        [SerializeField] protected float offsetZ = 0f;

        public ItemData item;
        public UnityEvent onInteract = new UnityEvent();

        public virtual void Pickup()
        {
            onInteract?.Invoke();
            InventoryManager.Instance.EquipHandSlot(item);
            InventoryManager.Instance.RenderHand();
            UIManager.Instance.DeactivateInteractPrompt();
            Destroy(gameObject);
        }

        public virtual void OnHover() => UIManager.Instance.InteractPrompt(transform, interactText, offsetY, offsetZ);

        public virtual void OnMoveAway() => UIManager.Instance.DeactivateInteractPrompt();
    }
}