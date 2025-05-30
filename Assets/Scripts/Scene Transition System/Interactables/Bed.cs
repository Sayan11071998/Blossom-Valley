using BlossomValley.InventorySystem;
using BlossomValley.UISystem;
using BlossomValley.Utilities;

namespace BlossomValley.SceneTransitionSystem
{
    public class Bed : InteractableObject
    {
        public override void Pickup() => UIManager.Instance.TriggerYesNoPrompt("Do you want to sleep?", GameStateManager.Instance.Sleep);
    }
}