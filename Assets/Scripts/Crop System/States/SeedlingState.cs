namespace BlossomValley.CropSystem
{
    public class SeedlingState : ICropState
    {
        public void EnterState(CropContext context)
        {
            context.DeactivateAllVisuals();
            context.seedling.SetActive(true);
            context.health = context.maxHealth;
        }

        public void ExitState(CropContext context) { }

        public void Grow(CropContext context)
        {
            context.growth++;

            if (context.health < context.maxHealth)
                context.health++;

            if (context.growth >= context.maxGrowth)
                context.SetState(CropBehaviour.CropState.Harvestable);

            context.NotifyLandManager();
        }

        public void Wither(CropContext context)
        {
            context.health--;

            if (context.health <= 0)
                context.SetState(CropBehaviour.CropState.Wilted);

            context.NotifyLandManager();
        }

        public CropBehaviour.CropState GetStateType() => CropBehaviour.CropState.Seedling;
    }
}