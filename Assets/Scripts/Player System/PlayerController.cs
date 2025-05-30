using UnityEngine;
using BlossomValley.InventorySystem;

public class PlayerController
{
    private PlayerModel playerModel;
    private PlayerView playerView;

    private LandView selectedLand;
    private InteractableObject selectedInteractable;

    public PlayerModel PlayerModel => playerModel;

    public PlayerController(PlayerModel modelToSet, PlayerView viewToSet)
    {
        playerModel = modelToSet;
        playerView = viewToSet;
    }

    public void HandleMovement(float horizontal, float vertical, bool isSprinting)
    {
        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;
        float speed = isSprinting ? playerModel.RunSpeed : playerModel.WalkSpeed;
        Vector3 velocity = speed * Time.deltaTime * dir;

        if (playerView.IsGrounded())
            velocity.y = 0;

        velocity.y -= Time.deltaTime * playerModel.Gravity;
        playerView.Move(velocity, dir, isSprinting);
    }

    public void SelectLand(LandView land)
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