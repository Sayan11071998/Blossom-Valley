using UnityEngine;
using BlossomValley.CharacterSystem;

namespace BlossomValley.AnimalSystem
{
    [System.Serializable]
    public class AnimalRelationshipState : NPCRelationshipState
    {
        public string animalType;

        private const int MAX_MOOD = 255;

        private int mood;
        public int Mood
        {
            get => mood;
            set => mood = Mathf.Clamp(value, 0, MAX_MOOD);
        }

        public int age;
        public bool givenProduceToday;

        public AnimalRelationshipState(string name, AnimalData animalType) : base(name)
        {
            this.animalType = animalType.name;
            Mood = MAX_MOOD;
        }

        public AnimalRelationshipState(string name, AnimalData animalType, int friendshipPoints) : base(name, friendshipPoints)
        {
            this.animalType = animalType.name;
            Mood = MAX_MOOD;
        }

        public AnimalRelationshipState(string name, AnimalData animalType, int friendshipPoints, int mood) : base(name, friendshipPoints)
        {
            this.animalType = animalType.name;
            Mood = mood;
        }

        public AnimalData AnimalType() => AnimalStats.GetAnimalTypeFromString(animalType);
    }
}