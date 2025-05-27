using UnityEngine;

public class InteractableCharacter : InteractableObject
{
    public CharacterScriptableObject characterData;
    private NPCRelationshipState relationship;
    private CharacterRotationHandler rotationHandler;
    private DialogueContext dialogueContext;
    private DefaultDialogueStrategy defaultStrategy;
    private GiftDialogueStrategy giftStrategy;

    private void Start()
    {
        relationship = RelationshipStats.GetRelationship(characterData);
        rotationHandler = new CharacterRotationHandler(transform, this);
        
        // Initialize strategies
        dialogueContext = new DialogueContext();
        defaultStrategy = new DefaultDialogueStrategy();
        giftStrategy = new GiftDialogueStrategy();
    }

    public override void Pickup()
    {
        rotationHandler.LookAtPlayer();
        TriggerDialogue(); 
    }

    private void TriggerDialogue()
    {
        System.Action onComplete = () => {
            rotationHandler.ResetRotation();
            // Refresh relationship state after dialogue
            relationship = RelationshipStats.GetRelationship(characterData);
        };

        if (InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Item))
        {
            dialogueContext.SetStrategy(giftStrategy);
        }
        else
        {
            dialogueContext.SetStrategy(defaultStrategy);
        }

        dialogueContext.ExecuteDialogue(characterData, relationship, onComplete);
    }
}