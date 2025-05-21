using UnityEngine;

public class LocationEntryPoint : MonoBehaviour
{
    [SerializeField] SceneTransitionManager.Location locationToSwitch;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneTransitionManager.Instance.SwitchLocation(locationToSwitch);
        }
    }
}