using System.Collections.Generic;
using BlossomValley.DialogueSystem;
using UnityEngine;

namespace BlossomValley.CharacterSystem
{
    [CreateAssetMenu(fileName = "CharacterScriptableObject", menuName = "Character/Character")]
    public class CharacterScriptableObject : ScriptableObject
    {
        [Header("Character Information")]
        public Sprite portrait;
        public GameTimestamp birthday;
        public List<ItemData> likes;
        public List<ItemData> dislikes;

        [Header("Default Dialogues")]
        public List<DialogueLine> onFirstMeet;
        public List<DialogueLine> defaultDialogue;

        [Header("Gift Dialogues")]
        public List<DialogueLine> likedGiftDialogue;
        public List<DialogueLine> dislikedGiftDialogue;
        public List<DialogueLine> neutralGiftDialogue;

        [Header("Birthday Dialogues")]
        public List<DialogueLine> birthdayLikedGiftDialogue;
        public List<DialogueLine> birthdayDislikedGiftDialogue;
        public List<DialogueLine> birthdayNeutralGiftDialogue;
    }
}