public class LandController : ITimeTracker
{
    private LandModel landModel;
    private LandView landView;

    public LandController(LandModel modelToSet, LandView viewToSet)
    {
        landModel = modelToSet;
        landView = viewToSet;

        landModel.OnLandStatusChanged += HandleLandStatusChanged;
        landModel.OnObstacleStatusChanged += HandleObstacleStatusChanged;
        landModel.OnSelectionChanged += HandleSelectionChanged;
        landModel.OnTimeWateredChanged += HandleTimeWateredChanged;
        landModel.OnCropStateChanged += HandleCropStateChanged;

        TimeManager.Instance.RegisterTracker(this);
    }

    public void Initialize()
    {
        landModel.SetLandStatus(LandModel.LandStatus.Soil);
        landModel.SetSelection(false);
    }

    public void LoadLandData(LandModel.LandStatus landStatusToSwitch, GameTimestamp lastWatered, LandModel.FarmObstacleStatus obstacleStatusToSwitch)
    {
        landModel.SetLandStatus(landStatusToSwitch);
        landModel.SetTimeWatered(lastWatered);
        landModel.SetObstacleStatus(obstacleStatusToSwitch);
    }

    public void SwitchLandStatus(LandModel.LandStatus statusToSwitch)
    {
        landModel.SetLandStatus(statusToSwitch);

        if (statusToSwitch == LandModel.LandStatus.Watered)
            landModel.SetTimeWatered(TimeManager.Instance.GetGameTimestamp());

        LandManager.Instance.OnLandStateChange(landModel.id, landModel.landStatus, landModel.timeWatered, landModel.obstacleStatus);
    }

    public void SetObstacleStatus(LandModel.FarmObstacleStatus statusToSwitch)
    {
        landModel.SetObstacleStatus(statusToSwitch);
        LandManager.Instance.OnLandStateChange(landModel.id, landModel.landStatus, landModel.timeWatered, landModel.obstacleStatus);
    }

    public void Select(bool toggle) => landModel.SetSelection(toggle);

    public void Interact()
    {
        ItemData toolSlot = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Tool);

        if (!InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Tool)) return;

        EquipmentData equipmentTool = toolSlot as EquipmentData;

        if (equipmentTool != null)
        {
            HandleEquipmentInteraction(equipmentTool);
            return;
        }

        SeedData seedTool = toolSlot as SeedData;

        if (seedTool != null)
            HandleSeedInteraction(seedTool);
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
                if (landModel.CanUseWateringCan())
                    SwitchLandStatus(LandModel.LandStatus.Watered);
                break;

            case EquipmentData.ToolType.Shovel:
                if (landModel.hasCrop)
                {
                    landView.RemoveCrop();
                    landModel.SetCropState(false);
                }
                if (landModel.CanRemoveObstacle(LandModel.FarmObstacleStatus.Weeds))
                    SetObstacleStatus(LandModel.FarmObstacleStatus.None);
                break;

            case EquipmentData.ToolType.Axe:
                if (landModel.CanRemoveObstacle(LandModel.FarmObstacleStatus.Wood))
                    SetObstacleStatus(LandModel.FarmObstacleStatus.None);
                break;

            case EquipmentData.ToolType.Pickaxe:
                if (landModel.CanRemoveObstacle(LandModel.FarmObstacleStatus.Rock))
                    SetObstacleStatus(LandModel.FarmObstacleStatus.None);
                break;
        }
    }

    private void HandleSeedInteraction(SeedData seedTool)
    {
        if (landModel.CanPlantSeed())
        {
            CropBehaviour cropPlanted = landView.SpawnCrop();
            cropPlanted.Plant(landModel.id, seedTool);
            landModel.SetCropState(true);
            InventoryManager.Instance.ConsumeItem(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool));
        }
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        if (landModel.landStatus == LandModel.LandStatus.Watered)
        {
            int hoursElapsed = GameTimestamp.CompareTimestamps(landModel.timeWatered, timestamp);

            if (landModel.hasCrop)
                landView.GrowCrop();

            if (hoursElapsed > 24)
                SwitchLandStatus(LandModel.LandStatus.Farmland);
        }

        if (landModel.landStatus != LandModel.LandStatus.Watered && landModel.hasCrop)
            landView.WitherCrop();
    }

    private void HandleLandStatusChanged(LandModel.LandStatus newStatus) => landView.UpdateLandVisuals(newStatus);

    private void HandleObstacleStatusChanged(LandModel.FarmObstacleStatus newStatus) => landView.UpdateObstacleVisuals(newStatus);

    private void HandleSelectionChanged(bool isSelected) => landView.UpdateSelectionVisuals(isSelected);

    private void HandleTimeWateredChanged(GameTimestamp timestamp) { }

    private void HandleCropStateChanged(bool hasCrop) { }

    public void Dispose()
    {
        if (landModel != null)
        {
            landModel.OnLandStatusChanged -= HandleLandStatusChanged;
            landModel.OnObstacleStatusChanged -= HandleObstacleStatusChanged;
            landModel.OnSelectionChanged -= HandleSelectionChanged;
            landModel.OnTimeWateredChanged -= HandleTimeWateredChanged;
            landModel.OnCropStateChanged -= HandleCropStateChanged;
        }

        if (TimeManager.Instance != null)
            TimeManager.Instance.UnregisterTracker(this);
    }

    public int Id => landModel.id;
    public LandModel.LandStatus LandStatus => landModel.landStatus;
    public LandModel.FarmObstacleStatus ObstacleStatus => landModel.obstacleStatus;
    public GameTimestamp TimeWatered => landModel.timeWatered;
    public bool HasCrop => landModel.hasCrop;
}