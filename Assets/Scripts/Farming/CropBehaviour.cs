using UnityEngine;

public class CropBehaviour : MonoBehaviour
{
    int landID;

    SeedData seedToGrow;

    [Header("Stages of Life")]
    public GameObject seed;
    public GameObject wilted;
    private GameObject seedling;
    private GameObject harvestable;

    int growth;
    int maxGrowth;

    int maxHealth = GameTimestamp.HoursToMinutes(48);
    int health;

    public enum CropState
    {
        Seed,
        Seedling,
        Harvestable,
        Wilted
    }

    public CropState cropState;

    public void Plant(int landID, SeedData seedToGrow)
    {
        LoadCrop(landID, seedToGrow, CropState.Seed, 0, 0);
        LandManager.Instance.RegisterCrop(landID, seedToGrow, cropState, growth, health);
    }

    public void LoadCrop(int landID, SeedData seedToGrow, CropState cropState, int growth, int health)
    {
        this.landID = landID;
        //Save the seed information
        this.seedToGrow = seedToGrow;

        //Instantiate the seedling and harvestable GameObjects
        seedling = Instantiate(seedToGrow.seedling, transform);

        //Access the crop item data
        ItemData cropToYield = seedToGrow.cropToYield;

        //Instantiate the harvestable crop
        harvestable = Instantiate(cropToYield.gameModel, transform);

        //Convert Days To Grow into hours
        int hoursToGrow = GameTimestamp.DaysToHours(seedToGrow.daysToGrow);
        //Convert it to minutes
        maxGrowth = GameTimestamp.HoursToMinutes(hoursToGrow);

        this.growth = growth;
        this.health = health;

        //Check if it is regrowable
        if (seedToGrow.regrowable)
        {
            //Get the RegrowableHarvestBehaviour from the GameObject
            RegrowableHarvestBehaviour regrowableHarvest = harvestable.GetComponent<RegrowableHarvestBehaviour>();

            //Initialise the harvestable 
            regrowableHarvest.SetParent(this);
        }

        //Set the initial state to Seed
        SwitchState(cropState);

    }

    public void Grow()
    {
        growth++;

        if (health < maxHealth)
        {
            health++;
        }

        if (growth >= maxGrowth / 2 && cropState == CropState.Seed)
        {
            SwitchState(CropState.Seedling);
        }

        if (growth >= maxGrowth && cropState == CropState.Seedling)
        {
            SwitchState(CropState.Harvestable);
        }

        LandManager.Instance.OnCropStateChange(landID, cropState, growth, health);
    }

    public void Wither()
    {
        health--;
        if (health <= 0 && cropState != CropState.Seed)
        {
            SwitchState(CropState.Wilted);
        }

        LandManager.Instance.OnCropStateChange(landID, cropState, growth, health);
    }

    void SwitchState(CropState stateToSwitch)
    {
        seed.SetActive(false);
        seedling.SetActive(false);
        harvestable.SetActive(false);
        wilted.SetActive(false);

        switch (stateToSwitch)
        {
            case CropState.Seed:
                seed.SetActive(true);
                break;
            case CropState.Seedling:
                seedling.SetActive(true);
                health = maxHealth;
                break;
            case CropState.Harvestable:
                harvestable.SetActive(true);
                if (!seedToGrow.regrowable)
                {
                    harvestable.transform.parent = null;
                    RemoveCrop();
                }
                break;
            case CropState.Wilted:
                wilted.SetActive(true);
                break;
        }

        cropState = stateToSwitch;
    }

    public void RemoveCrop()
    {
        LandManager.Instance.DeregisterCrop(landID);
        Destroy(gameObject);
    }

    public void Regrow()
    {
        int hoursToRegrow = GameTimestamp.DaysToHours(seedToGrow.daysToRegrow);
        growth = maxGrowth - GameTimestamp.HoursToMinutes(hoursToRegrow);
        SwitchState(CropState.Seedling);
    }
}