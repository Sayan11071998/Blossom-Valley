using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour, ITimeTracker
{
    public static WeatherManager Instance { get; private set; }

    [Header("Rain Settings")]
    [SerializeField] private ParticleSystem rainParticleSystem;
    [SerializeField] private bool isRaining = false;
    
    [Header("Rain Schedule")]
    [SerializeField] private int rainStartHour = 14; // 2 PM
    [SerializeField] private int rainStartMinute = 0;
    [SerializeField] private int rainEndHour = 16; // 4 PM
    [SerializeField] private int rainEndMinute = 0;
    
    [Header("Seasonal Rain Probability")]
    [SerializeField] private float springRainChance = 1f;
    [SerializeField] private float summerRainChance = 0.4f;
    [SerializeField] private float fallRainChance = 0.8f;
    [SerializeField] private float winterRainChance = 0.3f;
    
    [Header("Random Rain Events")]
    [SerializeField] private bool enableRandomRain = true;
    [SerializeField] private float randomRainCheckInterval = 60f; // Check every 60 minutes
    
    private GameTimestamp lastRainCheck;
    private bool wasRainTime = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start()
    {
        // Register with TimeManager to receive time updates
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.RegisterTracker(this);
        }
        
        // Initialize rain particle system
        if (rainParticleSystem != null)
        {
            rainParticleSystem.Stop();
        }
        else
        {
            Debug.LogWarning("Rain Particle System not assigned to WeatherManager!");
        }
        
        lastRainCheck = new GameTimestamp(0, Season.Spring, 1, 0, 0);
    }

    private void OnDestroy()
    {
        // Unregister from TimeManager
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.UnregisterTracker(this);
        }
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        CheckRainSchedule(timestamp);
        
        if (enableRandomRain)
        {
            CheckRandomRain(timestamp);
        }
    }

    private void CheckRainSchedule(GameTimestamp timestamp)
    {
        bool isCurrentlyRainTime = IsRainTime(timestamp);
        
        // Start rain if it's rain time and wasn't raining before
        if (isCurrentlyRainTime && !wasRainTime)
        {
            float rainChance = GetSeasonalRainChance(timestamp.season);
            if (Random.Range(0f, 1f) <= rainChance)
            {
                StartRain();
            }
        }
        // Stop rain if it's no longer rain time and was raining before
        else if (!isCurrentlyRainTime && wasRainTime && isRaining)
        {
            StopRain();
        }
        
        wasRainTime = isCurrentlyRainTime;
    }

    private void CheckRandomRain(GameTimestamp timestamp)
    {
        int timeDifferenceInMinutes = GameTimestamp.CompareTimestamps(lastRainCheck, timestamp);
        
        if (timeDifferenceInMinutes >= randomRainCheckInterval)
        {
            lastRainCheck = new GameTimestamp(timestamp);
            
            // Don't trigger random rain during scheduled rain time
            if (!IsRainTime(timestamp) && !isRaining)
            {
                float rainChance = GetSeasonalRainChance(timestamp.season) * 0.1f; // Lower chance for random rain
                if (Random.Range(0f, 1f) <= rainChance)
                {
                    StartRain();
                    // Random rain lasts for a random duration between 30-90 minutes
                    int rainDuration = Random.Range(30, 91);
                    StartCoroutine(StopRainAfterDuration(rainDuration));
                }
            }
        }
    }

    private bool IsRainTime(GameTimestamp timestamp)
    {
        int currentTimeInMinutes = timestamp.hour * 60 + timestamp.minute;
        int rainStartInMinutes = rainStartHour * 60 + rainStartMinute;
        int rainEndInMinutes = rainEndHour * 60 + rainEndMinute;
        
        return currentTimeInMinutes >= rainStartInMinutes && currentTimeInMinutes < rainEndInMinutes;
    }

    private float GetSeasonalRainChance(Season season)
    {
        switch (season)
        {
            case Season.Spring:
                return springRainChance;
            case Season.Summer:
                return summerRainChance;
            case Season.Fall:
                return fallRainChance;
            case Season.Winter:
                return winterRainChance;
            default:
                return 0.5f;
        }
    }

    public void StartRain()
    {
        if (!isRaining && rainParticleSystem != null)
        {
            isRaining = true;
            rainParticleSystem.Play();
            Debug.Log("Rain started!");
            
            // Notify other systems about rain starting
            OnRainStarted();
        }
    }

    public void StopRain()
    {
        if (isRaining && rainParticleSystem != null)
        {
            isRaining = false;
            rainParticleSystem.Stop();
            Debug.Log("Rain stopped!");
            
            // Notify other systems about rain stopping
            OnRainStopped();
        }
    }

    private IEnumerator StopRainAfterDuration(int durationInMinutes)
    {
        // Wait for the specified duration (converted to real-time seconds based on time scale)
        float timeScale = 1.0f; // You might want to get this from TimeManager if it's accessible
        float waitTime = durationInMinutes / timeScale;
        yield return new WaitForSeconds(waitTime);
        
        // Only stop if it's still raining and not during scheduled rain time
        if (isRaining)
        {
            GameTimestamp currentTime = TimeManager.Instance.GetGameTimestamp();
            if (!IsRainTime(currentTime))
            {
                StopRain();
            }
        }
    }

    // Public methods for manual control
    public void ForceStartRain() => StartRain();
    public void ForceStopRain() => StopRain();
    public bool IsCurrentlyRaining() => isRaining;

    // Events for other systems to listen to
    private void OnRainStarted()
    {
        // You can add event system here if needed
        // Example: RainStartedEvent?.Invoke();
    }

    private void OnRainStopped()
    {
        // You can add event system here if needed
        // Example: RainStoppedEvent?.Invoke();
    }

    // Method to set custom rain schedule
    public void SetRainSchedule(int startHour, int startMinute, int endHour, int endMinute)
    {
        rainStartHour = startHour;
        rainStartMinute = startMinute;
        rainEndHour = endHour;
        rainEndMinute = endMinute;
    }

    // Method to adjust seasonal rain chances
    public void SetSeasonalRainChances(float spring, float summer, float fall, float winter)
    {
        springRainChance = Mathf.Clamp01(spring);
        summerRainChance = Mathf.Clamp01(summer);
        fallRainChance = Mathf.Clamp01(fall);
        winterRainChance = Mathf.Clamp01(winter);
    }
}