using System.Collections.Generic;
using UnityEngine;

public class GiftDialogueStrategy : IDialogueStrategy
{
    public void ExecuteDialogue(CharacterScriptableObject characterData, NPCRelationshipState relationship, System.Action onComplete)
    {
        if (!IsEligibleForGift(characterData, onComplete))
            return;

        ItemSlotData handSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item);
        List<DialogueLine> dialogueToHave = characterData.neutralGiftDialogue;

        System.Action onDialogueEnd = () =>
        {
            // Get the current relationship state (in case it was updated)
            NPCRelationshipState currentRelationship = RelationshipStats.GetRelationship(characterData);
            if (currentRelationship != null)
            {
                currentRelationship.giftGivenToday = true;
            }
            InventoryManager.Instance.ConsumeItem(handSlot);
        };
        onDialogueEnd += onComplete;

        bool isBirthday = RelationshipStats.IsBirthday(characterData);
        int pointsToAdd = 0;

        switch (RelationshipStats.GetReactionToGift(characterData, handSlot.itemData))
        {
            case RelationshipStats.GiftReaction.Like:
                dialogueToHave = characterData.likedGiftDialogue;
                pointsToAdd = 80;
                if (isBirthday) dialogueToHave = characterData.birthdayLikedGiftDialogue;
                break;
            case RelationshipStats.GiftReaction.Dislike:
                dialogueToHave = characterData.dislikedGiftDialogue;
                pointsToAdd = -20;
                if (isBirthday) dialogueToHave = characterData.birthdayDislikedGiftDialogue;
                break;
            case RelationshipStats.GiftReaction.Neutral:
                dialogueToHave = characterData.neutralGiftDialogue;
                pointsToAdd = 20;
                if (isBirthday) dialogueToHave = characterData.birthdayNeutralGiftDialogue;
                break; 
        }
        if (isBirthday) pointsToAdd *= 8;

        RelationshipStats.AddFriendPoints(characterData, pointsToAdd);
        DialogueManager.Instance.StartDialogue(dialogueToHave, onDialogueEnd);
    }

    private bool IsEligibleForGift(CharacterScriptableObject characterData, System.Action onComplete)
    {
        if (RelationshipStats.FirstMeeting(characterData))
        {
            DialogueManager.Instance.StartDialogue(DialogueManager.CreateSimpleMessage("You have not unlocked this character yet."), onComplete); 
            return false; 
        }

        if (RelationshipStats.GiftGivenToday(characterData))
        {
            DialogueManager.Instance.StartDialogue(DialogueManager.CreateSimpleMessage($"You have already given {characterData.name} a gift today."), onComplete);
            return false; 
        }
        return true; 
    }
}