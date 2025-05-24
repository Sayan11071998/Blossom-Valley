using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class FarmSaveState 
{
    public List<LandSaveState> landData;
    public List<CropSaveState> cropData;
    public List<EggIncubationSaveState> eggsIncubating;

    public FarmSaveState(
        List<LandSaveState> landData,
        List<CropSaveState> cropData,
        List<EggIncubationSaveState> eggsIncubating
        )
    {
        this.landData = landData;
        this.cropData = cropData;
        this.eggsIncubating = eggsIncubating;
    }

    //Prepare the data for exporting 
    public static FarmSaveState Export()
    {
        List<LandSaveState> landData = LandManager.farmData.Item1;
        List<CropSaveState> cropData = LandManager.farmData.Item2;
        List<EggIncubationSaveState> eggsIncubating = IncubationManager.eggsIncubating; 
        return new FarmSaveState(landData, cropData, eggsIncubating); 

    }

    //Load in the data into LandManager
    public void LoadData()
    {
        LandManager.farmData = new System.Tuple<List<LandSaveState>, List<CropSaveState>>(landData, cropData);
        IncubationManager.eggsIncubating = eggsIncubating;
    }
}
