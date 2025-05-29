using System;
using UnityEngine;
using UnityEngine.UI;

public class NamingPrompt : MonoBehaviour
{
    [SerializeField] private Text promptText;
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