using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BlossomValley.TimeSystem;
using BlossomValley.CharacterSystem;

public class CalendarUIListing : MonoBehaviour
{
    [SerializeField] private List<CalendarEntry> entries;
    [SerializeField] private TextMeshProUGUI calendarHeader;

    private GameTimestamp timestamp;

    private void Start() => RenderCalendar(TimeManager.Instance.GetGameTimestamp());

    public void RenderCalendar(GameTimestamp timestamp)
    {
        this.timestamp = timestamp;
        calendarHeader.text = "Year " + timestamp.year + " " + timestamp.season.ToString();

        GameTimestamp seasonsTime = new GameTimestamp(timestamp.year, timestamp.season, 1, 0, 0);
        int dayOfWeek = (int)seasonsTime.GetDayOfTheWeek();
        dayOfWeek = (dayOfWeek + 6) % 7;

        int entryIndex = 0;
        if (dayOfWeek != 0)
        {
            for (entryIndex = 0; entryIndex < dayOfWeek; entryIndex++)
                entries[entryIndex].EmptyEntry();
        }

        int lastDay = entryIndex + 30;
        for (; entryIndex < lastDay; entryIndex++)
        {
            entries[entryIndex].season = timestamp.season;

            CharacterScriptableObject charactersBirthday = RelationshipStats.WhoseBirthday(seasonsTime);
            if (charactersBirthday != null)
                entries[entryIndex].Display(seasonsTime.day, seasonsTime.GetDayOfTheWeek(), charactersBirthday.portrait, charactersBirthday.name + "'s birthday");
            else
                entries[entryIndex].Display(seasonsTime.day, seasonsTime.GetDayOfTheWeek());

            seasonsTime.hour = 23;
            seasonsTime.minute = 59;
            seasonsTime.UpdateClock();
        }

        for (; entryIndex < entries.Count; entryIndex++)
            entries[entryIndex].EmptyEntry();
    }

    public void NextSeason()
    {
        GameTimestamp nextSeason = new GameTimestamp(timestamp);
        nextSeason.day = 30;
        nextSeason.hour = 23;
        nextSeason.minute = 59;
        nextSeason.UpdateClock();
        RenderCalendar(nextSeason);
    }

    public void PrevSeason()
    {
        GameTimestamp prevSeason = new GameTimestamp(timestamp);
        if (prevSeason.season == Season.Spring)
        {
            prevSeason.year--;
            prevSeason.season = Season.Winter;
        }
        else
        {
            prevSeason.season--;
        }
        RenderCalendar(prevSeason);
    }
}