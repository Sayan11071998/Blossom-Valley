using UnityEngine;

namespace BlossomValley.SceneTransitionSystem
{
    public interface IPlayerPositionStrategy
    {
        public Transform GetPlayerPosition(SceneTransitionManager.Location fromLocation);
    }
}