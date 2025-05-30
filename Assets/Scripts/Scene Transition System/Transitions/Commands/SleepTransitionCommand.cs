namespace BlossomValley.SceneTransitionSystem
{
    public class SleepTransitionCommand : ITransitionCommand
    {
        public void Execute() => GameStateManager.Instance.Sleep();
    }
}