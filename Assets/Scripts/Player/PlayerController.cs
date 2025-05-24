public class PlayerController
{
    private PlayerModel model;
    private PlayerView view;

    private Land selectedLand;
    private InteractableObject selectedInteractable;

    private float walkSpeed = 4f;
    private float runSpeed = 8f;
    private float gravity = 9.81f;

    public PlayerController(PlayerModel model, PlayerView view)
    {
        this.model = model;
        this.view = view;
    }

    public void HandleMovement(float horizontal, float vertical, bool isSprinting)
    {
        UnityEngine.Vector3 dir = new UnityEngine.Vector3(horizontal, 0f, vertical).normalized;
        float speed = isSprinting ? runSpeed : walkSpeed;
        UnityEngine.Vector3 velocity = speed * UnityEngine.Time.deltaTime * dir;

        if (view.IsGrounded())
        {
            velocity.y = 0;
        }
        velocity.y -= UnityEngine.Time.deltaTime * gravity;

        view.Move(velocity, dir, isSprinting);
    }

    public void SelectLand(Land land)
    {
        if (selectedLand != null)
        {
            selectedLand.Select(false);
        }
        selectedLand = land;
        if (land != null)
        {
            land.Select(true);
        }
    }

    public void SelectInteractable(InteractableObject interactable)
    {
        selectedInteractable = interactable;
    }

    public void Deselect()
    {
        if (selectedLand != null)
        {
            selectedLand.Select(false);
            selectedLand = null;
        }
        selectedInteractable = null;
    }

    public void Interact()
    {
        if (InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Item))
        {
            return;
        }
        if (selectedLand != null)
        {
            selectedLand.Interact();
        }
        else
        {
            UnityEngine.Debug.Log("Not on any land!");
        }
    }

    public void ItemInteract()
    {
        if (selectedInteractable != null)
        {
            selectedInteractable.Pickup();
        }
    }

    public void ItemKeep()
    {
        if (InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Item))
        {
            InventoryManager.Instance.HandToInventory(InventorySlot.InventoryType.Item);
        }
    }
}