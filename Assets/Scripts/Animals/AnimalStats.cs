using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimalStats : MonoBehaviour
{

    //The relationship data of all the NPCs that the player has met in the game
    public static List<AnimalRelationshipState> animalRelationships = new List<AnimalRelationshipState>();

    //Load all the animal data scriptable objects
    static List<AnimalData> animals = Resources.LoadAll<AnimalData>("Animals").ToList();

    //To be fired up when a new animal is born or purchased
    public static void StartAnimalCreation(AnimalData animalType)
    {
        //Handle Chicken spawning here
        UIManager.Instance.TriggerNamingPrompt($"Give your new {animalType.name} a name.", (inputString) => {
            //Create a new animal and add it to the animal relationships data
            animalRelationships.Add(new AnimalRelationshipState(inputString, animalType));
        });
    }

    //Load in the animal relationships
    public static void LoadStats(List<AnimalRelationshipState> relationshipsToLoad)
    {
        Debug.Log("Animals: " + relationshipsToLoad.Count); 
        if(relationshipsToLoad == null)
        {
            animalRelationships = new List<AnimalRelationshipState>();
            return; 
        }
        animalRelationships = relationshipsToLoad; 
    }

    //Get the animals by type
    public static List<AnimalRelationshipState> GetAnimalsByType(string animalTypeName)
    {
        return animalRelationships.FindAll(x => x.animalType == animalTypeName);
    }

    public static List<AnimalRelationshipState> GetAnimalsByType(AnimalData animalType)
    {
        return GetAnimalsByType(animalType.name);
    }

    public static void OnDayReset()
    {
        //Reset animal relationship states
        foreach (AnimalRelationshipState animal in AnimalStats.animalRelationships)
        {
            //Increase friendship if player has spoken with the animal
            if (animal.hasTalkedToday)
            {
                animal.friendshipPoints += 30;
            } else
            {
                animal.friendshipPoints -= (10 - (animal.friendshipPoints / 200));
            }

            //Feeding
            if (animal.giftGivenToday)
            {
                animal.Mood += 15;
            }
            else
            {
                animal.Mood -= 100;
                animal.friendshipPoints -= 20; 
            }

            animal.hasTalkedToday = false;
            //Gift given refers to whether the animal has been fed
            animal.giftGivenToday = false;
            animal.givenProduceToday = false;

            //Advance the age of the animal 
            animal.age++;
        }
    }

    //Get the animal data type from a string
    public static AnimalData GetAnimalTypeFromString(string name)
    {
        return animals.Find(i => i.name == name);
    }

}
