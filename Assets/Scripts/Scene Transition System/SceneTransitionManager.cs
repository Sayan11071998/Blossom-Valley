using System;
using System.Collections;
using BlossomValley.PlayerSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public enum Location { Farm, PlayerHome, Town, ChickenCoop }

    public Location currentLocation;

    private Transform playerPoint;
    private bool screenFadedOut;
    private PlayerPositioner playerPositioner;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnLocationLoad;
        playerPoint = FindAnyObjectByType<PlayerView>().transform;
        playerPositioner = new PlayerPositioner(new StartPointPositionStrategy());
        FadeEventManager.OnFadeOutComplete += OnFadeOutComplete;
    }

    private void OnDestroy() => FadeEventManager.OnFadeOutComplete -= OnFadeOutComplete;

    public bool CurrentlyIndoor() => LocationTypeChecker.IsIndoor(currentLocation);

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
            yield return new WaitForSeconds(0.1f);

        screenFadedOut = false;
        UIManager.Instance.ResetFadeDefaults();
        SceneLoader.Instance.LoadScene(locationToSwitch);
    }

    public void OnFadeOutComplete() => screenFadedOut = true;

    public void OnLocationLoad(Scene scene, LoadSceneMode mode)
    {
        Location oldLocation = currentLocation;
        Location newLocation = (Location)Enum.Parse(typeof(Location), scene.name);

        if (currentLocation == newLocation) return;
        if (playerPoint == null) return;

        playerPositioner.PositionPlayer(playerPoint, oldLocation);
        currentLocation = newLocation;
    }
}