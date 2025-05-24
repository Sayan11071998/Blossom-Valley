using UnityEngine;

public class PlayerController
{
    private PlayerModel model;
    private PlayerView view;

    private Land selectedLand;
    private InteractableObject selectedInteractable;

    public PlayerModel PlayerModel => model;

    public PlayerController(PlayerModel model, PlayerView view)
    {
        this.model = model;
        this.view = view;
    }

    public void HandleMovement(float horizontal, float vertical, bool isSprinting)
    {
        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;
        float speed = isSprinting ? model.RunSpeed : model.WalkSpeed;
        Vector3 velocity = speed * Time.deltaTime * dir;

        if (view.IsGrounded())
            velocity.y = 0;

        velocity.y -= Time.deltaTime * model.Gravity;
        view.Move(velocity, dir, isSprinting);
    }

    public void SelectLand(Land land)
    {
        if (selectedLand != null)
            selectedLand.Select(false);

        selectedLand = land;

        if (land != null)
            land.Select(true);
    }

    public void SelectInteractable(InteractableObject interactable) => selectedInteractable = interactable;

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
        if (InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Item)) return;

        if (selectedLand != null)
            selectedLand.Interact();
    }

    public void ItemInteract()
    {
        if (selectedInteractable != null)
            selectedInteractable.Pickup();
    }

    public void ItemKeep()
    {
        if (InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Item))
            InventoryManager.Instance.HandToInventory(InventorySlot.InventoryType.Item);
    }
}