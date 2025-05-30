using System.Collections.Generic;
using UnityEngine;

namespace BlossomValley.AnimalSystem
{
    public class AnimalFeedManager : MonoBehaviour
    {
        public static Dictionary<AnimalData, bool[]> feedboxStatus = new Dictionary<AnimalData, bool[]>();
        public Feedbox[] feedboxes;
        public AnimalData animal;

        private void OnEnable()
        {
            feedboxes = GetComponentsInChildren<Feedbox>();
            RegisterFeedboxes();
            LoadFeedboxData();
        }

        public static void ResetFeedboxes() => feedboxStatus = new Dictionary<AnimalData, bool[]>();

        public void FeedAnimal(int id)
        {
            List<AnimalRelationshipState> eligibleAnimals = AnimalStats.GetAnimalsByType(animal);

            foreach (AnimalRelationshipState a in eligibleAnimals)
            {
                if (!a.giftGivenToday)
                {
                    a.giftGivenToday = true;
                    break;
                }
            }

            feedboxStatus[animal][id] = true;
            LoadFeedboxData();
        }

        void RegisterFeedboxes()
        {
            for (int i = 0; i < feedboxes.Length; i++)
                feedboxes[i].id = i;
        }

        void LoadFeedboxData()
        {
            if (!feedboxStatus.ContainsKey(animal))
                feedboxStatus.Add(animal, new bool[feedboxes.Length]);

            bool[] currentFeedboxStatus = feedboxStatus[animal];

            for (int i = 0; i < feedboxes.Length; i++)
                feedboxes[i].SetFeedState(currentFeedboxStatus[i]);
        }
    }
}