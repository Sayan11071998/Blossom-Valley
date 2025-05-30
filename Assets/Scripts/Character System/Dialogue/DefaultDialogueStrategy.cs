using System.Collections.Generic;
using BlossomValley.DialogueSystem;

namespace BlossomValley.CharacterSystem
{
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
                onDialogueEnd += () =>
                {
                    RelationshipStats.UnlockCharacter(characterData);
                };
            }

            if (isFirstConversationOfDay)
            {
                onDialogueEnd += () =>
                {
                    RelationshipStats.AddFriendPoints(characterData, 20);
                    NPCRelationshipState updatedRelationship = RelationshipStats.GetRelationship(characterData);
                    if (updatedRelationship != null)
                        updatedRelationship.hasTalkedToday = true;
                };
            }

            DialogueManager.Instance.StartDialogue(dialogueToHave, onDialogueEnd);
        }
    }
}