using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public enum Location { Farm, PlayerHome, Town }

    public Location currentLocation;
    public Location previousLocation;

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
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLocationLoad;
    }

    public void SwitchLocation(Location locationToSwitch)
    {
        previousLocation = currentLocation;
        SceneManager.LoadScene(locationToSwitch.ToString());
    }

    public void OnLocationLoad(Scene scene, LoadSceneMode mode)
    {
        // Parse the new location from the scene name
        Location newLocation;
        if (Enum.TryParse(scene.name, out newLocation))
        {
            // Only process if we're actually changing location
            if (newLocation != currentLocation)
            {
                // Find the player in the new scene
                PlayerController player = FindAnyObjectByType<PlayerController>();
                
                if (player != null)
                {
                    // Get the starting position based on where we came from
                    Transform startPoint = LocationManager.Instance?.GetPlayerStartingPosition(previousLocation);
                    
                    if (startPoint != null)
                    {
                        // Get the character controller and position the player
                        CharacterController playerCharacter = player.GetComponent<CharacterController>();
                        
                        if (playerCharacter != null)
                        {
                            playerCharacter.enabled = false;
                            player.transform.position = startPoint.position;
                            player.transform.rotation = startPoint.rotation;
                            playerCharacter.enabled = true;
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"No start point found for previous location: {previousLocation}");
                    }
                }
                else
                {
                    Debug.LogError("PlayerController not found in the new scene!");
                }
                
                currentLocation = newLocation;
            }
        }
        else
        {
            Debug.LogError($"Could not parse scene name '{scene.name}' into a Location!");
        }
    }
}