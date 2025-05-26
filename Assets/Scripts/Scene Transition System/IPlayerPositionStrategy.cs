using UnityEngine;

public interface IPlayerPositionStrategy
{
    public Transform GetPlayerPosition(SceneTransitionManager.Location fromLocation);
}