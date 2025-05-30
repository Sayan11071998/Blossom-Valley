using UnityEngine;
using BlossomValley.InventorySystem;

namespace BlossomValley.CropSystem
{
    public class CropContext
    {
        public int landID { get; private set; }
        public SeedData seedToGrow { get; private set; }
        public int growth { get; set; }
        public int health { get; set; }
        public int maxGrowth { get; private set; }
        public int maxHealth { get; private set; }

        public CropBehaviour cropBehaviour { get; private set; }
        public GameObject seed { get; private set; }
        public GameObject seedling { get; private set; }
        public GameObject harvestable { get; private set; }
        public GameObject wilted { get; private set; }
        public Transform transform { get; private set; }

        private ICropState currentState;
        private CropStateFactory stateFactory;

        public CropContext(CropBehaviour cropBehaviourToSet)
        {
            cropBehaviour = cropBehaviourToSet;
            transform = cropBehaviourToSet.transform;
            stateFactory = new CropStateFactory();
            maxHealth = GameTimestamp.HoursToMinutes(48);
        }

        public void Initialize(int landIDToSet, SeedData seedToGrowToSet, GameObject seedToSet, GameObject wiltedToSet)
        {
            landID = landIDToSet;
            seedToGrow = seedToGrowToSet;
            seed = seedToSet;
            wilted = wiltedToSet;

            seedling = Object.Instantiate(seedToGrowToSet.seedling, transform);

            ItemData cropToYield = seedToGrowToSet.cropToYield;
            harvestable = Object.Instantiate(cropToYield.gameModel, transform);

            int hoursToGrow = GameTimestamp.DaysToHours(seedToGrowToSet.daysToGrow);
            maxGrowth = GameTimestamp.HoursToMinutes(hoursToGrow);

            if (seedToGrowToSet.regrowable)
            {
                RegrowableHarvestBehaviour regrowableHarvest = harvestable.GetComponent<RegrowableHarvestBehaviour>();
                regrowableHarvest.SetParent(cropBehaviour);
            }
        }

        public void SetState(CropBehaviour.CropState stateType)
        {
            ICropState newState = stateFactory.CreateState(stateType);

            if (currentState != null)
                currentState.ExitState(this);

            currentState = newState;
            currentState.EnterState(this);
        }

        public void LoadState(CropBehaviour.CropState stateType, int growthValue, int healthValue)
        {
            growth = growthValue;
            health = healthValue;

            SetState(stateType);
        }

        public CropBehaviour.CropState GetCurrentStateType() => currentState?.GetStateType() ?? CropBehaviour.CropState.Seed;

        public void Grow() => currentState?.Grow(this);

        public void Wither() => currentState?.Wither(this);

        public void DeactivateAllVisuals()
        {
            seed.SetActive(false);
            seedling.SetActive(false);
            harvestable.SetActive(false);
            wilted.SetActive(false);
        }

        public void NotifyLandManager() => LandManager.Instance.OnCropStateChange(landID, GetCurrentStateType(), growth, health);

        public void RemoveCrop()
        {
            LandManager.Instance.DeregisterCrop(landID);
            Object.Destroy(cropBehaviour.gameObject);
        }

        public void Regrow()
        {
            int hoursToRegrow = GameTimestamp.DaysToHours(seedToGrow.daysToRegrow);
            growth = maxGrowth - GameTimestamp.HoursToMinutes(hoursToRegrow);
            SetState(CropBehaviour.CropState.Seedling);
        }
    }
}