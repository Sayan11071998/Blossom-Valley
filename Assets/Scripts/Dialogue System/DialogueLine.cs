using UnityEngine;

namespace BlossomValley.DialogueSystem
{
    [System.Serializable]
    public struct DialogueLine
    {
        public string speaker;
        [TextArea(2, 5)]
        public string message;

        public DialogueLine(string speakerValue, string messageValue)
        {
            speaker = speakerValue;
            message = messageValue;
        }
    }
}