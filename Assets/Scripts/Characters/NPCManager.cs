using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCManager : MonoBehaviour, ITimeTracker
{
    public static NPCManager Instance { get; private set; }

    private void Awake()
    {
        //If there is more than one instance, destroy the extra
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            //Set the static instance to this instance
            Instance = this;
        }
    }

    

    List<CharacterData> characters = null; 

    List<NPCScheduleData> npcSchedules;

    [SerializeField]
    List<NPCLocationState> npcLocations; 


    //Load all character data
    public List<CharacterData> Characters()
    {
        if (characters != null) return characters; 
        CharacterData[] characterDatabase = Resources.LoadAll<CharacterData>("Characters");
        characters = characterDatabase.ToList();
        return characters;
    }

    private void OnEnable()
    {
        //Load NPC Schedules
        NPCScheduleData[] schedules = Resources.LoadAll<NPCScheduleData>("Schedules");
        npcSchedules = schedules.ToList();
        InitNPCLocations();
        
    }

    private void Start()
    {
        //Add this to TimeManager's Listener list
        TimeManager.Instance.RegisterTracker(this);
        SceneTransitionManager.Instance.onLocationLoad.AddListener(RenderNPCs); 
    }

    private void InitNPCLocations()
    {
        npcLocations = new List<NPCLocationState>(); 
        foreach(CharacterData character in Characters())
        {
            npcLocations.Add(new NPCLocationState(character)); 
        }
        
    }

    void RenderNPCs()
    {
        foreach(NPCLocationState npc in npcLocations)
        {
            if(npc.location == SceneTransitionManager.Instance.currentLocation)
            {
                Instantiate(npc.character.prefab, npc.coord, Quaternion.Euler(npc.facing)); 
            }
        }
    }

    public void ClockUpdate(GameTimestamp timestamp) {
        UpdateNPCLocations(timestamp); 
    }

    private void UpdateNPCLocations(GameTimestamp timestamp)
    {
        for (int i = 0; i < npcLocations.Count; i++)
        {
            NPCLocationState npcLocator = npcLocations[i]; 
            //Find the schedule belonging to the NPC
            NPCScheduleData schedule = npcSchedules.Find(x => x.character == npcLocator.character);
            if(schedule == null)
            {
                Debug.LogError("No schedule found for " + npcLocator.character.name);
                continue; 
            }

            //Current time 
            
            GameTimestamp.DayOfTheWeek dayOfWeek = timestamp.GetDayOfTheWeek();

            //Find the events that correspond to the current time
            //E.g. if the event is set to 8am, the current time must be after 8am, so the hour of timeNow has to be greater than the event
            //Either the day of the week matches or it is set to ignore the day of the week

            //In future we will also have the Seasons factored in
            List<ScheduleEvent> eventsToConsider = schedule.npcScheduleList.FindAll(x => x.time.hour <= timestamp.hour && (x.dayOfTheWeek == dayOfWeek || x.ignoreDayOfTheWeek));
            //Check if the events are empty
            if(eventsToConsider.Count < 1)
            {
                Debug.LogError("None found for " + npcLocator.character.name);
                Debug.LogError(timestamp.hour); 
                continue;
            }
            
            //Remove all the events with the hour that is lower than the max time (The time has already elapsed)
            int maxHour = eventsToConsider.Max(x => x.time.hour);
            eventsToConsider.RemoveAll(x => x.time.hour < maxHour);

            //Get the event with the highest priority
            ScheduleEvent eventToExecute = eventsToConsider.OrderByDescending(x => x.priority).First();
            //Set the NPC Locator value accordingly
            npcLocations[i] = new NPCLocationState(schedule.character, eventToExecute.location, eventToExecute.coord, eventToExecute.facing);
        }

    }







}
