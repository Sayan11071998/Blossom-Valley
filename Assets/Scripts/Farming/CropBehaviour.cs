using UnityEngine;

public class CropBehaviour : MonoBehaviour
{
    public GameObject seed;
    private GameObject seedling;
    private GameObject harvestable;

    public enum CropState
    {
        Seed,
        Seedling,
        Harvestable
    }

    public CropState cropState;

    private SeedData seedToGrow;

    public void Plant(SeedData seedToGrow)
    {
        this.seedToGrow = seedToGrow;

        seedling = Instantiate(seedToGrow.seedling, transform);

        ItemData cropToYield = seedToGrow.cropToYield;

        harvestable = Instantiate(cropToYield.gameModel, transform);

        SwitchState(CropState.Seed);
    }

    public void Grow()
    {

    }

    private void SwitchState(CropState stateToSwitch)
    {
        seed.SetActive(false);
        seedling.SetActive(false);
        harvestable.SetActive(false);

        switch (stateToSwitch)
        {
            case CropState.Seed:
                seed.SetActive(true);
                break;
            case CropState.Seedling:
                seedling.SetActive(true);
                break;
            case CropState.Harvestable:
                harvestable.SetActive(true);
                break;
        }

        cropState = stateToSwitch;
    }
}