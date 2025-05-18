using UnityEngine;

[CreateAssetMenu(fileName = "Seed", menuName = "Items/Seed")]
public class SeedData : ItemData
{
    public int daysToGrow;
    public ItemData cropToYield;
}