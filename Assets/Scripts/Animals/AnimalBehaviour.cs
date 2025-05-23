using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimalMovement))]
public class AnimalBehaviour : InteractableObject
{
    AnimalRelationshipState relationship;
    AnimalMovement movement;

    private void Start()
    {
        movement = GetComponent<AnimalMovement>();
    }

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
        movement.ToggleMovement(false);
        DialogueManager.Instance.StartDialogue(DialogueManager.CreateSimpleMessage($"{relationship.name} seems happy."),
            () => {
                movement.ToggleMovement(true);
            }
            );
    }
}
