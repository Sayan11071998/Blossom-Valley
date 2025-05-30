namespace BlossomValley.SceneTransitionSystem
{
    public static class FadeEventManager
    {
        public static event System.Action OnFadeOutComplete;
        public static event System.Action OnFadeInComplete;

        public static void NotifyFadeOutComplete() => OnFadeOutComplete?.Invoke();
        public static void NotifyFadeInComplete() => OnFadeInComplete?.Invoke();
    }
}