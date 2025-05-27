using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; 

public class RelationshipListingManager : ListingManager<NPCRelationshipState>
{
    //The full database of characters
    List<CharacterScriptableObject> characters; 

    protected override void DisplayListing(NPCRelationshipState relationship, GameObject listingGameObject)
    {
        //Load and cache all the characters if it hasn't already been loaded
        if(characters == null)
        {
            LoadAllCharacters(); 
        }
        CharacterScriptableObject characterData = GetCharacterDataFromString(relationship.name);
        listingGameObject.GetComponent<NPCRelationshipListing>().Display(characterData, relationship); 
    }

    public CharacterScriptableObject GetCharacterDataFromString(string name)
    {
        return characters.Find(i => i.name == name);
    }

    //Load all characters from the resource folder
    void LoadAllCharacters()
    {
        CharacterScriptableObject[] characterDatabase = Resources.LoadAll<CharacterScriptableObject>("Characters");
        characters = characterDatabase.ToList();
    }
}
