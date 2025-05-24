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
    static readonly Location[] indoor = { Location.PlayerHome, Location.ChickenCoop };
    Transform playerPoint;
    bool screenFadedOut;

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
    }

    public bool CurrentlyIndoor()
    {
        return indoor.Contains(currentLocation); 
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
        SceneManager.LoadScene(locationToSwitch.ToString());
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
        Transform startPoint = LocationManager.Instance.GetPlayerStartingPosition(oldLocation);
        if (playerPoint == null) return; 
        CharacterController playerCharacter = playerPoint.GetComponent<CharacterController>();
        playerCharacter.enabled = false; 
        playerPoint.position = startPoint.position;
        playerPoint.rotation = startPoint.rotation;
        playerCharacter.enabled = true;
        currentLocation = newLocation; 
    }
}