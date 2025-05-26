using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;
    public enum Location { Farm, PlayerHome, Town, ChickenCoop }
    public Location currentLocation;
    
    Transform playerPoint;
    bool screenFadedOut;
    PlayerPositioner playerPositioner;

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
        playerPoint = FindAnyObjectByType<PlayerView>().transform;
        
        // Initialize player positioner with strategy
        playerPositioner = new PlayerPositioner(new StartPointPositionStrategy());
        
        // Subscribe to fade events
        FadeEventManager.OnFadeOutComplete += OnFadeOutComplete;
    }
    
    private void OnDestroy()
    {
        // Unsubscribe from events
        FadeEventManager.OnFadeOutComplete -= OnFadeOutComplete;
    }

    public bool CurrentlyIndoor()
    {
        return LocationTypeChecker.IsIndoor(currentLocation);
    }

    public void SwitchLocation(Location locationToSwitch)
    {
        UIManager.Instance.FadeOutScreen();
        screenFadedOut = false;
        StartCoroutine(ChangeScene(locationToSwitch)); 
    }

    IEnumerator ChangeScene(Location locationToSwitch)
    {
        CharacterController playerCharacter = playerPoint.GetComponent<CharacterController>();
        playerCharacter.enabled = false;
        while (!screenFadedOut)
        {
            yield return new WaitForSeconds(0.1f); 
        }
        screenFadedOut = false;
        UIManager.Instance.ResetFadeDefaults();
        SceneLoader.Instance.LoadScene(locationToSwitch);
    }

    public void OnFadeOutComplete()
    {
        screenFadedOut = true;
    }

    public void OnLocationLoad(Scene scene, LoadSceneMode mode)
    {
        Location oldLocation = currentLocation;
        Location newLocation = (Location) Enum.Parse(typeof(Location), scene.name);
        if (currentLocation == newLocation) return; 
        
        if (playerPoint == null) return;
        
        // Use player positioner to handle positioning logic
        playerPositioner.PositionPlayer(playerPoint, oldLocation);
        currentLocation = newLocation; 
    }
}
