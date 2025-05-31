using System.Collections;
using UnityEngine;

namespace BlossomValley.UISystem
{
    public class WorldBubble : WorldUI
    {

        [SerializeField] private Animator speechAnimator;

        public enum Emote
        {
            Happy, BadMood, Heart, Thinking, Sad
        }

        public void Display(Emote mood)
        {
            ResetAnimator();
            speechAnimator.SetBool(mood.ToString(), true);
        }

        public void Display(Emote mood, float time)
        {
            Display(mood);
            StartCoroutine(Delay(time));
        }

        private IEnumerator Delay(float time)
        {
            yield return new WaitForSeconds(time);
            ResetAnimator();
            gameObject.SetActive(false);
        }

        private void ResetAnimator()
        {
            foreach (AnimatorControllerParameter param in speechAnimator.parameters)
                speechAnimator.SetBool(param.name, false);
        }

        private void OnDisable() => ResetAnimator();
    }
}