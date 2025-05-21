using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public enum Location { Farm, PlayerHome, Town }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SwitchLocation(Location locationToSwitch)
    {
        SceneManager.LoadScene(locationToSwitch.ToString());
    }
}