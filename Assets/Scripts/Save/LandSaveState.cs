using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct LandSaveState 
{
    public LandModel.LandStatus landStatus;
    public GameTimestamp lastWatered;
    public LandModel.FarmObstacleStatus obstacleStatus; 

    public LandSaveState(LandModel.LandStatus landStatus, GameTimestamp lastWatered, LandModel.FarmObstacleStatus obstacleStatus)
    {
        this.landStatus = landStatus;
        this.lastWatered = lastWatered;
        this.obstacleStatus = obstacleStatus; 
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        //Checked if 24 hours has passed since last watered
        if (landStatus == LandModel.LandStatus.Watered)
        {
            //Hours since the land was watered
            int hoursElapsed = GameTimestamp.CompareTimestamps(lastWatered, timestamp);
            Debug.Log(hoursElapsed + " hours since this was watered");

            if (hoursElapsed > 24)
            {
                //Dry up (Switch back to farmland)
                landStatus = LandModel.LandStatus.Farmland; 
            }
        }

        
    }
}
