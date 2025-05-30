using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BlossomValley.UISystem
{
    public class YesNoPrompt : MonoBehaviour
    {
        [SerializeField] private GameObject quantityControls;
        [SerializeField] private TextMeshProUGUI quantityText;
        [SerializeField] private TextMeshProUGUI promptText;
        [SerializeField] private Button plusButton;
        [SerializeField] private Button minusButton;

        private Action onYesSelected = null;
        private Action<int> onYesSelectedWithQuantity = null;

        private int currentQuantity = 1;
        private int maxQuantity = 1;

        private void Start()
        {
            plusButton.onClick.AddListener(IncreaseQuantity);
            minusButton.onClick.AddListener(DecreaseQuantity);
        }

        public void CreatePrompt(string message, Action onYesSelectedAction)
        {
            onYesSelected = onYesSelectedAction;
            onYesSelectedWithQuantity = null;

            promptText.text = message;
            quantityControls.SetActive(false);
            gameObject.SetActive(true);
        }

        public void CreateQuantityPrompt(string message, int maxQuantityValue, Action<int> onYesSelectedWithQuantityValue)
        {
            onYesSelected = null;
            onYesSelectedWithQuantity = onYesSelectedWithQuantityValue;
            maxQuantity = maxQuantityValue;
            currentQuantity = 1;

            promptText.text = message;
            UpdateQuantityDisplay();
            quantityControls.SetActive(true);
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
            minusButton.interactable = currentQuantity > 1;
            plusButton.interactable = currentQuantity < maxQuantity;
        }

        public void Answer(bool yes)
        {
            if (yes)
            {
                if (onYesSelected != null)
                    onYesSelected();
                else if (onYesSelectedWithQuantity != null)
                    onYesSelectedWithQuantity(currentQuantity);
            }

            onYesSelected = null;
            onYesSelectedWithQuantity = null;
            currentQuantity = 1;
            maxQuantity = 1;
            gameObject.SetActive(false);
        }
    }
}