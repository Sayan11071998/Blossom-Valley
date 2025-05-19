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
        }
    }

    public void ClockUpdate(GameTimeStamp timeStamp)
    {
        if (landStatus == LandStatus.Watered)
        {
            int hoursElasped = GameTimeStamp.CompareTimestamp(timeWatered, timeStamp);

            if (hoursElasped > 24)
            {
                SwitchLandStatus(LandStatus.Farmland);
            }
        }
    }
}