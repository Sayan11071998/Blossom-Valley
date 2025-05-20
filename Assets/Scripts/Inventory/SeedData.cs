using UnityEngine;

[CreateAssetMenu(fileName = "Seed", menuName = "Items/Seed")]
public class SeedData : ItemData
{
    public int daysToGrow;
    public ItemData cropToYield;
    public GameObject seedling;

    [Header("Regrowable")]
    public bool regrowable;
    public int daysToRegrow;
}