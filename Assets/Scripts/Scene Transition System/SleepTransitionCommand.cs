using UnityEngine;

public class SleepTransitionCommand : ITransitionCommand
{
    public void Execute()
    {
        GameStateManager.Instance.Sleep();
    }
}