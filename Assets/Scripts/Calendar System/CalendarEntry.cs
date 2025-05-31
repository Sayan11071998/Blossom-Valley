using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BlossomValley.TimeSystem;
using BlossomValley.Utilities;

namespace BlossomValley.CalendarSystem
{
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

        void Awake()
        {
            entry = GetComponent<Image>();
        }

        public void Display(int date, DayOfTheWeek day, Sprite eventSprite, string eventDescription)
        {
            if (dateText != null)
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

            if (entry != null)
                entry.color = colorToSet;

            if (icon != null)
            {
                if (eventSprite != null)
                {
                    icon.gameObject.SetActive(true);
                    icon.sprite = eventSprite;
                }
                else
                {
                    icon.gameObject.SetActive(false);
                }
            }
        }

        public void Display(int date, DayOfTheWeek day) => Display(date, day, null, GameString.OrdinaryDay);

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
}