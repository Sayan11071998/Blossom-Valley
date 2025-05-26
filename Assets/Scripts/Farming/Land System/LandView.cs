using UnityEngine;

/// <summary>
/// MonoBehaviour view handling the visual representation and Unity-specific functionality of land
/// </summary>
public class LandView : MonoBehaviour
{
    [Header("Materials")]
    public Material soilMat, farmlandMat, wateredMat;
    
    [Header("Selection")]
    public GameObject select;
    
    [Header("Crops")]
    public GameObject cropPrefab;
    
    [Header("Obstacles")]
    public GameObject rockPrefab, woodPrefab, weedsPrefab;
    
    // Private fields
    private new Renderer renderer;
    private GameObject obstacleObject;
    private CropBehaviour cropPlanted = null;
    private LandController controller;

    void Start()
    {
        // Get the renderer component
        renderer = GetComponent<Renderer>();
        select.gameObject.SetActive(false); // Hide selection by default
        
        // Controller will be set by LandManager
    }

    /// <summary>
    /// Initialize the MVC components (called by LandManager)
    /// </summary>
    public void InitializeMVC(int id)
    {
        // Create model and controller
        LandModel model = new LandModel(id);
        controller = new LandController(model, this);
        controller.Initialize();
    }

    public void LoadLandData(LandModel.LandStatus landStatusToSwitch, GameTimestamp lastWatered, LandModel.FarmObstacleStatus obstacleStatusToSwitch)
    {
        controller?.LoadLandData(landStatusToSwitch, lastWatered, obstacleStatusToSwitch);
    }

    public void SwitchLandStatus(LandModel.LandStatus statusToSwitch)
    {
        controller?.SwitchLandStatus(statusToSwitch);
    }

    public void SetObstacleStatus(LandModel.FarmObstacleStatus statusToSwitch)
    {
        controller?.SetObstacleStatus(statusToSwitch);
    }

    public void Select(bool toggle)
    {
        controller?.Select(toggle);
    }

    public void Interact()
    {
        controller?.Interact();
    }

    // Visual update methods called by controller
    public void UpdateLandVisuals(LandModel.LandStatus landStatus)
    {
        Material materialToSwitch = soilMat;

        switch (landStatus)
        {
            case LandModel.LandStatus.Soil:
                materialToSwitch = soilMat;
                break;
            case LandModel.LandStatus.Farmland:
                materialToSwitch = farmlandMat;
                break;
            case LandModel.LandStatus.Watered:
                materialToSwitch = wateredMat;
                break;
        }

        renderer.material = materialToSwitch;
    }

    public void UpdateObstacleVisuals(LandModel.FarmObstacleStatus obstacleStatus)
    {
        // Destroy existing obstacle
        if (obstacleObject != null) 
        {
            Destroy(obstacleObject);
            obstacleObject = null;
        }

        // Create new obstacle based on status
        switch (obstacleStatus)
        {
            case LandModel.FarmObstacleStatus.None:
                // Already destroyed above
                break;
            case LandModel.FarmObstacleStatus.Rock:
                obstacleObject = Instantiate(rockPrefab, transform);
                break;
            case LandModel.FarmObstacleStatus.Wood:
                obstacleObject = Instantiate(woodPrefab, transform);
                break;
            case LandModel.FarmObstacleStatus.Weeds:
                obstacleObject = Instantiate(weedsPrefab, transform);
                break;
        }

        // Position the obstacle
        if (obstacleObject != null)
        {
            obstacleObject.transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
        }
    }

    public void UpdateSelectionVisuals(bool isSelected)
    {
        select.SetActive(isSelected);
    }

    // Crop-related methods
    public CropBehaviour SpawnCrop()
    {
        // Instantiate the crop object parented to the land
        GameObject cropObject = Instantiate(cropPrefab, transform);
        // Move the crop object to the top of the land gameobject
        cropObject.transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);

        // Access the CropBehaviour of the crop we're going to plant
        cropPlanted = cropObject.GetComponent<CropBehaviour>();
        return cropPlanted;
    }

    public void RemoveCrop()
    {
        if (cropPlanted != null)
        {
            cropPlanted.RemoveCrop();
            cropPlanted = null;
        }
    }

    public void GrowCrop()
    {
        if (cropPlanted != null)
        {
            cropPlanted.Grow();
        }
    }

    public void WitherCrop()
    {
        if (cropPlanted != null)
        {
            // If the crop has already germinated, start the withering
            if (cropPlanted.cropState != CropBehaviour.CropState.Seed)
            {
                cropPlanted.Wither();
            }
        }
    }

    // Public getters for accessing controller data (for LandManager)
    public int Id => controller?.Id ?? 0;
    public LandModel.LandStatus LandStatus => controller?.LandStatus ?? LandModel.LandStatus.Soil;
    public LandModel.FarmObstacleStatus ObstacleStatus => controller?.ObstacleStatus ?? LandModel.FarmObstacleStatus.None;
    public GameTimestamp TimeWatered => controller?.TimeWatered ?? new GameTimestamp(0, GameTimestamp.Season.Spring, 1, 0, 0);
    public bool HasCrop => controller?.HasCrop ?? false;

    void OnDestroy()
    {
        controller?.Dispose();
    }
}