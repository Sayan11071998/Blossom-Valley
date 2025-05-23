using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalRelationshipState : NPCRelationshipState
{
    //The type of animal this animal is 
    public string animalType;

    const int MAX_MOOD = 255;

    private int _mood; 
    public int Mood
    {
        get => _mood;
        set
        {
            _mood = Mathf.Clamp(value, 0, MAX_MOOD); 
        }
    }

    //Age in days
    public int age;

    public bool givenProduceToday;

    public AnimalRelationshipState(string name, AnimalData animalType) : base(name)
    {
        this.animalType = animalType.name;

        Mood = MAX_MOOD; 
    }

    public AnimalRelationshipState(string name, AnimalData animalType, int friendshipPoints): base(name, friendshipPoints)
    {
        this.animalType = animalType.name;
        Mood = MAX_MOOD;
    }

    public AnimalRelationshipState(string name, AnimalData animalType, int friendshipPoints, int mood) : base(name, friendshipPoints)
    {
        this.animalType = animalType.name;
        Mood = mood; 
    }

    //Convert the string back into AnimalData
    public AnimalData AnimalType()
    {
        return AnimalStats.GetAnimalTypeFromString(animalType); 
    } 
}
