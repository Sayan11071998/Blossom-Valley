using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public enum Location { Farm, PlayerHome, Town }

    public Location currentLocation;

    Transform playerPoint;

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
        SceneManager.sceneLoaded += OnLocationLoad;
        playerPoint = FindAnyObjectByType<PlayerController>().transform;
    }

    public void SwitchLocation(Location locationToSwitch)
    {
        SceneManager.LoadScene(locationToSwitch.ToString());
    }

    public void OnLocationLoad(Scene scene, LoadSceneMode mode)
    {
        Location oldLocation = currentLocation;
        Location newLocation = (Location)Enum.Parse(typeof(Location), scene.name);

        if (newLocation == currentLocation) return;

        Transform startPoint = LocationManager.Instance.GetPlayerStartingPosition(oldLocation);
        CharacterController playerCharacter = playerPoint.GetComponent<CharacterController>();
        playerCharacter.enabled = false;
        playerPoint.position = startPoint.position;
        playerPoint.rotation = startPoint.rotation;
        playerCharacter.enabled = true;
        currentLocation = newLocation;
    }
}