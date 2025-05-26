using UnityEngine;

public class HarvestableState : ICropState
{
    public void EnterState(CropContext context)
    {
        context.DeactivateAllVisuals();
        context.harvestable.SetActive(true);

        // If the seed is not regrowable, detach the harvestable from this crop gameobject
        if (!context.seedToGrow.regrowable)
        {
            // Unparent it to the crop
            context.harvestable.transform.parent = null;

            // Pass the RemoveCrop function over to the Interactable Object
            context.harvestable.GetComponent<InteractableObject>().onInteract.AddListener(context.RemoveCrop);
        }
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

        // Already at max growth state, no state change needed

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
        return CropBehaviour.CropState.Harvestable;
    }
}