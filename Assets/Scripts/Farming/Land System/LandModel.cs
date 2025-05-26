using System;

/// <summary>
/// Pure C# model representing the data and state of a land plot
/// </summary>
[Serializable]
public class LandModel
{
    public int id;
    
    public enum LandStatus
    {
        Soil, Farmland, Watered
    }

    public enum FarmObstacleStatus 
    { 
        None, Rock, Wood, Weeds
    }

    // Land properties
    public LandStatus landStatus;
    public FarmObstacleStatus obstacleStatus;
    public bool isSelected;
    
    // Time tracking
    public GameTimestamp timeWatered;
    
    // Crop state
    public bool hasCrop;
    
    // Events for notifying changes
    public event Action<LandStatus> OnLandStatusChanged;
    public event Action<FarmObstacleStatus> OnObstacleStatusChanged;
    public event Action<bool> OnSelectionChanged;
    public event Action<GameTimestamp> OnTimeWateredChanged;
    public event Action<bool> OnCropStateChanged;

    public LandModel(int landId)
    {
        id = landId;
        landStatus = LandStatus.Soil;
        obstacleStatus = FarmObstacleStatus.None;
        isSelected = false;
        hasCrop = false;
        // Initialize with default timestamp (year 0, spring, day 1, hour 0, minute 0)
        timeWatered = new GameTimestamp(0, GameTimestamp.Season.Spring, 1, 0, 0);
    }

    public void SetLandStatus(LandStatus newStatus)
    {
        if (landStatus != newStatus)
        {
            landStatus = newStatus;
            OnLandStatusChanged?.Invoke(landStatus);
        }
    }

    public void SetObstacleStatus(FarmObstacleStatus newStatus)
    {
        if (obstacleStatus != newStatus)
        {
            obstacleStatus = newStatus;
            OnObstacleStatusChanged?.Invoke(obstacleStatus);
        }
    }

    public void SetSelection(bool selected)
    {
        if (isSelected != selected)
        {
            isSelected = selected;
            OnSelectionChanged?.Invoke(isSelected);
        }
    }

    public void SetTimeWatered(GameTimestamp timestamp)
    {
        timeWatered = timestamp;
        OnTimeWateredChanged?.Invoke(timeWatered);
    }

    public void SetCropState(bool cropPresent)
    {
        if (hasCrop != cropPresent)
        {
            hasCrop = cropPresent;
            OnCropStateChanged?.Invoke(hasCrop);
        }
    }

    public bool CanPlantSeed()
    {
        return landStatus != LandStatus.Soil && !hasCrop && obstacleStatus == FarmObstacleStatus.None;
    }

    public bool CanUseWateringCan()
    {
        return landStatus != LandStatus.Soil;
    }

    public bool CanRemoveObstacle(FarmObstacleStatus targetObstacle)
    {
        return obstacleStatus == targetObstacle;
    }
}