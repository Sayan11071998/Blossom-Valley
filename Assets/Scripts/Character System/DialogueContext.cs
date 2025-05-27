public class DialogueContext
{
    private IDialogueStrategy strategy;

    public void SetStrategy(IDialogueStrategy strategyToSet) => strategy = strategyToSet;

    public void ExecuteDialogue(CharacterScriptableObject characterData, NPCRelationshipState relationship, System.Action onComplete) => strategy?.ExecuteDialogue(characterData, relationship, onComplete);
}