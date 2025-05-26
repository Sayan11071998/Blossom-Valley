using UnityEngine;

public class StartPointPositionStrategy : IPlayerPositionStrategy
{
    public Transform GetPlayerPosition(SceneTransitionManager.Location fromLocation) => LocationManager.Instance.GetPlayerStartingPosition(fromLocation);
}