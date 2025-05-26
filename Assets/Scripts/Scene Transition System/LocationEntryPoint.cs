using UnityEngine;

public class LocationEntryPoint : MonoBehaviour
{
    [SerializeField]
    SceneTransitionManager.Location locationToSwitch;

    private void OnTriggerEnter(Collider other)
    {
        //Check if the collider belongs to the player
        if(other.tag == "Player")
        {
            //Create and execute location transition command
            ITransitionCommand transition = TransitionFactory.CreateTransition(TransitionType.Location, locationToSwitch);
            transition.Execute();
        }
    }
}