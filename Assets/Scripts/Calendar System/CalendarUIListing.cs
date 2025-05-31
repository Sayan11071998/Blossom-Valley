using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BlossomValley.TimeSystem;
using BlossomValley.CharacterSystem;

public class CalendarUIListing : MonoBehaviour
{
    [SerializeField] private List<CalendarEntry> entries;
    [SerializeField] private TextMeshProUGUI calendarHeader;
    [SerializeField] private List<CharacterScriptableObject> allCharacters; // Add this field in inspector

    private GameTimestamp timestamp;

    private void Start() => RenderCalendar(TimeManager.Instance.GetGameTimestamp());

    public void RenderCalendar(GameTimestamp timestamp)
    {
        this.timestamp = timestamp;
        calendarHeader.text = "Year " + timestamp.year + " " + timestamp.season.ToString();

        // Create timestamp for the first day of the season
        GameTimestamp seasonsTime = new GameTimestamp(timestamp.year, timestamp.season, 1, 0, 0);
        
        // Fix day of week calculation - your enum starts with Saturday=0
        DayOfTheWeek firstDayOfSeason = seasonsTime.GetDayOfTheWeek();
        
        // Convert to calendar layout (assuming Monday start)
        // Saturday=0, Sunday=1, Monday=2, etc. from your enum
        // Convert to: Monday=0, Tuesday=1, ..., Saturday=5, Sunday=6
        int dayOfWeek = ((int)firstDayOfSeason + 1) % 7; // This shifts Saturday to 6, Sunday to 0, Monday to 1, etc.
        if (dayOfWeek == 0) dayOfWeek = 6; // Sunday should be last (6)
        else dayOfWeek -= 1; // Shift everything else left by 1

        int entryIndex = 0;
        
        // Fill empty entries for days before the first day of the month
        for (int i = 0; i < dayOfWeek; i++, entryIndex++)
        {
            if (entryIndex < entries.Count)
                entries[entryIndex].EmptyEntry();
        }

        // Fill entries for all days of the season (30 days)
        for (int day = 1; day <= 30; day++, entryIndex++)
        {
            if (entryIndex >= entries.Count) break; // Safety check
            
            // Create timestamp for this specific day
            GameTimestamp currentDayTime = new GameTimestamp(timestamp.year, timestamp.season, day, 0, 0);
            entries[entryIndex].season = timestamp.season;

            // Check for birthday on this day using the fixed method
            CharacterScriptableObject charactersBirthday = GetCharacterWithBirthday(currentDayTime);
            
            if (charactersBirthday != null)
            {
                Debug.Log($"Found birthday for {charactersBirthday.name} on {currentDayTime.season} {day}");
                entries[entryIndex].Display(day, currentDayTime.GetDayOfTheWeek(), 
                    charactersBirthday.portrait, charactersBirthday.name + "'s birthday");
            }
            else
            {
                entries[entryIndex].Display(day, currentDayTime.GetDayOfTheWeek());
            }
        }

        // Clear remaining entries
        for (; entryIndex < entries.Count; entryIndex++)
            entries[entryIndex].EmptyEntry();
    }

    // Fixed method that doesn't rely on scene objects
    private CharacterScriptableObject GetCharacterWithBirthday(GameTimestamp timestamp)
    {
        if (allCharacters == null || allCharacters.Count == 0)
        {
            Debug.LogWarning("No characters assigned to calendar! Please assign CharacterScriptableObjects in the inspector.");
            return null;
        }

        foreach (CharacterScriptableObject character in allCharacters)
        {
            if (character != null && 
                character.birthday.day == timestamp.day && 
                character.birthday.season == timestamp.season)
            {
                return character;
            }
        }

        return null;
    }

    public void NextSeason()
    {
        GameTimestamp nextSeason = new GameTimestamp(timestamp);
        
        // Move to next season
        if (timestamp.season == Season.Winter)
        {
            nextSeason.year++;
            nextSeason.season = Season.Spring;
        }
        else
        {
            nextSeason.season = (Season)((int)timestamp.season + 1);
        }
        
        nextSeason.day = 1; // Start at first day of new season
        nextSeason.hour = 0;
        nextSeason.minute = 0;
        
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
            prevSeason.season = (Season)((int)prevSeason.season - 1);
        }
        
        prevSeason.day = 1; // Start at first day of previous season
        prevSeason.hour = 0;
        prevSeason.minute = 0;
        
        RenderCalendar(prevSeason);
    }
}