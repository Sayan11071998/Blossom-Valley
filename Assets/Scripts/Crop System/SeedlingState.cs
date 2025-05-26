using UnityEngine;

public class SeedlingState : ICropState
{
    public void EnterState(CropContext context)
    {
        context.DeactivateAllVisuals();
        context.seedling.SetActive(true);
        
        // Give the seed health when becoming seedling
        context.health = context.maxHealth;
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

        // Grow from seedling to harvestable
        if (context.growth >= context.maxGrowth)
        {
            context.SetState(CropBehaviour.CropState.Harvestable);
        }

        // Inform LandManager on the changes
        context.NotifyLandManager();
    }

    public void Wither(CropContext context)
    {
        context.health--;
        
        // If the health is below 0, kill it
        if (context.health <= 0)
        {
            context.SetState(CropBehaviour.CropState.Wilted);
        }

        // Inform LandManager on the changes
        context.NotifyLandManager();
    }

    public CropBehaviour.CropState GetStateType()
    {
        return CropBehaviour.CropState.Seedling;
    }
}