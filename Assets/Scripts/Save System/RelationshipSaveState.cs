using System.Collections.Generic;
using BlossomValley.AnimalSystem;

[System.Serializable]
public class RelationshipSaveState
{
    public List<NPCRelationshipState> relationships;
    public List<AnimalRelationshipState> animals;

    public RelationshipSaveState(List<NPCRelationshipState> relationshipsValue, List<AnimalRelationshipState> animalsValue)
    {
        relationships = relationshipsValue;
        animals = animalsValue;
    }

    public static RelationshipSaveState Export() => new RelationshipSaveState(RelationshipStats.relationships, AnimalStats.animalRelationships);

    public void LoadData()
    {
        RelationshipStats.LoadStats(relationships);
        AnimalStats.LoadStats(animals);
    }
}