using System.Collections.Generic;
using UnityEngine;
using BlossomValley.InventorySystem;
using BlossomValley.TimeSystem;

namespace BlossomValley.CharacterSystem
{
    public class RelationshipStats : MonoBehaviour
    {
        public static List<NPCRelationshipState> relationships = new List<NPCRelationshipState>();

        public enum GiftReaction
        {
            Like, Dislike, Neutral
        }

        public static void ResetAllRelationships() => relationships = new List<NPCRelationshipState>();

        public static void LoadStats(List<NPCRelationshipState> relationshipsToLoad)
        {
            if (relationshipsToLoad == null)
            {
                relationships = new List<NPCRelationshipState>();
                return;
            }
            relationships = relationshipsToLoad;
        }

        public static bool FirstMeeting(CharacterScriptableObject character) => !relationships.Exists(i => i.name == character.name);

        public static NPCRelationshipState GetRelationship(CharacterScriptableObject character)
        {
            if (FirstMeeting(character)) return null;
            return relationships.Find(i => i.name == character.name);
        }

        public static void UnlockCharacter(CharacterScriptableObject character) => relationships.Add(new NPCRelationshipState(character.name));

        public static void AddFriendPoints(CharacterScriptableObject character, int points)
        {
            if (FirstMeeting(character)) return;
            GetRelationship(character).friendshipPoints += points;
        }

        public static bool IsFirstConversationOfTheDay(CharacterScriptableObject character)
        {
            if (FirstMeeting(character)) return true;
            NPCRelationshipState npc = GetRelationship(character);
            return !npc.hasTalkedToday;
        }

        public static bool GiftGivenToday(CharacterScriptableObject character)
        {
            NPCRelationshipState npc = GetRelationship(character);
            return npc.giftGivenToday;
        }

        public static GiftReaction GetReactionToGift(CharacterScriptableObject character, ItemData item)
        {
            if (character.likes.Contains(item)) return GiftReaction.Like;
            if (character.dislikes.Contains(item)) return GiftReaction.Dislike;
            return GiftReaction.Neutral;
        }

        public static bool IsBirthday(CharacterScriptableObject character)
        {
            GameTimestamp birthday = character.birthday;
            GameTimestamp today = TimeManager.Instance.GetGameTimestamp();
            return (today.day == birthday.day) && (today.season == birthday.season);
        }

        public static CharacterScriptableObject WhoseBirthday(GameTimestamp timestamp)
        {
            InteractableCharacter[] allCharacters = Object.FindObjectsByType<InteractableCharacter>(FindObjectsSortMode.None);

            foreach (InteractableCharacter character in allCharacters)
            {
                CharacterScriptableObject characterData = character.CharacterData;

                if (characterData != null && characterData.birthday.day == timestamp.day && characterData.birthday.season == timestamp.season)
                    return characterData;
            }

            return null;
        }
    }
}