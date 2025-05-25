using UnityEngine;

[CreateAssetMenu(fileName = "SeedScriptableObject", menuName = "Items/Seed")]
public class SeedData : EquipmentData
{
    public int daysToGrow;
    public ItemData cropToYield;
    public GameObject seedling;

    [Header("Regrowable")]
    public bool regrowable;
    public int daysToRegrow;
}