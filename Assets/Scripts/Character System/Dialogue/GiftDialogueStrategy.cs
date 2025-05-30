using System.Collections.Generic;
using BlossomValley.Utilities;
using BlossomValley.DialogueSystem;
using BlossomValley.InventorySystem;
using BlossomValley.UISystem;

namespace BlossomValley.CharacterSystem
{
    public class GiftDialogueStrategy : IDialogueStrategy
    {
        public void ExecuteDialogue(CharacterScriptableObject characterData, NPCRelationshipState relationship, System.Action onComplete)
        {
            if (!IsEligibleForGift(characterData, onComplete)) return;

            ItemSlotData handSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item);
            List<DialogueLine> dialogueToHave = characterData.neutralGiftDialogue;

            System.Action onDialogueEnd = () =>
            {
                NPCRelationshipState currentRelationship = RelationshipStats.GetRelationship(characterData);

                if (currentRelationship != null)
                    currentRelationship.giftGivenToday = true;

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
                    if (isBirthday)
                        dialogueToHave = characterData.birthdayLikedGiftDialogue;
                    break;
                case RelationshipStats.GiftReaction.Dislike:
                    dialogueToHave = characterData.dislikedGiftDialogue;
                    pointsToAdd = -20;
                    if (isBirthday)
                        dialogueToHave = characterData.birthdayDislikedGiftDialogue;
                    break;
                case RelationshipStats.GiftReaction.Neutral:
                    dialogueToHave = characterData.neutralGiftDialogue;
                    pointsToAdd = 20;
                    if (isBirthday)
                        dialogueToHave = characterData.birthdayNeutralGiftDialogue;
                    break;
            }

            if (isBirthday)
                pointsToAdd *= 8;

            RelationshipStats.AddFriendPoints(characterData, pointsToAdd);
            DialogueManager.Instance.StartDialogue(dialogueToHave, onDialogueEnd);
        }

        private bool IsEligibleForGift(CharacterScriptableObject characterData, System.Action onComplete)
        {
            if (RelationshipStats.FirstMeeting(characterData))
            {
                DialogueManager.Instance.StartDialogue(DialogueManager.CreateSimpleMessage(GameString.CharacterNotUnlockedPrompt), onComplete);
                return false;
            }

            if (RelationshipStats.GiftGivenToday(characterData))
            {
                string message = string.Format(GameString.AlreadyGiftedMessage, characterData.name);
                DialogueManager.Instance.StartDialogue(DialogueManager.CreateSimpleMessage(message), onComplete);
                return false;
            }

            return true;
        }
    }
}