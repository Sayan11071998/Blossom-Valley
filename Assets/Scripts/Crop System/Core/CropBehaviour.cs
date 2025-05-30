using UnityEngine;
using BlossomValley.InventorySystem;

namespace BlossomValley.CropSystem
{
    public class CropBehaviour : MonoBehaviour
    {
        [Header("Stages of Life")]
        [SerializeField] private GameObject seedGameObject;
        [SerializeField] private GameObject wiltedGameObject;

        private CropContext cropContext;

        public enum CropState
        {
            Seed, Seedling, Harvestable, Wilted
        }

        public CropState cropState => cropContext.GetCurrentStateType();
        public int landID => cropContext.landID;
        public SeedData seedToGrow => cropContext.seedToGrow;
        public int growth => cropContext.growth;
        public int health => cropContext.health;

        private void Awake() => cropContext = new CropContext(this);

        public void Plant(int landID, SeedData seedToGrow)
        {
            LoadCrop(landID, seedToGrow, CropState.Seed, 0, 0);
            LandManager.Instance.RegisterCrop(landID, seedToGrow, cropState, growth, health);
        }

        public void LoadCrop(int landID, SeedData seedToGrow, CropState cropState, int growth, int health)
        {
            cropContext.Initialize(landID, seedToGrow, seedGameObject, wiltedGameObject);
            cropContext.LoadState(cropState, growth, health);
        }

        public void Grow() => cropContext.Grow();

        public void Wither() => cropContext.Wither();

        public void RemoveCrop() => cropContext.RemoveCrop();

        public void Regrow() => cropContext.Regrow();
    }
}