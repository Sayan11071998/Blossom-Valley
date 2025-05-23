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
        if (relationship == null)
        {
            Debug.LogError("Relationship not set");
            return;
        }
        TriggerDialogue();
    }

    void TriggerDialogue()
    {
        movement.ToggleMovement(false);

        //Get the mood
        int mood = relationship.Mood;

        //The dialogue string to output in the end 
        string dialogueLine = $"{relationship.name} seems ";

        //The action to execute upon dialogue end
        System.Action onDialogueEnd = () =>
        {
            movement.ToggleMovement(true);
        };

        //Check if the player has talked with the animal
        if (!relationship.hasTalkedToday)
        {
            onDialogueEnd += OnFirstConversation; 
        }

        if(mood >= 200 && mood <= 255)
        {
            dialogueLine += "really happy today!";
        } else if(mood >= 30 && mood < 200)
        {
            dialogueLine += "fine.";
        } else {
            dialogueLine += "sad"; 
        }

        DialogueManager.Instance.StartDialogue(DialogueManager.CreateSimpleMessage(dialogueLine), onDialogueEnd);
    }

    void OnFirstConversation()
    {
        relationship.Mood += 30;
        relationship.hasTalkedToday = true;

        Debug.Log($"{relationship.name} is now of mood {relationship.Mood}");

    }
}
