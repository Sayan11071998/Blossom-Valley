using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipStats : MonoBehaviour
{
    //The relationship data of all the NPCs that the player has met in the game
    public static List<NPCRelationshipState> relationships = new List<NPCRelationshipState>();
    

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
    public static bool FirstMeeting(CharacterData character)
    {
        return !relationships.Exists(i => i.name == character.name);
    }

    //Get relationship information about a character
    public static NPCRelationshipState GetRelationship(CharacterData character)
    {
        //Check if it is the first meeting
        if (FirstMeeting(character)) return null;

        return relationships.Find(i => i.name == character.name);
    }

    //Add the character to the relationships data
    public static void UnlockCharacter(CharacterData character)
    {
        relationships.Add(new NPCRelationshipState(character.name)); 
    }

    //Improve the relationship with an NPC
    public static void AddFriendPoints(CharacterData character, int points)
    {
        if (FirstMeeting(character))
        {
            Debug.LogError("The player has not met this character yet!"); 
            return; 
        }

        GetRelationship(character).friendshipPoints += points;
    }

    //Check if this is the first conversation the player is having with the NPC today
    public static bool IsFirstConversationOfTheDay(CharacterData character)
    {
        //If the player is meeting him for the first time, definitely is
        if (FirstMeeting(character)) return true;

        NPCRelationshipState npc = GetRelationship(character);
        return !npc.hasTalkedToday; 
    }

}
