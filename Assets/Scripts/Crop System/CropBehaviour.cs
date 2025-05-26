using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehaviour : MonoBehaviour
{
    [Header("Stages of Life")]
    public GameObject seed;
    public GameObject wilted;

    // State management
    private CropContext cropContext;

    public enum CropState
    {
        Seed, Seedling, Harvestable, Wilted
    }

    // Properties to access context data
    public CropState cropState => cropContext.GetCurrentStateType();
    public int landID => cropContext.landID;
    public SeedData seedToGrow => cropContext.seedToGrow;
    public int growth => cropContext.growth;
    public int health => cropContext.health;

    private void Awake()
    {
        cropContext = new CropContext(this);
    }

    // Initialisation for the crop GameObject
    // Called when the player plants a seed
    public void Plant(int landID, SeedData seedToGrow)
    {
        LoadCrop(landID, seedToGrow, CropState.Seed, 0, 0);
        LandManager.Instance.RegisterCrop(landID, seedToGrow, cropState, growth, health);
    }

    public void LoadCrop(int landID, SeedData seedToGrow, CropState cropState, int growth, int health)
    {
        cropContext.Initialize(landID, seedToGrow, seed, wilted);
        cropContext.LoadState(cropState, growth, health);
    }

    // Delegate to context
    public void Grow()
    {
        cropContext.Grow();
    }

    // Delegate to context
    public void Wither()
    {
        cropContext.Wither();
    }

    // Delegate to context
    public void RemoveCrop()
    {
        cropContext.RemoveCrop();
    }

    // Delegate to context
    public void Regrow()
    {
        cropContext.Regrow();
    }
}