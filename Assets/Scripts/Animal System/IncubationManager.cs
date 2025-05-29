using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class IncubationManager : MonoBehaviour
{
    public static List<EggIncubationSaveState> eggsIncubating = new List<EggIncubationSaveState>();
    public const int daysToIncubate = 3;
    public List<Incubator> incubators;
    public static UnityEvent onEggUpdate = new UnityEvent();
    
    [Header("Chicken Spawning")]
    [SerializeField] private float chickenWanderRadius = 3f; // Smaller radius for newly hatched chickens
    [SerializeField] private float spawnRadius = 1f; // How far from incubator to spawn chicken

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

                // Spawn chicken near the specific incubator
                if (manager != null)
                    manager.SpawnChickenNearIncubator(egg.incubatorID);
                else
                {
                    // Fallback to original method
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
            
            // Create the chicken with a callback to set its position and wander behavior
            AnimalData chickenData = AnimalStats.GetAnimalTypeFromString("Chicken");
            
            UIManager.Instance.TriggerNamingPrompt($"Give your new {chickenData.name} a name.", (inputString) =>
            {
                AnimalRelationshipState newChicken = new AnimalRelationshipState(inputString, chickenData);
                AnimalStats.animalRelationships.Add(newChicken);
                
                // Spawn the chicken near the incubator
                SpawnChickenAtLocation(newChicken, incubatorPosition);
            });
        }
    }

    private void SpawnChickenAtLocation(AnimalRelationshipState chickenRelation, Vector3 nearPosition)
    {
        AnimalData chickenData = chickenRelation.AnimalType();
        
        // Find a random position near the incubator
        Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
        randomOffset.y = 0; // Keep on ground level
        Vector3 spawnPosition = nearPosition + randomOffset;
        
        // Ensure the spawn position is on the NavMesh
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(spawnPosition, out hit, spawnRadius * 2, UnityEngine.AI.NavMesh.AllAreas))
        {
            spawnPosition = hit.position;
        }
        
        float randomYRotation = Random.Range(0f, 360f);
        Quaternion randomRotation = Quaternion.Euler(0f, randomYRotation, 0f);
        
        AnimalBehaviour chicken = Instantiate(chickenData.animalObject, spawnPosition, randomRotation);
        chicken.LoadRelationship(chickenRelation);
        
        // Set the chicken to wander in a smaller area around the incubator
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