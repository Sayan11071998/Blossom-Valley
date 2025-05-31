using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BlossomValley.TimeSystem;
using BlossomValley.CharacterSystem;
using BlossomValley.Utilities;

namespace BlossomValley.CalendarSystem
{
    public class CalendarUIListing : MonoBehaviour
    {
        [SerializeField] private List<CalendarEntry> entries;
        [SerializeField] private TextMeshProUGUI calendarHeader;
        [SerializeField] private List<CharacterScriptableObject> allCharacters;

        private GameTimestamp timestamp;

        private void Start() => RenderCalendar(TimeManager.Instance.GetGameTimestamp());

        public void RenderCalendar(GameTimestamp timestamp)
        {
            this.timestamp = timestamp;
            calendarHeader.text = string.Format(GameString.CalendarHeader, timestamp.year, timestamp.season.ToString());


            GameTimestamp seasonsTime = new GameTimestamp(timestamp.year, timestamp.season, 1, 0, 0);
            DayOfTheWeek firstDayOfSeason = seasonsTime.GetDayOfTheWeek();

            int dayOfWeek = (int)firstDayOfSeason;
            int entryIndex = 0;

            for (int i = 0; i < dayOfWeek; i++, entryIndex++)
            {
                if (entryIndex < entries.Count)
                    entries[entryIndex].EmptyEntry();
            }

            for (int day = 1; day <= 30; day++, entryIndex++)
            {
                if (entryIndex >= entries.Count) break;

                GameTimestamp currentDayTime = new GameTimestamp(timestamp.year, timestamp.season, day, 0, 0);
                entries[entryIndex].season = timestamp.season;

                CharacterScriptableObject charactersBirthday = GetCharacterWithBirthday(currentDayTime);

                if (charactersBirthday != null)
                    entries[entryIndex].Display(day, currentDayTime.GetDayOfTheWeek(), charactersBirthday.portrait, charactersBirthday.name + "'s birthday");
                else
                    entries[entryIndex].Display(day, currentDayTime.GetDayOfTheWeek());
            }

            for (; entryIndex < entries.Count; entryIndex++)
                entries[entryIndex].EmptyEntry();
        }

        private CharacterScriptableObject GetCharacterWithBirthday(GameTimestamp timestamp)
        {
            if (allCharacters == null || allCharacters.Count == 0) return null;

            foreach (CharacterScriptableObject character in allCharacters)
            {
                if (character != null && character.birthday.day == timestamp.day && character.birthday.season == timestamp.season)
                    return character;
            }

            return null;
        }

        public void NextSeason()
        {
            GameTimestamp nextSeason = new GameTimestamp(timestamp);

            if (timestamp.season == Season.Winter)
            {
                nextSeason.year++;
                nextSeason.season = Season.Spring;
            }
            else
            {
                nextSeason.season = (Season)((int)timestamp.season + 1);
            }

            nextSeason.day = 1;
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

            prevSeason.day = 1;
            prevSeason.hour = 0;
            prevSeason.minute = 0;

            RenderCalendar(prevSeason);
        }
    }
}