using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentScriptableObject", menuName = "Items/Equipment")]
public class EquipmentData : ItemData
{
    public enum ToolType
    {
        Hoe, WateringCan, Axe, Pickaxe, Shovel
    }

    public ToolType toolType;
}