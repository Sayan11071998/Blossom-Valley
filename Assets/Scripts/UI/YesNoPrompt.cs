using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class YesNoPrompt : MonoBehaviour
{
    [SerializeField] Text promptText;
    [SerializeField] Text quantityText;
    [SerializeField] Button plusButton;
    [SerializeField] Button minusButton;
    [SerializeField] GameObject quantityControls; // Parent object containing quantity UI elements
    
    Action onYesSelected = null;
    Action<int> onYesSelectedWithQuantity = null;
    
    private int currentQuantity = 1;
    private int maxQuantity = 1;

    void Start()
    {
        // Set up button listeners
        plusButton.onClick.AddListener(IncreaseQuantity);
        minusButton.onClick.AddListener(DecreaseQuantity);
    }

    // Original method for simple yes/no prompts
    public void CreatePrompt(string message, Action onYesSelected)
    {
        this.onYesSelected = onYesSelected;
        this.onYesSelectedWithQuantity = null;
        
        promptText.text = message;
        quantityControls.SetActive(false); // Hide quantity controls for simple prompts
        gameObject.SetActive(true);
    }

    // New method for quantity-based prompts
    public void CreateQuantityPrompt(string message, int maxQuantity, Action<int> onYesSelectedWithQuantity)
    {
        this.onYesSelected = null;
        this.onYesSelectedWithQuantity = onYesSelectedWithQuantity;
        this.maxQuantity = maxQuantity;
        this.currentQuantity = 1;
        
        promptText.text = message;
        UpdateQuantityDisplay();
        quantityControls.SetActive(true); // Show quantity controls
        gameObject.SetActive(true);
    }

    private void IncreaseQuantity()
    {
        if (currentQuantity < maxQuantity)
        {
            currentQuantity++;
            UpdateQuantityDisplay();
        }
    }

    private void DecreaseQuantity()
    {
        if (currentQuantity > 1)
        {
            currentQuantity--;
            UpdateQuantityDisplay();
        }
    }

    private void UpdateQuantityDisplay()
    {
        quantityText.text = currentQuantity.ToString();
        
        // Update button interactability
        minusButton.interactable = currentQuantity > 1;
        plusButton.interactable = currentQuantity < maxQuantity;
    }

    public void Answer(bool yes)
    {
        if (yes)
        {
            // Execute appropriate action based on which type of prompt this is
            if (onYesSelected != null)
            {
                onYesSelected();
            }
            else if (onYesSelectedWithQuantity != null)
            {
                onYesSelectedWithQuantity(currentQuantity);
            }
        }

        // Reset everything
        onYesSelected = null;
        onYesSelectedWithQuantity = null;
        currentQuantity = 1;
        maxQuantity = 1;
        
        gameObject.SetActive(false);
    }
}