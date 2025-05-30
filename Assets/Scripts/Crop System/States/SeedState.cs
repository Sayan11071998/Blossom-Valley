namespace BlossomValley.CropSystem
{
    public class SeedState : ICropState
    {
        public void EnterState(CropContext context)
        {
            context.DeactivateAllVisuals();
            context.seed.SetActive(true);
        }

        public void ExitState(CropContext context) { }

        public void Grow(CropContext context)
        {
            context.growth++;

            if (context.health < context.maxHealth)
                context.health++;

            if (context.growth >= context.maxGrowth / 2)
                context.SetState(CropBehaviour.CropState.Seedling);

            context.NotifyLandManager();
        }

        public void Wither(CropContext context)
        {
            context.health--;
            context.NotifyLandManager();
        }

        public CropBehaviour.CropState GetStateType() => CropBehaviour.CropState.Seed;
    }
}