using BlossomValley.GameStrings;
using UnityEngine;
using BlossomValley.DialogueSystem;

namespace BlossomValley.AnimalSystem
{
    [RequireComponent(typeof(AnimalMovement))]
    public class AnimalBehaviour : InteractableObject
    {
        [SerializeField] protected WorldBubble speechBubble;

        protected AnimalRelationshipState relationship;
        protected AnimalMovement movement;
        protected AnimalRenderer animalRenderer;

        protected virtual void Start() => movement = GetComponent<AnimalMovement>();

        public void LoadRelationship(AnimalRelationshipState relationshipToLoad)
        {
            relationship = relationshipToLoad;
            animalRenderer = GetComponent<AnimalRenderer>();
            animalRenderer.RenderAnimal(relationshipToLoad.age, relationshipToLoad.animalType);
        }

        public override void Pickup()
        {
            if (relationship == null) return;
            TriggerDialogue();
        }

        void TriggerDialogue()
        {
            movement.ToggleMovement(false);

            int mood = relationship.Mood;
            string dialogueLine = string.Format(GameString.RelationshipStatusPrefix, relationship.name);

            System.Action onDialogueEnd = () =>
            {
                movement.ToggleMovement(true);
            };

            if (!relationship.hasTalkedToday)
                onDialogueEnd += OnFirstConversation;

            if (mood >= 200 && mood <= 255)
                dialogueLine += GameString.MoodHappy;
            else if (mood >= 30 && mood < 200)
                dialogueLine += GameString.MoodNeutral;
            else
                dialogueLine += GameString.MoodSad;

            DialogueManager.Instance.StartDialogue(DialogueManager.CreateSimpleMessage(dialogueLine), onDialogueEnd);
        }

        void OnFirstConversation()
        {
            relationship.Mood += 30;
            relationship.hasTalkedToday = true;
            speechBubble.gameObject.SetActive(true);
            WorldBubble.Emote emote = WorldBubble.Emote.Thinking;

            switch (relationship.Mood)
            {
                case int n when (n >= 200):
                    emote = WorldBubble.Emote.Heart;
                    break;
                case int n when (n < 30):
                    emote = WorldBubble.Emote.Sad;
                    break;
                case int n when (n >= 30 && n < 60):
                    emote = WorldBubble.Emote.BadMood;
                    break;
                default:
                    emote = WorldBubble.Emote.Happy;
                    break;

            }

            speechBubble.Display(emote, 3f);
        }
    }
}