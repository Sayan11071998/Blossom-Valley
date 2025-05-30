using UnityEngine;

namespace BlossomValley.AnimalSystem
{
    [CreateAssetMenu(fileName = "AnimalScriptableObject", menuName = "Animals/Animal")]
    public class AnimalData : ScriptableObject
    {
        public Sprite portrait;
        public AnimalBehaviour animalObject;
        public int purchasePrice;
        public int daysToMature;
        public ItemData produce;
        public SceneTransitionManager.Location locationToSpawn;
    }
}