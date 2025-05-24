using UnityEngine;

public class AnimEventsTools : MonoBehaviour
{
    // this script contains the anim events for the spawing of tools when the animation is played

    // this is the original objects of the tools 
    public GameObject wateringCan;
    public GameObject Hoe;

    private GameObject newWateringCan;
    private GameObject newHoe;

    public Transform handPoint;
    public void SpawnWaterCan()
    {
        newWateringCan = Instantiate(wateringCan, handPoint);
    }

    public void DespawnWaterCan()
    {
        Destroy(newWateringCan);
    }

    public void SpawnHoe()
    {
        newHoe = Instantiate(Hoe, handPoint);
    }

    public void DespawnHoe()
    {
        Destroy(newHoe);
    }
}
