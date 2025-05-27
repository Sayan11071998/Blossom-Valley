using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipStats : MonoBehaviour
{
    //The relationship data of all the NPCs that the player has met in the game
    public static List<NPCRelationshipState> relationships = new List<NPCRelationshipState>();
    
    public enum GiftReaction
    {
        Like, Dislike, Neutral
    }

    // Add this method to reset all relationship data for new games
    public static void ResetAllRelationships()
    {
        relationships = new List<NPCRelationshipState>();
        Debug.Log("All relationship data has been reset for new game");
    }

    public static void LoadStats(List<NPCRelationshipState> relationshipsToLoad)
    {
        if (relationshipsToLoad == null)
        {
            relationships = new List<NPCRelationshipState>();
            return; 
        }
        relationships = relationshipsToLoad;
    }

    //Check if the player has met this NPC. 
    public static bool FirstMeeting(CharacterScriptableObject character)
    {
        return !relationships.Exists(i => i.name == character.name);
    }

    //Get relationship information about a character
    public static NPCRelationshipState GetRelationship(CharacterScriptableObject character)
    {
        //Check if it is the first meeting
        if (FirstMeeting(character)) return null;

        return relationships.Find(i => i.name == character.name);
    }

    //Add the character to the relationships data
    public static void UnlockCharacter(CharacterScriptableObject character)
    {
        relationships.Add(new NPCRelationshipState(character.name)); 
    }

    //Improve the relationship with an NPC
    public static void AddFriendPoints(CharacterScriptableObject character, int points)
    {
        if (FirstMeeting(character))
        {
            Debug.LogError("The player has not met this character yet!"); 
            return; 
        }

        GetRelationship(character).friendshipPoints += points;
    }

    //Check if this is the first conversation the player is having with the NPC today
    public static bool IsFirstConversationOfTheDay(CharacterScriptableObject character)
    {
        //If the player is meeting him for the first time, definitely is
        if (FirstMeeting(character)) return true;

        NPCRelationshipState npc = GetRelationship(character);
        return !npc.hasTalkedToday; 
    }

    //Check if the player has already given this character a gift today
    public static bool GiftGivenToday(CharacterScriptableObject character)
    {
        NPCRelationshipState npc = GetRelationship(character);
        return npc.giftGivenToday; 
    }

    public static GiftReaction GetReactionToGift(CharacterScriptableObject character, ItemData item)
    {
        //If it's in the list of liked items, it means the character likes it
        if (character.likes.Contains(item)) return GiftReaction.Like;
        //If it's in the list of disliked items, it means the character dislikes it
        if (character.dislikes.Contains(item)) return GiftReaction.Dislike;

        return GiftReaction.Neutral; 
    }

    //Check if it's the character's birthday
    public static bool IsBirthday(CharacterScriptableObject character)
    {
        GameTimestamp birthday = character.birthday;
        GameTimestamp today = TimeManager.Instance.GetGameTimestamp();

        return (today.day == birthday.day) && (today.season == birthday.season);
    }
}