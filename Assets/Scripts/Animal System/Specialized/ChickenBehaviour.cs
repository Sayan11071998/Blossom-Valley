using UnityEngine;
using BlossomValley.InventorySystem;

namespace BlossomValley.AnimalSystem
{
    public class ChickenBehaviour : AnimalBehaviour
    {
        protected override void Start()
        {
            base.Start();
            LayEgg();
        }

        private void LayEgg()
        {
            AnimalData animalType = AnimalStats.GetAnimalTypeFromString(relationship.animalType);

            if (relationship.age < animalType.daysToMature) return;

            if (relationship.Mood > 30 && !relationship.givenProduceToday)
            {
                ItemData egg = InventoryManager.Instance.GetItemFromString("Egg");
                Instantiate(egg.gameModel, transform.position, Quaternion.identity);
                relationship.givenProduceToday = true;
            }
        }
    }
}