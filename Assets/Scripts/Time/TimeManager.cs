using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [SerializeField] private GameTimeStamp timeStamp;
    [SerializeField] private float timeScale = 1.0f;

    [Header("Day and Night Cycle")]
    [SerializeField] private Transform sunTransform;

    private List<ITimeTracker> listeners = new List<ITimeTracker>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        timeStamp = new GameTimeStamp(0, GameTimeStamp.Season.Spring, 1, 6, 0);
        StartCoroutine(TimeUpdate());
    }

    IEnumerator TimeUpdate()
    {
        while (true)
        {
            Tick();
            yield return new WaitForSeconds(1 / timeScale);
        }
    }

    public void Tick()
    {
        timeStamp.UpdateClock();

        foreach (ITimeTracker listener in listeners)
        {
            listener.ClockUpdate(timeStamp);
        }

        UpdateSunMovement();
    }

    private void UpdateSunMovement()
    {
        int timeInMinutes = GameTimeStamp.HoursToMinutes(timeStamp.hour) + timeStamp.minute;
        float sunAngle = 0.25f * timeInMinutes - 90;
        sunTransform.eulerAngles = new Vector3(sunAngle, 0, 0);
    }

    public GameTimeStamp GetGameTimeStamp()
    {
        return new GameTimeStamp(timeStamp);
    }

    public void RegisterTracker(ITimeTracker listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterTracker(ITimeTracker listerner)
    {
        listeners.Remove(listerner);
    }
}