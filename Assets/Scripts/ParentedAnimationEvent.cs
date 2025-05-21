using UnityEngine;

public class ParentedAnimationEvent : MonoBehaviour
{
    public void NotifyAncenstors(string message)
    {
        SendMessageUpwards(message);
    }
}