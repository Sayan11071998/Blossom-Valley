using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlossomValley.CropSystem;

[RequireComponent(typeof(ObstacleGenerator))]
public class LandManager : MonoBehaviour
{
    public static LandManager Instance { get; private set; }

    public static Tuple<List<LandSaveState>, List<CropSaveState>> farmData = null;

    private List<LandView> landPlots = new List<LandView>();
    private List<LandSaveState> landData = new List<LandSaveState>();
    private List<CropSaveState> cropData = new List<CropSaveState>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void OnEnable()
    {
        RegisterLandPlots();
        StartCoroutine(LoadFarmData());
    }

    private IEnumerator LoadFarmData()
    {
        yield return new WaitForEndOfFrame();
        if (farmData != null)
        {
            ImportLandData(farmData.Item1);
            ImportCropData(farmData.Item2);
        }
        else
        {
            GetComponent<ObstacleGenerator>().GenerateObstacles(landPlots);
        }
    }

    private void OnDestroy()
    {
        farmData = new Tuple<List<LandSaveState>, List<CropSaveState>>(landData, cropData);
        cropData.ForEach((CropSaveState crop) =>
        {
            Debug.Log(crop.seedToGrow);
        });
    }

    private void RegisterLandPlots()
    {
        foreach (Transform landTransform in transform)
        {
            LandView landView = landTransform.GetComponent<LandView>();
            if (landView != null)
            {
                landPlots.Add(landView);
                landData.Add(new LandSaveState());
                int landId = landPlots.Count - 1;
                landView.InitializeMVC(landId);
            }
        }
    }

    public void RegisterCrop(int landID, SeedData seedToGrow, CropBehaviour.CropState cropState, int growth, int health) => cropData.Add(new CropSaveState(landID, seedToGrow.name, cropState, growth, health));

    public void DeregisterCrop(int landID) => cropData.RemoveAll(x => x.landID == landID);

    public void OnLandStateChange(int id, LandModel.LandStatus landStatus, GameTimestamp lastWatered, LandModel.FarmObstacleStatus obstacleStatus) => landData[id] = new LandSaveState(landStatus, lastWatered, obstacleStatus);

    public void OnCropStateChange(int landID, CropBehaviour.CropState cropState, int growth, int health)
    {
        int cropIndex = cropData.FindIndex(x => x.landID == landID);
        string seedToGrow = cropData[cropIndex].seedToGrow;
        cropData[cropIndex] = new CropSaveState(landID, seedToGrow, cropState, growth, health);
    }

    public void ImportLandData(List<LandSaveState> landDatasetToLoad)
    {
        for (int i = 0; i < landDatasetToLoad.Count; i++)
        {
            LandSaveState landDataToLoad = landDatasetToLoad[i];
            landPlots[i].LoadLandData(landDataToLoad.landStatus, landDataToLoad.lastWatered, landDataToLoad.obstacleStatus);
        }

        landData = landDatasetToLoad;
    }

    public void ImportCropData(List<CropSaveState> cropDatasetToLoad)
    {
        cropData = cropDatasetToLoad;
        foreach (CropSaveState cropSave in cropDatasetToLoad)
        {
            LandView landToPlant = landPlots[cropSave.landID];
            CropBehaviour cropToPlant = landToPlant.SpawnCrop();
            Debug.Log(cropToPlant.gameObject);
            SeedData seedToGrow = (SeedData)InventoryManager.Instance.GetItemFromString(cropSave.seedToGrow);
            cropToPlant.LoadCrop(cropSave.landID, seedToGrow, cropSave.cropState, cropSave.growth, cropSave.health);
        }
    }
}