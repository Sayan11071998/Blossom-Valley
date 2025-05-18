using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipment")]
public class EquipmentData : ItemData
{
    public enum ToolType
    {
        Hoe,
        WateringCan,
        Axe,
        Pickaxe
    }

    public ToolType toolType;
}