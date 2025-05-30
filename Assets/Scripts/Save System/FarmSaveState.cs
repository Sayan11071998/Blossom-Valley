using System.Collections.Generic;
using BlossomValley.IncubationSystem;
using BlossomValley.LandSystem;

namespace BlossomValley.SaveSystem
{
    [System.Serializable]
    public class FarmSaveState
    {
        public List<LandSaveState> landData;
        public List<CropSaveState> cropData;
        public List<EggIncubationSaveState> eggsIncubating;

        public FarmSaveState(List<LandSaveState> landDataValue, List<CropSaveState> cropDataValue, List<EggIncubationSaveState> eggsIncubatingValue)
        {
            landData = landDataValue;
            cropData = cropDataValue;
            eggsIncubating = eggsIncubatingValue;
        }

        public static FarmSaveState Export()
        {
            List<LandSaveState> landData = LandManager.farmData.Item1;
            List<CropSaveState> cropData = LandManager.farmData.Item2;
            List<EggIncubationSaveState> eggsIncubating = IncubationManager.eggsIncubating;
            return new FarmSaveState(landData, cropData, eggsIncubating);
        }

        public void LoadData()
        {
            LandManager.farmData = new System.Tuple<List<LandSaveState>, List<CropSaveState>>(landData, cropData);
            IncubationManager.eggsIncubating = eggsIncubating;
        }
    }
}