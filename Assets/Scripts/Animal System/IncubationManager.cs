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

        foreach (EggIncubationSaveState egg in eggsIncubating.ToList())
        {
            egg.Tick();
            onEggUpdate?.Invoke();
            if (egg.timeToIncubate <= 0)
            {
                eggsIncubating.Remove(egg);

                AnimalData chickenData = AnimalStats.GetAnimalTypeFromString("Chicken");
                AnimalStats.StartAnimalCreation(chickenData);
            }
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