using System.Collections;
using UnityEngine;
using BlossomValley.SoundSystem;
using BlossomValley.TimeSystem;

namespace BlossomValley.WeatherSystem
{
    public class WeatherManager : MonoBehaviour, ITimeTracker
    {
        public static WeatherManager Instance { get; private set; }

        [Header("Rain Settings")]
        [SerializeField] private ParticleSystem rainParticleSystem;
        [SerializeField] private bool isRaining = false;

        [Header("Rain Schedule")]
        [SerializeField] private int rainStartHour = 14;
        [SerializeField] private int rainStartMinute = 0;
        [SerializeField] private int rainEndHour = 16;
        [SerializeField] private int rainEndMinute = 0;

        [Header("Seasonal Rain Probability")]
        [SerializeField] private float springRainChance = 1f;
        [SerializeField] private float summerRainChance = 0.4f;
        [SerializeField] private float fallRainChance = 0.8f;
        [SerializeField] private float winterRainChance = 0.3f;

        [Header("Random Rain Events")]
        [SerializeField] private bool enableRandomRain = true;
        [SerializeField] private float randomRainCheckInterval = 60f;

        [Header("Rain Audio")]
        [SerializeField] private AudioSource rainAudioSource;

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
            if (TimeManager.Instance != null)
                TimeManager.Instance.RegisterTracker(this);

            if (rainParticleSystem != null)
                rainParticleSystem.Stop();

            if (rainAudioSource == null)
            {
                rainAudioSource = gameObject.AddComponent<AudioSource>();
                rainAudioSource.loop = true;
                rainAudioSource.playOnAwake = false;
            }

            lastRainCheck = new GameTimestamp(0, Season.Spring, 1, 0, 0);
        }

        private void OnDestroy()
        {
            if (TimeManager.Instance != null)
                TimeManager.Instance.UnregisterTracker(this);
        }

        public void ClockUpdate(GameTimestamp timestamp)
        {
            CheckRainSchedule(timestamp);

            if (enableRandomRain)
                CheckRandomRain(timestamp);
        }

        private void CheckRainSchedule(GameTimestamp timestamp)
        {
            bool isCurrentlyRainTime = IsRainTime(timestamp);

            if (isCurrentlyRainTime && !wasRainTime)
            {
                float rainChance = GetSeasonalRainChance(timestamp.season);
                if (Random.Range(0f, 1f) <= rainChance)
                    StartRain();
            }
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

                if (!IsRainTime(timestamp) && !isRaining)
                {
                    float rainChance = GetSeasonalRainChance(timestamp.season) * 0.1f;
                    if (Random.Range(0f, 1f) <= rainChance)
                    {
                        StartRain();
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
                StartRainSound();
            }
        }

        public void StopRain()
        {
            if (isRaining && rainParticleSystem != null)
            {
                isRaining = false;
                rainParticleSystem.Stop();
                StopRainSound();
            }
        }

        private void StartRainSound()
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayMusic(SoundType.Raining);
        }

        private void StopRainSound()
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.StopMusic();
                SoundManager.Instance.PlayMusic(SoundType.BackgroundMusic);
            }
        }

        private IEnumerator StopRainAfterDuration(int durationInMinutes)
        {
            float timeScale = 1.0f;
            float waitTime = durationInMinutes / timeScale;
            yield return new WaitForSeconds(waitTime);

            if (isRaining)
            {
                GameTimestamp currentTime = TimeManager.Instance.GetGameTimestamp();
                if (!IsRainTime(currentTime))
                    StopRain();
            }
        }

        public bool IsCurrentlyRaining() => isRaining;

        public void SetRainSchedule(int startHour, int startMinute, int endHour, int endMinute)
        {
            rainStartHour = startHour;
            rainStartMinute = startMinute;
            rainEndHour = endHour;
            rainEndMinute = endMinute;
        }

        public void SetSeasonalRainChances(float spring, float summer, float fall, float winter)
        {
            springRainChance = Mathf.Clamp01(spring);
            summerRainChance = Mathf.Clamp01(summer);
            fallRainChance = Mathf.Clamp01(fall);
            winterRainChance = Mathf.Clamp01(winter);
        }
    }
}