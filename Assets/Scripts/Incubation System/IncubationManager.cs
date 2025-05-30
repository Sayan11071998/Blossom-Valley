using System.Collections.Generic;
using System.Linq;
using BlossomValley.AnimalSystem;
using BlossomValley.Utilities;
using UnityEngine;
using UnityEngine.Events;
using BlossomValley.UISystem;

namespace BlossomValley.IncubationSystem
{
    public class IncubationManager : MonoBehaviour
    {
        public static List<EggIncubationSaveState> eggsIncubating = new List<EggIncubationSaveState>();

        [SerializeField] public List<Incubator> incubators;

        [Header("Chicken Spawning")]
        [SerializeField] private float chickenWanderRadius = 3f;
        [SerializeField] private float spawnRadius = 1f;

        public const int daysToIncubate = 3;

        public static UnityEvent onEggUpdate = new UnityEvent();

        private void OnEnable()
        {
            RegisterIncubators();
            LoadIncubatorData();
            onEggUpdate.AddListener(LoadIncubatorData);
        }

        private void OnDestroy() => onEggUpdate.RemoveListener(LoadIncubatorData);

        public static void UpdateEggs()
        {
            if (eggsIncubating.Count == 0) return;

            IncubationManager manager = FindAnyObjectByType<IncubationManager>();

            foreach (EggIncubationSaveState egg in eggsIncubating.ToList())
            {
                egg.Tick();
                onEggUpdate?.Invoke();
                if (egg.timeToIncubate <= 0)
                {
                    eggsIncubating.Remove(egg);

                    if (manager != null)
                        manager.SpawnChickenNearIncubator(egg.incubatorID);
                    else
                    {
                        AnimalData chickenData = AnimalStats.GetAnimalTypeFromString("Chicken");
                        AnimalStats.StartAnimalCreation(chickenData);
                    }
                }
            }
        }

        private void SpawnChickenNearIncubator(int incubatorID)
        {
            if (incubatorID >= 0 && incubatorID < incubators.Count)
            {
                Incubator incubator = incubators[incubatorID];
                Vector3 incubatorPosition = incubator.transform.position;

                AnimalData chickenData = AnimalStats.GetAnimalTypeFromString("Chicken");

                string prompt = string.Format(GameString.AnimalNamingPrompt, chickenData.name);
                UIManager.Instance.TriggerNamingPrompt(prompt, (inputString) =>
                {
                    AnimalRelationshipState newChicken = new AnimalRelationshipState(inputString, chickenData);
                    AnimalStats.animalRelationships.Add(newChicken);
                    SpawnChickenAtLocation(newChicken, incubatorPosition);
                });

            }
        }

        private void SpawnChickenAtLocation(AnimalRelationshipState chickenRelation, Vector3 nearPosition)
        {
            AnimalData chickenData = chickenRelation.AnimalType();

            Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
            randomOffset.y = 0;
            Vector3 spawnPosition = nearPosition + randomOffset;

            UnityEngine.AI.NavMeshHit hit;

            if (UnityEngine.AI.NavMesh.SamplePosition(spawnPosition, out hit, spawnRadius * 2, UnityEngine.AI.NavMesh.AllAreas))
                spawnPosition = hit.position;

            float randomYRotation = Random.Range(0f, 360f);
            Quaternion randomRotation = Quaternion.Euler(0f, randomYRotation, 0f);

            AnimalBehaviour chicken = Instantiate(chickenData.animalObject, spawnPosition, randomRotation);
            chicken.LoadRelationship(chickenRelation);

            AnimalMovement movement = chicken.GetComponent<AnimalMovement>();
            if (movement != null)
            {
                movement.SetWanderRadius(chickenWanderRadius);
                movement.SetWanderCenter(nearPosition);
            }
        }

        private void RegisterIncubators()
        {
            for (int i = 0; i < incubators.Count; i++)
                incubators[i].incubationID = i;
        }

        private void LoadIncubatorData()
        {
            if (eggsIncubating.Count == 0) return;

            foreach (EggIncubationSaveState egg in eggsIncubating)
            {
                Incubator incubatorToLoad = incubators[egg.incubatorID];
                bool isIncubating = true;

                if (egg.timeToIncubate <= 0)
                    isIncubating = false;

                incubatorToLoad.SetIncubationState(isIncubating, egg.timeToIncubate);
            }
        }
    }
}