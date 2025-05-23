using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalRelationshipState : NPCRelationshipState
{
    //The type of animal this animal is 
    public string animalType;

    public AnimalRelationshipState(string name, AnimalData animalType) : base(name)
    {
        this.animalType = animalType.name;
    }

    public AnimalRelationshipState(string name, AnimalData animalType, int friendshipPoints): base(name, friendshipPoints)
    {
        this.animalType = animalType.name; 
    }

    //Convert the string back into AnimalData
    public AnimalData AnimalType()
    {
        return AnimalStats.GetAnimalTypeFromString(animalType); 
    } 
}
