using BlossomValley.InventorySystem;
using BlossomValley.UISystem;
using BlossomValley.Utilities;

namespace BlossomValley.SceneTransitionSystem
{
    public class Bed : InteractableObject
    {
        public override void Pickup() => UIManager.Instance.TriggerYesNoPrompt(GameString.WantToSleepPrompt, GameStateManager.Instance.Sleep);
    }
}