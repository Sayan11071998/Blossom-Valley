using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlossomValley.UISystem
{
    public class NamingPrompt : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI promptText;
        [SerializeField] private InputField inputField;

        private Action<string> onConfirm;
        private Action onPromptComplete;

        public void CreatePrompt(string message, Action<string> onConfirm)
        {
            this.onConfirm = onConfirm;
            promptText.text = message;
        }

        public void QueuePromptAction(Action action) => onPromptComplete += action;

        public void Confirm()
        {
            onConfirm?.Invoke(inputField.text);
            onConfirm = null;
            inputField.text = "";
            gameObject.SetActive(false);
            onPromptComplete?.Invoke();
            onPromptComplete = null;
        }
    }
}