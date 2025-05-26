using UnityEngine;

public class CropContext
{
    // Crop data
    public int landID { get; private set; }
    public SeedData seedToGrow { get; private set; }
    public int growth { get; set; }
    public int health { get; set; }
    public int maxGrowth { get; private set; }
    public int maxHealth { get; private set; }

    // GameObjects for visual representation
    public GameObject seed { get; private set; }
    public GameObject seedling { get; private set; }
    public GameObject harvestable { get; private set; }
    public GameObject wilted { get; private set; }
    public Transform transform { get; private set; }

    // State management
    private ICropState currentState;
    private CropStateFactory stateFactory;

    // Reference to the MonoBehaviour for destroy operations
    public CropBehaviour cropBehaviour { get; private set; }

    public CropContext(CropBehaviour cropBehaviour)
    {
        this.cropBehaviour = cropBehaviour;
        this.transform = cropBehaviour.transform;
        stateFactory = new CropStateFactory();
        maxHealth = GameTimestamp.HoursToMinutes(48);
    }

    public void Initialize(int landID, SeedData seedToGrow, GameObject seed, GameObject wilted)
    {
        this.landID = landID;
        this.seedToGrow = seedToGrow;
        this.seed = seed;
        this.wilted = wilted;

        // Create seedling and harvestable GameObjects
        this.seedling = Object.Instantiate(seedToGrow.seedling, transform);
        
        ItemData cropToYield = seedToGrow.cropToYield;
        this.harvestable = Object.Instantiate(cropToYield.gameModel, transform);

        // Calculate max growth
        int hoursToGrow = GameTimestamp.DaysToHours(seedToGrow.daysToGrow);
        maxGrowth = GameTimestamp.HoursToMinutes(hoursToGrow);

        // Setup regrowable behavior if needed
        if (seedToGrow.regrowable)
        {
            RegrowableHarvestBehaviour regrowableHarvest = harvestable.GetComponent<RegrowableHarvestBehaviour>();
            regrowableHarvest.SetParent(cropBehaviour);
        }
    }

    public void SetState(CropBehaviour.CropState stateType)
    {
        ICropState newState = stateFactory.CreateState(stateType);
        
        if (currentState != null)
        {
            currentState.ExitState(this);
        }

        currentState = newState;
        currentState.EnterState(this);
    }

    public void LoadState(CropBehaviour.CropState stateType, int growth, int health)
    {
        this.growth = growth;
        this.health = health;
        SetState(stateType);
    }

    public CropBehaviour.CropState GetCurrentStateType()
    {
        return currentState?.GetStateType() ?? CropBehaviour.CropState.Seed;
    }

    public void Grow()
    {
        currentState?.Grow(this);
    }

    public void Wither()
    {
        currentState?.Wither(this);
    }

    public void DeactivateAllVisuals()
    {
        seed.SetActive(false);
        seedling.SetActive(false);
        harvestable.SetActive(false);
        wilted.SetActive(false);
    }

    public void NotifyLandManager()
    {
        LandManager.Instance.OnCropStateChange(landID, GetCurrentStateType(), growth, health);
    }

    public void RemoveCrop()
    {
        LandManager.Instance.DeregisterCrop(landID);
        Object.Destroy(cropBehaviour.gameObject);
    }

    public void Regrow()
    {
        // Reset the growth 
        int hoursToRegrow = GameTimestamp.DaysToHours(seedToGrow.daysToRegrow);
        growth = maxGrowth - GameTimestamp.HoursToMinutes(hoursToRegrow);
        
        // Switch back to seedling state
        SetState(CropBehaviour.CropState.Seedling);
    }
}