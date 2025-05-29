using UnityEngine;

public class ParentedAnimationEvent : MonoBehaviour
{
     public void NotifyAncestors(string message) => SendMessageUpwards(message);
}