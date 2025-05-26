using UnityEngine;

public class WiltedState : ICropState
{
    public void EnterState(CropContext context)
    {
        context.DeactivateAllVisuals();
        context.wilted.SetActive(true);
    }

    public void ExitState(CropContext context)
    {
        // No specific exit logic needed
    }

    public void Grow(CropContext context)
    {
        // Wilted crops don't grow, but we still update for consistency
        context.growth++;

        if (context.health < context.maxHealth)
        {
            context.health++;
        }

        // Inform LandManager on the changes
        context.NotifyLandManager();
    }

    public void Wither(CropContext context)
    {
        // Already wilted, can't wither more, but update for consistency
        context.health--;

        // Inform LandManager on the changes
        context.NotifyLandManager();
    }

    public CropBehaviour.CropState GetStateType()
    {
        return CropBehaviour.CropState.Wilted;
    }
}