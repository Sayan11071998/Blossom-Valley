using UnityEngine;

public interface ICropState
{
    void EnterState(CropContext context);
    void ExitState(CropContext context);
    void Grow(CropContext context);
    void Wither(CropContext context);
    CropBehaviour.CropState GetStateType();
}