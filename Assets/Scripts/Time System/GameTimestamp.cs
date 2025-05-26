using UnityEngine;

[System.Serializable]
public class GameTimestamp
{
    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    public enum DayOfTheWeek
    {
        Saturday,
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday
    }

    [SerializeField] public Season season;
    [SerializeField] public int year;
    [SerializeField] public int day;
    [SerializeField] public int hour;
    [SerializeField] public int minute;

    // public int Year => year;
    // public int Day => day;
    // public int Hour => hour;
    // public int Minute => minute;

    public GameTimestamp(int yearToSet, Season seasonToSet, int dayToSet, int hourToSet, int minuteToSet)
    {
        year = yearToSet;
        season = seasonToSet;
        day = dayToSet;
        hour = hourToSet;
        minute = minuteToSet;
    }

    public GameTimestamp(GameTimestamp timestamp)
    {
        year = timestamp.year;
        season = timestamp.season;
        day = timestamp.day;
        hour = timestamp.hour;
        minute = timestamp.minute;
    }

    public void UpdateClock()
    {
        minute++;

        if (minute >= 60)
        {
            minute = 0;
            hour++;
        }

        if (hour >= 24)
        {
            hour = 0;
            day++;
        }

        if (day > 30)
        {
            day = 1;

            if (season == Season.Winter)
            {
                season = Season.Spring;
                year++;
            }
            else
            {
                season++;
            }
        }
    }

    public DayOfTheWeek GetDayOfTheWeek()
    {
        int daysPassed = YearsToDays(year) + SeasonsToDays(season) + day;
        int dayIndex = daysPassed % 7;
        return (DayOfTheWeek)dayIndex;
    }

    public static int HoursToMinutes(int hour) => hour * 60;

    public static int DaysToHours(int days) => days * 24;

    public static int SeasonsToDays(Season season)
    {
        int seasonIndex = (int)season;
        return seasonIndex * 30;
    }

    public static int YearsToDays(int years) => years * 4 * 30;

    public static int TimestampInMinutes(GameTimestamp timestamp) => HoursToMinutes(DaysToHours(YearsToDays(timestamp.year)) + DaysToHours(SeasonsToDays(timestamp.season)) + DaysToHours(timestamp.day) + timestamp.hour) + timestamp.minute;

    public static int CompareTimestamps(GameTimestamp timestamp1, GameTimestamp timestamp2)
    {
        int timestamp1Hours = DaysToHours(YearsToDays(timestamp1.year)) + DaysToHours(SeasonsToDays(timestamp1.season)) + DaysToHours(timestamp1.day) + timestamp1.hour;
        int timestamp2Hours = DaysToHours(YearsToDays(timestamp2.year)) + DaysToHours(SeasonsToDays(timestamp2.season)) + DaysToHours(timestamp2.day) + timestamp2.hour;
        int difference = timestamp2Hours - timestamp1Hours;
        return Mathf.Abs(difference);
    }
}