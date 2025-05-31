using UnityEngine;
using BlossomValley.CameraSystem;

namespace BlossomValley.UISystem
{
    public class WorldUI : MonoBehaviour
    {
        private Transform cameraPos;

        private void Start() => cameraPos = FindFirstObjectByType<CameraController>().transform;

        private void Update() => transform.rotation = cameraPos.rotation;
    }
}