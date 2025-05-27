using System.Collections.Generic;
using UnityEngine;

public class DefaultDialogueStrategy : IDialogueStrategy
{
    public void ExecuteDialogue(CharacterScriptableObject characterData, NPCRelationshipState relationship, System.Action onComplete)
    {
        List<DialogueLine> dialogueToHave = characterData.defaultDialogue;
        System.Action onDialogueEnd = onComplete;

        bool isFirstMeeting = RelationshipStats.FirstMeeting(characterData);
        bool isFirstConversationOfDay = RelationshipStats.IsFirstConversationOfTheDay(characterData);

        if (isFirstMeeting)
        {
            dialogueToHave = characterData.onFirstMeet;
            onDialogueEnd += () => {
                RelationshipStats.UnlockCharacter(characterData);
            };
        }

        if (isFirstConversationOfDay)
        {
            onDialogueEnd += () => {
                Debug.Log("This is the first conversation of the day");
                RelationshipStats.AddFriendPoints(characterData, 20);
                // Get the updated relationship after unlocking (in case it was first meeting)
                NPCRelationshipState updatedRelationship = RelationshipStats.GetRelationship(characterData);
                if (updatedRelationship != null)
                {
                    updatedRelationship.hasTalkedToday = true;
                }
            };
        }

        DialogueManager.Instance.StartDialogue(dialogueToHave, onDialogueEnd);
    }
}