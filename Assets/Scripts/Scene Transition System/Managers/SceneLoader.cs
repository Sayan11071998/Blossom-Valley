using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlossomValley.SceneTransitionSystem
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(gameObject);
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void LoadScene(SceneTransitionManager.Location location) => SceneManager.LoadScene(location.ToString());
    }
}