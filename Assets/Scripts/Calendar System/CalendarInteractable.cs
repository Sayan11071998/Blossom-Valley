using BlossomValley.InventorySystem;
using BlossomValley.TimeSystem;
using BlossomValley.UISystem;

namespace BlossomValley.CalendarSystem
{
    public class CalendarInteractable : InteractableObject
    {
        public override void Pickup()
        {
            CalendarUIListing calendar = UIManager.Instance.calendar;
            calendar.gameObject.SetActive(true);
            calendar.RenderCalendar(TimeManager.Instance.GetGameTimestamp());
        }
    }
}