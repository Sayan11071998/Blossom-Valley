using BlossomValley.InventorySystem;

public class Bed : InteractableObject
{
    public override void Pickup() => UIManager.Instance.TriggerYesNoPrompt("Do you want to sleep?", GameStateManager.Instance.Sleep);
}