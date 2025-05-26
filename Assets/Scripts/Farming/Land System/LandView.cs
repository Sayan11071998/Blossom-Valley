using UnityEngine;

public class LandView : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material soilMaterial;
    [SerializeField] private Material farmlandMaterial;
    [SerializeField] private Material wateredMaterial;

    [Header("Selection")]
    [SerializeField] private GameObject selectGameObject;

    [Header("Crops")]
    [SerializeField] private GameObject cropPrefab;

    [Header("Obstacles")]
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private GameObject weedsPrefab;

    private new Renderer renderer;
    private GameObject obstacleObject;
    private CropBehaviour cropPlanted = null;
    private LandController controller;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        selectGameObject.gameObject.SetActive(false);
    }

    public void InitializeMVC(int id)
    {
        LandModel model = new LandModel(id);
        controller = new LandController(model, this);
        controller.Initialize();
    }

    public void LoadLandData(LandModel.LandStatus landStatusToSwitch, GameTimestamp lastWatered, LandModel.FarmObstacleStatus obstacleStatusToSwitch) => controller?.LoadLandData(landStatusToSwitch, lastWatered, obstacleStatusToSwitch);

    public void SwitchLandStatus(LandModel.LandStatus statusToSwitch) => controller?.SwitchLandStatus(statusToSwitch);

    public void SetObstacleStatus(LandModel.FarmObstacleStatus statusToSwitch) => controller?.SetObstacleStatus(statusToSwitch);

    public void Select(bool toggle) => controller?.Select(toggle);

    public void Interact() => controller?.Interact();

    public void UpdateLandVisuals(LandModel.LandStatus landStatus)
    {
        Material materialToSwitch = soilMaterial;

        switch (landStatus)
        {
            case LandModel.LandStatus.Soil:
                materialToSwitch = soilMaterial;
                break;
            case LandModel.LandStatus.Farmland:
                materialToSwitch = farmlandMaterial;
                break;
            case LandModel.LandStatus.Watered:
                materialToSwitch = wateredMaterial;
                break;
        }

        renderer.material = materialToSwitch;
    }

    public void UpdateObstacleVisuals(LandModel.FarmObstacleStatus obstacleStatus)
    {
        if (obstacleObject != null)
        {
            Destroy(obstacleObject);
            obstacleObject = null;
        }

        switch (obstacleStatus)
        {
            case LandModel.FarmObstacleStatus.None:
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

        if (obstacleObject != null)
            obstacleObject.transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
    }

    public void UpdateSelectionVisuals(bool isSelected) => selectGameObject.SetActive(isSelected);

    public CropBehaviour SpawnCrop()
    {
        GameObject cropObject = Instantiate(cropPrefab, transform);
        cropObject.transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
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
            cropPlanted.Grow();
    }

    public void WitherCrop()
    {
        if (cropPlanted != null)
        {
            if (cropPlanted.cropState != CropBehaviour.CropState.Seed)
                cropPlanted.Wither();
        }
    }

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