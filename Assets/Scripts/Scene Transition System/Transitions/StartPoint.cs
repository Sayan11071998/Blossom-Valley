using UnityEngine;

namespace BlossomValley.SceneTransitionSystem
{
    [System.Serializable]
    public struct StartPoint
    {
        public SceneTransitionManager.Location enteringFrom;

        public Transform playerStart;
    }
}