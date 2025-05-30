using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using BlossomValley.AnimalSystem;
using BlossomValley.IncubationSystem;
using BlossomValley.CharacterSystem;
using BlossomValley.LandSystem;
using BlossomValley.SaveSystem;
using BlossomValley.SceneTransitionSystem;

namespace BlossomValley.UISystem
{
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

        private void ClearSaveFile()
        {
            string filePath = Application.persistentDataPath + "/Save.save";

            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        private void InitializeNewGame()
        {
            if (GameStateManager.Instance == null) return;

            LandManager.farmData = null;
            RelationshipStats.ResetAllRelationships();
            AnimalStats.ResetAllAnimalRelationships();
            AnimalFeedManager.ResetFeedboxes();
            IncubationManager.eggsIncubating = new System.Collections.Generic.List<EggIncubationSaveState>();
        }

        private void LoadGame()
        {
            if (GameStateManager.Instance == null) return;
            GameStateManager.Instance.LoadSave();
        }

        public void QuitGame() => Application.Quit();

        IEnumerator LoadGameAsync(SceneTransitionManager.Location scene, Action onFirstFrameLoad)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.ToString());
            DontDestroyOnLoad(gameObject);

            while (!asyncLoad.isDone)
                yield return null;

            yield return new WaitForEndOfFrame();

            onFirstFrameLoad?.Invoke();
            Destroy(gameObject);
        }

        void Start() => loadGameButton.interactable = SaveManager.HasSave();
    }
}