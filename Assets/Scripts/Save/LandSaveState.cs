using UnityEngine;

[System.Serializable]
public struct LandSaveState
{
    public Land.LandStatus landStatus;
    public GameTimestamp lastWatered;

    public LandSaveState(Land.LandStatus landStatus, GameTimestamp lastWatered)
    {
        this.landStatus = landStatus;
        this.lastWatered = lastWatered;
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        //Checked if 24 hours has passed since last watered
        if (landStatus == Land.LandStatus.Watered)
        {
            //Hours since the land was watered
            int hoursElapsed = GameTimestamp.CompareTimestamps(lastWatered, timestamp);

            if (hoursElapsed > 24)
            {
                landStatus = Land.LandStatus.Farmland;
            }
        }
    }
}