using UnityEngine;

/// <summary>
/// Pure C# controller handling the business logic for land operations
/// </summary>
public class LandController : ITimeTracker
{
    private LandModel model;
    private LandView view;

    public LandController(LandModel landModel, LandView landView)
    {
        model = landModel;
        view = landView;

        // Subscribe to model events
        model.OnLandStatusChanged += HandleLandStatusChanged;
        model.OnObstacleStatusChanged += HandleObstacleStatusChanged;
        model.OnSelectionChanged += HandleSelectionChanged;
        model.OnTimeWateredChanged += HandleTimeWateredChanged;
        model.OnCropStateChanged += HandleCropStateChanged;

        // Register for time updates
        TimeManager.Instance.RegisterTracker(this);
    }

    public void Initialize()
    {
        // Set initial state
        model.SetLandStatus(LandModel.LandStatus.Soil);
        model.SetSelection(false);
    }

    public void LoadLandData(LandModel.LandStatus landStatusToSwitch, GameTimestamp lastWatered, LandModel.FarmObstacleStatus obstacleStatusToSwitch)
    {
        model.SetLandStatus(landStatusToSwitch);
        model.SetTimeWatered(lastWatered);
        model.SetObstacleStatus(obstacleStatusToSwitch);
    }

    public void SwitchLandStatus(LandModel.LandStatus statusToSwitch)
    {
        model.SetLandStatus(statusToSwitch);

        if (statusToSwitch == LandModel.LandStatus.Watered)
        {
            model.SetTimeWatered(TimeManager.Instance.GetGameTimestamp());
        }

        // Notify LandManager of state change
        LandManager.Instance.OnLandStateChange(model.id, model.landStatus, model.timeWatered, model.obstacleStatus);
    }

    public void SetObstacleStatus(LandModel.FarmObstacleStatus statusToSwitch)
    {
        model.SetObstacleStatus(statusToSwitch);

        // Notify LandManager of state change
        LandManager.Instance.OnLandStateChange(model.id, model.landStatus, model.timeWatered, model.obstacleStatus);
    }

    public void Select(bool toggle)
    {
        model.SetSelection(toggle);
    }

    public void Interact()
    {
        // Check the player's tool slot
        ItemData toolSlot = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Tool);

        // If there's nothing equipped, return
        if (!InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Tool))
        {
            return;
        }

        // Try casting the itemdata in the toolslot as EquipmentData
        EquipmentData equipmentTool = toolSlot as EquipmentData;

        // Check if it is of type EquipmentData 
        if (equipmentTool != null)
        {
            HandleEquipmentInteraction(equipmentTool);
            return;
        }

        // Try casting the itemdata in the toolslot as SeedData
        SeedData seedTool = toolSlot as SeedData;

        if (seedTool != null)
        {
            HandleSeedInteraction(seedTool);
        }
    }

    private void HandleEquipmentInteraction(EquipmentData equipmentTool)
    {
        EquipmentData.ToolType toolType = equipmentTool.toolType;

        switch (toolType)
        {
            case EquipmentData.ToolType.Hoe:
                SwitchLandStatus(LandModel.LandStatus.Farmland);
                break;

            case EquipmentData.ToolType.WateringCan:
                if (model.CanUseWateringCan())
                {
                    SwitchLandStatus(LandModel.LandStatus.Watered);
                }
                break;

            case EquipmentData.ToolType.Shovel:
                // Remove the crop from the land
                if (model.hasCrop)
                {
                    view.RemoveCrop();
                    model.SetCropState(false);
                }

                // Remove weed obstacle
                if (model.CanRemoveObstacle(LandModel.FarmObstacleStatus.Weeds))
                {
                    SetObstacleStatus(LandModel.FarmObstacleStatus.None);
                }
                break;

            case EquipmentData.ToolType.Axe:
                // Remove wood obstacle
                if (model.CanRemoveObstacle(LandModel.FarmObstacleStatus.Wood))
                {
                    SetObstacleStatus(LandModel.FarmObstacleStatus.None);
                }
                break;

            case EquipmentData.ToolType.Pickaxe:
                // Remove rock obstacle
                if (model.CanRemoveObstacle(LandModel.FarmObstacleStatus.Rock))
                {
                    SetObstacleStatus(LandModel.FarmObstacleStatus.None);
                }
                break;
        }
    }

    private void HandleSeedInteraction(SeedData seedTool)
    {
        // Conditions for the player to be able to plant a seed
        if (model.CanPlantSeed())
        {
            CropBehaviour cropPlanted = view.SpawnCrop();
            // Plant it with the seed's information
            cropPlanted.Plant(model.id, seedTool);
            model.SetCropState(true);

            // Consume the item
            InventoryManager.Instance.ConsumeItem(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool));
        }
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        // Checked if 24 hours has passed since last watered
        if (model.landStatus == LandModel.LandStatus.Watered)
        {
            // Hours since the land was watered
            int hoursElapsed = GameTimestamp.CompareTimestamps(model.timeWatered, timestamp);
            Debug.Log(hoursElapsed + " hours since this was watered");

            // Grow the planted crop, if any
            if (model.hasCrop)
            {
                view.GrowCrop();
            }

            if (hoursElapsed > 24)
            {
                // Dry up (Switch back to farmland)
                SwitchLandStatus(LandModel.LandStatus.Farmland);
            }
        }

        // Handle the wilting of the plant when the land is not watered
        if (model.landStatus != LandModel.LandStatus.Watered && model.hasCrop)
        {
            view.WitherCrop();
        }
    }

    // Event handlers for model changes
    private void HandleLandStatusChanged(LandModel.LandStatus newStatus)
    {
        view.UpdateLandVisuals(newStatus);
    }

    private void HandleObstacleStatusChanged(LandModel.FarmObstacleStatus newStatus)
    {
        view.UpdateObstacleVisuals(newStatus);
    }

    private void HandleSelectionChanged(bool isSelected)
    {
        view.UpdateSelectionVisuals(isSelected);
    }

    private void HandleTimeWateredChanged(GameTimestamp timestamp)
    {
        // Could be used for additional logic if needed
    }

    private void HandleCropStateChanged(bool hasCrop)
    {
        // Could be used for additional logic if needed
    }

    public void Dispose()
    {
        // Unsubscribe from events
        if (model != null)
        {
            model.OnLandStatusChanged -= HandleLandStatusChanged;
            model.OnObstacleStatusChanged -= HandleObstacleStatusChanged;
            model.OnSelectionChanged -= HandleSelectionChanged;
            model.OnTimeWateredChanged -= HandleTimeWateredChanged;
            model.OnCropStateChanged -= HandleCropStateChanged;
        }

        // Unregister from time manager
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.UnregisterTracker(this);
        }
    }

    // Public getters for accessing model data (for LandManager)
    public int Id => model.id;
    public LandModel.LandStatus LandStatus => model.landStatus;
    public LandModel.FarmObstacleStatus ObstacleStatus => model.obstacleStatus;
    public GameTimestamp TimeWatered => model.timeWatered;
    public bool HasCrop => model.hasCrop;
}