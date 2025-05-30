using UnityEngine;

namespace BlossomValley.Utilities
{
     public class ParentedAnimationEvent : MonoBehaviour
     {
          public void NotifyAncestors(string message) => SendMessageUpwards(message);
     }
}