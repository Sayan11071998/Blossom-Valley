public class CropStateFactory
{
    public ICropState CreateState(CropBehaviour.CropState stateType)
    {
        switch (stateType)
        {
            case CropBehaviour.CropState.Seed:
                return new SeedState();
            case CropBehaviour.CropState.Seedling:
                return new SeedlingState();
            case CropBehaviour.CropState.Harvestable:
                return new HarvestableState();
            case CropBehaviour.CropState.Wilted:
                return new WiltedState();
            default:
                return new SeedState();
        }
    }
}