using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBehaviour : InteractableObject
{
    AnimalRelationshipState relationship; 

    public void LoadRelationship(AnimalRelationshipState relationship)
    {
        this.relationship = relationship; 
    }

    public override void Pickup()
    {
        if(relationship == null)
        {
            Debug.LogError("Relationship not set");
            return; 
        }

        DialogueManager.Instance.StartDialogue(DialogueManager.CreateSimpleMessage($"{relationship.name} seems happy."));
    }
}
