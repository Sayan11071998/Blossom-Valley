[System.Serializable]
public struct CropSaveState
{
    public int landID;
    public string seedToGrow;
    public CropBehaviour.CropState cropState;
    public int growth;
    public int health;

    public CropSaveState(int landIDData, string seedToGrowData, CropBehaviour.CropState cropStateData, int growthData, int healthData)
    {
        landID = landIDData;
        seedToGrow = seedToGrowData;
        cropState = cropStateData;
        growth = growthData;
        health = healthData;
    }

    public void Grow()
    {
        growth++;

        SeedData seedInfo = (SeedData)InventoryManager.Instance.GetItemFromString(seedToGrow);
        int maxGrowth = GameTimestamp.HoursToMinutes(GameTimestamp.DaysToHours(seedInfo.daysToGrow));
        int maxHealth = GameTimestamp.HoursToMinutes(48);

        if (health < maxHealth)
            health++;

        if (growth >= maxGrowth / 2 && cropState == CropBehaviour.CropState.Seed)
            cropState = CropBehaviour.CropState.Seedling;

        if (growth >= maxGrowth && cropState == CropBehaviour.CropState.Seedling)
            cropState = CropBehaviour.CropState.Harvestable;
    }

    public void Wither()
    {
        health--;

        if (health <= 0 && cropState != CropBehaviour.CropState.Seed)
            cropState = CropBehaviour.CropState.Wilted;
    }
}