using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("Internal Clock")]
    [SerializeField] private GameTimestamp timestamp;
    [SerializeField] private float timeScale = 1.0f;

    [Header("Day and Night cycle")]
    [SerializeField] private Transform sunTransform;

    private List<ITimeTracker> listeners = new List<ITimeTracker>();
    private float indoorAngle = 40;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start()
    {
        timestamp = new GameTimestamp(0, Season.Spring, 1, 6, 0);
        StartCoroutine(TimeUpdate());
    }

    public void LoadTime(GameTimestamp timestampValue) => timestamp = new GameTimestamp(timestampValue);

    private IEnumerator TimeUpdate()
    {
        while (true)
        {
            Tick();
            yield return new WaitForSeconds(1 / timeScale);
        }
    }

    public void Tick()
    {
        timestamp.UpdateClock();

        foreach (ITimeTracker listener in listeners)
            listener.ClockUpdate(timestamp);

        UpdateSunMovement();
    }

    public void SkipTime(GameTimestamp timeToSkipTo)
    {
        int timeToSkipInMinutes = GameTimestamp.TimestampInMinutes(timeToSkipTo);
        int timeNowInMinutes = GameTimestamp.TimestampInMinutes(timestamp);
        int differenceInMinutes = timeToSkipInMinutes - timeNowInMinutes;

        if (differenceInMinutes <= 0) return;

        for (int i = 0; i < differenceInMinutes; i++)
            Tick();
    }

    private void UpdateSunMovement()
    {
        if (SceneTransitionManager.Instance.CurrentlyIndoor())
        {
            sunTransform.eulerAngles = new Vector3(indoorAngle, 0, 0);
            return;
        }

        int timeInMinutes = GameTimestamp.HoursToMinutes(timestamp.hour) + timestamp.minute;
        float sunAngle = .25f * timeInMinutes - 90;
        sunTransform.eulerAngles = new Vector3(sunAngle, 0, 0);
    }

    public GameTimestamp GetGameTimestamp() => new GameTimestamp(timestamp);

    public void RegisterTracker(ITimeTracker listener) => listeners.Add(listener);

    public void UnregisterTracker(ITimeTracker listener) => listeners.Remove(listener);
}