using UnityEngine;

public class Land : MonoBehaviour, ITimeTracker
{
    public enum LandStatus
    {
        Soil,
        Farmland,
        Watered
    }

    public LandStatus landStatus;

    [SerializeField] private Material soilMat;
    [SerializeField] private Material farmlandMat;
    [SerializeField] private Material wateredMat;
    [SerializeField] private GameObject select;

    private new Renderer renderer;
    private GameTimeStamp timeWatered;

    [Header("Crops")]
    public GameObject cropPrefab;
    private CropBehaviour cropPlanted = null;

    private void Start()
    {
        renderer = GetComponent<Renderer>();

        SwitchLandStatus(LandStatus.Soil);
        Select(false);

        TimeManager.Instance.RegisterTracker(this);
    }

    public void SwitchLandStatus(LandStatus statusToSwitch)
    {
        landStatus = statusToSwitch;
        Material materialToSwitch = soilMat;

        switch (statusToSwitch)
        {
            case LandStatus.Soil:
                materialToSwitch = soilMat;
                break;
            case LandStatus.Farmland:
                materialToSwitch = farmlandMat;
                break;
            case LandStatus.Watered:
                materialToSwitch = wateredMat;
                timeWatered = TimeManager.Instance.GetGameTimeStamp();
                break;
        }

        renderer.material = materialToSwitch;
    }

    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }

    public void Interact()
    {
        ItemData toolSlot = InventoryManager.Instance.equippedTool;

        if (toolSlot == null)
        {
            return;
        }

        EquipmentData equipmentTool = toolSlot as EquipmentData;

        if (equipmentTool != null)
        {
            EquipmentData.ToolType toolType = equipmentTool.toolType;

            switch (toolType)
            {
                case EquipmentData.ToolType.Hoe:
                    SwitchLandStatus(LandStatus.Farmland);
                    break;
                case EquipmentData.ToolType.WateringCan:
                    SwitchLandStatus(LandStatus.Watered);
                    break;
            }

            return;
        }

        SeedData seedTool = toolSlot as SeedData;

        if (seedTool != null && landStatus != LandStatus.Soil && cropPlanted == null)
        {
            GameObject cropObject = Instantiate(cropPrefab, transform);
            cropObject.transform.position = new Vector3(transform.position.x, 0.51f, transform.position.z);

            cropPlanted = cropObject.GetComponent<CropBehaviour>();
            cropPlanted.Plant(seedTool);
        }
    }

    public void ClockUpdate(GameTimeStamp timeStamp)
    {
        if (landStatus == LandStatus.Watered)
        {
            int hoursElasped = GameTimeStamp.CompareTimestamp(timeWatered, timeStamp);

            if (cropPlanted != null)
            {
                cropPlanted.Grow();
            }

            if (hoursElasped > 24)
            {
                SwitchLandStatus(LandStatus.Farmland);
            }
        }
    }
}