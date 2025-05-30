using BlossomValley.LandSystem;
using BlossomValley.TimeSystem;

namespace BlossomValley.SaveSystem
{
    [System.Serializable]
    public struct LandSaveState
    {
        public LandModel.LandStatus landStatus;
        public GameTimestamp lastWatered;
        public LandModel.FarmObstacleStatus obstacleStatus;

        public LandSaveState(LandModel.LandStatus landStatusToSave, GameTimestamp lastWateredToSave, LandModel.FarmObstacleStatus obstacleStatusToSave)
        {
            landStatus = landStatusToSave;
            lastWatered = lastWateredToSave;
            obstacleStatus = obstacleStatusToSave;
        }

        public void ClockUpdate(GameTimestamp timestamp)
        {
            if (landStatus == LandModel.LandStatus.Watered)
            {
                int hoursElapsed = GameTimestamp.CompareTimestamps(lastWatered, timestamp);

                if (hoursElapsed > 24)
                    landStatus = LandModel.LandStatus.Farmland;
            }
        }
    }
}