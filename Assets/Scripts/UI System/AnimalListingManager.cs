using UnityEngine;
using BlossomValley.AnimalSystem;

public class AnimalListingManager : ListingManager<AnimalRelationshipState>
{
    protected override void DisplayListing(AnimalRelationshipState relationship, GameObject listingGameObject)
    {
        AnimalData animalData = AnimalStats.GetAnimalTypeFromString(relationship.animalType);
        listingGameObject.GetComponent<NPCRelationshipListing>().Display(animalData, relationship);
    }
}