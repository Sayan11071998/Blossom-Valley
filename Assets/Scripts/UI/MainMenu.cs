using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public Button loadGameButton; 

    public void NewGame()
    {
        ClearSaveFile();
        StartCoroutine(LoadGameAsync(SceneTransitionManager.Location.PlayerHome, InitializeNewGame));
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadGameAsync(SceneTransitionManager.Location.PlayerHome, LoadGame));
    }

    // Clear the save file to ensure a fresh start
    void ClearSaveFile()
    {
        string filePath = Application.persistentDataPath + "/Save.save";
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("Save file cleared for new game");
        }
    }

    // Initialize a completely new game state
    void InitializeNewGame()
    {
        if(GameStateManager.Instance == null)
        {
            Debug.LogError("Cannot find Game State Manager!");
            return;
        }

        // Clear any existing farm data
        LandManager.farmData = null;
        
        // Reset all relationship data for new game
        RelationshipStats.ResetAllRelationships();
        
        // If you have AnimalStats as well, you might want to reset that too
        // AnimalStats.ResetAllAnimalRelationships(); // if such method exists
        
        // You might also want to reset other managers here
        // For example:
        // InventoryManager.Instance.ClearInventory(); // if such method exists
        // TimeManager.Instance.ResetToDefault(); // if such method exists
        
        Debug.Log("New game initialized with fresh state");
    }

    //To be called after the scene is loaded for continuing a game
    void LoadGame()
    {
        //Confirm if the GameStateManager is there (It should be if the scene is loaded)
        if(GameStateManager.Instance == null)
        {
            Debug.LogError("Cannot find Game State Manager!");
            return;
        }
        GameStateManager.Instance.LoadSave();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadGameAsync(SceneTransitionManager.Location scene, Action onFirstFrameLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.ToString());
        //Make this GameObject persistent so it can continue to run after the scene is loaded
        DontDestroyOnLoad(gameObject);
        //Wait for the scene to load
        while (!asyncLoad.isDone)
        {
            yield return null;
            Debug.Log("Loading"); 
        }

        //Scene Loaded
        Debug.Log("Loaded!");

        yield return new WaitForEndOfFrame();
        Debug.Log("First frame is loaded");
        //If there is an Action assigned, call it
        onFirstFrameLoad?.Invoke(); 

        //Done
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Disable or enable the Load Game button based on whether there is a save file
        loadGameButton.interactable = SaveManager.HasSave(); 
    }
}