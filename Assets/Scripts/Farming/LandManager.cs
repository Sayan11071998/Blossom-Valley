using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandManager : MonoBehaviour
{
    public static LandManager Instance { get; private set; }

    public static Tuple<List<LandSaveState>, List<CropSaveState>> farmData = null;

    List<Land> landPlots = new List<Land>();

    List<LandSaveState> landData = new List<LandSaveState>();
    List<CropSaveState> cropData = new List<CropSaveState>();

    private void Awake()
    {
        //If there is more than one instance, destroy the extra
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            //Set the static instance to this instance
            Instance = this;
        }
    }

    private void OnEnable()
    {
        RegisterLandPlots();
        StartCoroutine(LoadFarmData());
    }

    IEnumerator LoadFarmData()
    {
        yield return new WaitForEndOfFrame();
        //Load farm data if any
        if (farmData != null)
        {
            //Load in any saved data 
            ImportLandData(farmData.Item1);
            ImportCropData(farmData.Item2);
        }
    }

    private void OnDestroy()
    {
        farmData = new Tuple<List<LandSaveState>, List<CropSaveState>>(landData, cropData);
    }

    void RegisterLandPlots()
    {
        foreach (Transform landTransform in transform)
        {
            Land land = landTransform.GetComponent<Land>();
            landPlots.Add(land);
            landData.Add(new LandSaveState());
            land.id = landPlots.Count - 1;
        }
    }

    public void RegisterCrop(int landID, SeedData seedToGrow, CropBehaviour.CropState cropState, int growth, int health)
    {
        cropData.Add(new CropSaveState(landID, seedToGrow.name, cropState, growth, health));
    }

    public void DeregisterCrop(int landID)
    {
        //Find its index in the list from the landID and remove it
        cropData.RemoveAll(x => x.landID == landID);
    }

    public void OnLandStateChange(int id, Land.LandStatus landStatus, GameTimestamp lastWatered)
    {
        landData[id] = new LandSaveState(landStatus, lastWatered);
    }

    public void OnCropStateChange(int landID, CropBehaviour.CropState cropState, int growth, int health)
    {
        //Find its index in the list from the landID
        int cropIndex = cropData.FindIndex(x => x.landID == landID);

        string seedToGrow = cropData[cropIndex].seedToGrow;
        cropData[cropIndex] = new CropSaveState(landID, seedToGrow, cropState, growth, health);
    }

    public void ImportLandData(List<LandSaveState> landDatasetToLoad)
    {
        for (int i = 0; i < landDatasetToLoad.Count; i++)
        {
            //Get the individual land save state
            LandSaveState landDataToLoad = landDatasetToLoad[i];
            //Load it up onto the Land instance
            landPlots[i].LoadLandData(landDataToLoad.landStatus, landDataToLoad.lastWatered);

        }

        landData = landDatasetToLoad;
    }

    public void ImportCropData(List<CropSaveState> cropDatasetToLoad)
    {
        cropData = cropDatasetToLoad;

        foreach (CropSaveState cropSave in cropDatasetToLoad)
        {
            Land landToPlant = landPlots[cropSave.landID];
            //Spawn the crop
            CropBehaviour cropToPlant = landToPlant.SpawnCrop();
            Debug.Log(cropToPlant.gameObject);
            //Load in the data
            SeedData seedToGrow = (SeedData)InventoryManager.Instance.itemIndex.GetItemFromString(cropSave.seedToGrow);
            cropToPlant.LoadCrop(cropSave.landID, seedToGrow, cropSave.cropState, cropSave.growth, cropSave.health);
        }
    }
}