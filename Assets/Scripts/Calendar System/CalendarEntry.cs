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

    void OnEnable()
    {
        icon.gameObject.SetActive(false);
        entry = GetComponent<Image>();
    }

    public void Display(int date, DayOfTheWeek day, Sprite eventSprite, string eventDescription)
    {
        dateText.text = date.ToString();
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

        GameTimestamp today = TimeManager.Instance.GetGameTimestamp();
        if (date == today.day && today.season == season)
            colorToSet = this.today;

        entry.color = colorToSet;

        if (eventSprite != null)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = eventSprite;
        }
        else
        {
            icon.gameObject.SetActive(false);
        }

        this.eventDescription = eventDescription;
    }

    public void Display(int date, DayOfTheWeek day) => Display(date, day, null, "Just an ordinary day");
    public void EmptyEntry()
    {
        entry.color = Color.clear;
        dateText.text = "";
        icon.gameObject.SetActive(false);
    }
}