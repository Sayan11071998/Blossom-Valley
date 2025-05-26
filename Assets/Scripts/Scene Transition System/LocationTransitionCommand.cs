using UnityEngine;

public class LocationTransitionCommand : ITransitionCommand
{
    private SceneTransitionManager.Location targetLocation;
    
    public LocationTransitionCommand(SceneTransitionManager.Location target)
    {
        targetLocation = target;
    }
    
    public void Execute()
    {
        SceneTransitionManager.Instance.SwitchLocation(targetLocation);
    }
}