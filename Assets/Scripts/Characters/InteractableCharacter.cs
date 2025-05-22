using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCharacter : InteractableObject
{
    public CharacterData characterData;

    //Cache the relationship data of the NPC so we can access it
    NPCRelationshipState relationship;


    private void Start()
    {
        relationship = RelationshipStats.GetRelationship(characterData);
    }

    public override void Pickup()
    {
        List<DialogueLine> dialogueToHave = characterData.defaultDialogue;

        System.Action onDialogueEnd = null;

        //Do the checks to determine which dialogue to put out

        //Is the player meeting for the first time?
        if (RelationshipStats.FirstMeeting(characterData))
        {
            //Assign the first meet dialogue
            dialogueToHave = characterData.onFirstMeet;
            onDialogueEnd += OnFirstMeeting;

        }

        if (RelationshipStats.IsFirstConversationOfTheDay(characterData))
        {
            onDialogueEnd += OnFirstConversation; 
        }

        DialogueManager.Instance.StartDialogue(dialogueToHave, onDialogueEnd);
    }

    void OnFirstMeeting()
    {
        //Unlock the character on the relationships
        RelationshipStats.UnlockCharacter(characterData);
        //Update the relationship data
        relationship = RelationshipStats.GetRelationship(characterData);
    }

    void OnFirstConversation()
    {
        Debug.Log("This is the first conversation of the day");
        //Add 20 friend points 
        RelationshipStats.AddFriendPoints(characterData, 20);
 
        relationship.hasTalkedToday = true;
    }
}
