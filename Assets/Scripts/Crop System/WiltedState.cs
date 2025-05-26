public class WiltedState : ICropState
{
    public void EnterState(CropContext context)
    {
        context.DeactivateAllVisuals();
        context.wilted.SetActive(true);
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
        context.NotifyLandManager();
    }

    public CropBehaviour.CropState GetStateType() => CropBehaviour.CropState.Wilted;
}