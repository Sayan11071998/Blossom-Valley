using System.Collections;
using System.Collections.Generic;
using BlossomValley.InventorySystem;
using BlossomValley.TimeSystem;
using BlossomValley.UISystem;
using UnityEngine;

namespace BlossomValley.CalendarSystem
{
    public class CalendarInteractable : InteractableObject
    {
        public override void Pickup()
        {
            CalendarUIListing calendar = UIManager.Instance.calendar;
            Debug.Log("Calendar Interactable Triggered");
            calendar.gameObject.SetActive(true);
            calendar.RenderCalendar(TimeManager.Instance.GetGameTimestamp());
        }
    }
}