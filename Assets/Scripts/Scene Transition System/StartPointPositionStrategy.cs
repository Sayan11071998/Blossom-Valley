using UnityEngine;

public class StartPointPositionStrategy : IPlayerPositionStrategy
{
    public Transform GetPlayerPosition(SceneTransitionManager.Location fromLocation)
    {
        return LocationManager.Instance.GetPlayerStartingPosition(fromLocation);
    }
}
