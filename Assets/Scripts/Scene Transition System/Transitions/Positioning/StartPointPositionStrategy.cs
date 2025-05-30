using UnityEngine;

namespace BlossomValley.SceneTransitionSystem
{
    public class StartPointPositionStrategy : IPlayerPositionStrategy
    {
        public Transform GetPlayerPosition(SceneTransitionManager.Location fromLocation) => LocationManager.Instance.GetPlayerStartingPosition(fromLocation);
    }
}