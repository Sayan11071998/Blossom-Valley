public interface IDialogueStrategy
{
    void ExecuteDialogue(CharacterScriptableObject characterData, NPCRelationshipState relationship, System.Action onComplete);
}