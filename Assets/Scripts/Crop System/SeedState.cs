using UnityEngine;

public class SeedState : ICropState
{
    public void EnterState(CropContext context)
    {
        context.DeactivateAllVisuals();
        context.seed.SetActive(true);
    }

    public void ExitState(CropContext context)
    {
        // No specific exit logic needed
    }

    public void Grow(CropContext context)
    {
        // Increase the growth point by 1
        context.growth++;

        // Restore the health of the plant when it is watered
        if (context.health < context.maxHealth)
        {
            context.health++;
        }

        // The seed will sprout into a seedling when the growth is at 50%
        if (context.growth >= context.maxGrowth / 2)
        {
            context.SetState(CropBehaviour.CropState.Seedling);
        }

        // Inform LandManager on the changes
        context.NotifyLandManager();
    }

    public void Wither(CropContext context)
    {
        context.health--;
        // Seeds don't wilt - they stay as seeds even at 0 health
        
        // Inform LandManager on the changes
        context.NotifyLandManager();
    }

    public CropBehaviour.CropState GetStateType()
    {
        return CropBehaviour.CropState.Seed;
    }
}