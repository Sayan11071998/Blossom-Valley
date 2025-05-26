public class HarvestableState : ICropState
{
    public void EnterState(CropContext context)
    {
        context.DeactivateAllVisuals();
        context.harvestable.SetActive(true);

        if (!context.seedToGrow.regrowable)
        {
            context.harvestable.transform.parent = null;
            context.harvestable.GetComponent<InteractableObject>().onInteract.AddListener(context.RemoveCrop);
        }
    }

    public void ExitState(CropContext context) { }

    public void Grow(CropContext context)
    {
        context.growth++;

        if (context.health < context.maxHealth)
            context.health++;

        context.NotifyLandManager();
    }

    public void Wither(CropContext context)
    {
        context.health--;

        if (context.health <= 0)
            context.SetState(CropBehaviour.CropState.Wilted);

        context.NotifyLandManager();
    }

    public CropBehaviour.CropState GetStateType() => CropBehaviour.CropState.Harvestable;
}