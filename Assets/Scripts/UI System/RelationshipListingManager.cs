using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BlossomValley.CharacterSystem;

public class RelationshipListingManager : ListingManager<NPCRelationshipState>
{
    private List<CharacterScriptableObject> characters;

    protected override void DisplayListing(NPCRelationshipState relationship, GameObject listingGameObject)
    {
        if (characters == null)
            LoadAllCharacters();

        CharacterScriptableObject characterData = GetCharacterDataFromString(relationship.name);
        listingGameObject.GetComponent<NPCRelationshipListing>().Display(characterData, relationship);
    }

    public CharacterScriptableObject GetCharacterDataFromString(string name) => characters.Find(i => i.name == name);

    private void LoadAllCharacters()
    {
        CharacterScriptableObject[] characterDatabase = Resources.LoadAll<CharacterScriptableObject>("Characters");
        characters = characterDatabase.ToList();
    }
}