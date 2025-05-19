using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [SerializeField] private GameTimeStamp timeStamp;
    [SerializeField] private float timeScale = 1.0f;

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
            yield return new WaitForSeconds(1 / timeScale);
            Tick();
        }
    }

    public void Tick()
    {
        timeStamp.UpdateClock();
    }
}