﻿using System.Collections.Generic;
using System.Linq;
using BlossomValley.Utilities;
using UnityEngine;
using BlossomValley.UISystem;

namespace BlossomValley.AnimalSystem
{
    public class AnimalStats : MonoBehaviour
    {
        public static List<AnimalRelationshipState> animalRelationships = new List<AnimalRelationshipState>();

        private static List<AnimalData> animals = Resources.LoadAll<AnimalData>("Animals").ToList();

        public static void StartAnimalCreation(AnimalData animalType)
        {
            string prompt = string.Format(GameString.AnimalNamingPrompt, animalType.name);
            UIManager.Instance.TriggerNamingPrompt(prompt, (inputString) =>
            {
                animalRelationships.Add(new AnimalRelationshipState(inputString, animalType));
            });

        }

        public static void LoadStats(List<AnimalRelationshipState> relationshipsToLoad)
        {
            if (relationshipsToLoad == null)
            {
                animalRelationships = new List<AnimalRelationshipState>();
                return;
            }
            animalRelationships = relationshipsToLoad;
        }

        public static void ResetAllAnimalRelationships() => animalRelationships = new List<AnimalRelationshipState>();

        public static List<AnimalRelationshipState> GetAnimalsByType(string animalTypeName) => animalRelationships.FindAll(x => x.animalType == animalTypeName);

        public static List<AnimalRelationshipState> GetAnimalsByType(AnimalData animalType) => GetAnimalsByType(animalType.name);

        public static void OnDayReset()
        {
            foreach (AnimalRelationshipState animal in AnimalStats.animalRelationships)
            {
                if (animal.hasTalkedToday)
                    animal.friendshipPoints += 30;
                else
                    animal.friendshipPoints -= (10 - (animal.friendshipPoints / 200));

                if (animal.giftGivenToday)
                {
                    animal.Mood += 15;
                    animal.friendshipPoints += 20;
                }
                else
                {
                    animal.Mood -= 100;
                    animal.friendshipPoints -= 20;
                }

                animal.hasTalkedToday = false;
                animal.giftGivenToday = false;
                animal.givenProduceToday = false;
                animal.age++;
            }
        }

        public static AnimalData GetAnimalTypeFromString(string name) => animals.Find(i => i.name == name);
    }
}