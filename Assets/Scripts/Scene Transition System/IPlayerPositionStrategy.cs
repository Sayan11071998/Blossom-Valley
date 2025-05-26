using UnityEngine;

public interface IPlayerPositionStrategy
{
    Transform GetPlayerPosition(SceneTransitionManager.Location fromLocation);
}