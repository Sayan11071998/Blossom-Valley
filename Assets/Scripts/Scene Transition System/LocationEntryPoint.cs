using BlossomValley.GameStrings;
using UnityEngine;

public class LocationEntryPoint : MonoBehaviour
{
    [SerializeField] private SceneTransitionManager.Location locationToSwitch;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GameString.Player))
        {
            ITransitionCommand transition = TransitionFactory.CreateTransition(TransitionType.Location, locationToSwitch);
            transition.Execute();
        }
    }
}