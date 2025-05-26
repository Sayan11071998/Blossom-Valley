using UnityEngine;

public enum TransitionType { Location, Sleep }

public static class TransitionFactory
{
    public static ITransitionCommand CreateTransition(TransitionType type, object parameters = null)
    {
        return type switch
        {
            TransitionType.Location => new LocationTransitionCommand((SceneTransitionManager.Location)parameters),
            TransitionType.Sleep => new SleepTransitionCommand(),
            _ => throw new System.ArgumentException("Unknown transition type")
        };
    }
}