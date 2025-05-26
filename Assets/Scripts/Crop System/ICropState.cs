public interface ICropState
{
    public void EnterState(CropContext context);
    public void ExitState(CropContext context);
    public void Grow(CropContext context);
    public void Wither(CropContext context);
    public CropBehaviour.CropState GetStateType();
}