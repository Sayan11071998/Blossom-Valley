using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BlossomValley.TimeSystem;

[RequireComponent(typeof(Image))]
public class CalendarEntry : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private Image icon;
    [SerializeField] private Color weekday;
    [SerializeField] private Color sat;
    [SerializeField] private Color sun;
    [SerializeField] private Color today;

    public Season season;

    private Image entry;
    private string eventDescription;

    void Awake()
    {
        entry = GetComponent<Image>();
    }

    public void Display(int date, DayOfTheWeek day, Sprite eventSprite, string eventDescription)
    {
        // Always set the date first
        if (dateText != null)
            dateText.text = date.ToString();
        
        // Set the background color
        Color colorToSet = weekday;
        
        switch (day)
        {
            case DayOfTheWeek.Saturday:
                colorToSet = sat;
                break;
            case DayOfTheWeek.Sunday:
                colorToSet = sun;
                break;
            default:
                colorToSet = weekday;
                break;
        }

        // Check if this is today and override color
        GameTimestamp today = TimeManager.Instance.GetGameTimestamp();
        if (date == today.day && today.season == season)
            colorToSet = this.today;

        if (entry != null)
            entry.color = colorToSet;

        // Handle icon display - this is the critical fix
        if (icon != null)
        {
            if (eventSprite != null)
            {
                icon.gameObject.SetActive(true);
                icon.sprite = eventSprite;
                Debug.Log($"Activated icon for date {date} with sprite {eventSprite.name}");
            }
            else
            {
                icon.gameObject.SetActive(false);
                Debug.Log($"Deactivated icon for date {date} - no sprite provided");
            }
        }
        else
        {
            Debug.LogWarning($"Icon component is null on CalendarEntry for date {date}!");
        }

        this.eventDescription = eventDescription;
    }

    public void Display(int date, DayOfTheWeek day) 
    {
        Display(date, day, null, "Just an ordinary day");
    }
    
    public void EmptyEntry()
    {
        if (entry != null)
            entry.color = Color.clear;
        if (dateText != null)
            dateText.text = "";
        if (icon != null)
            icon.gameObject.SetActive(false);
    }
}