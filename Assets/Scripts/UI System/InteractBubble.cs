using UnityEngine;
using TMPro;

namespace BlossomValley.UISystem
{
    public class InteractBubble : WorldUI
    {
        [SerializeField] private TextMeshProUGUI messageText;

        public void Display(string message) => messageText.text = message;
    }
}