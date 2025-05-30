﻿using UnityEngine;
using BlossomValley.InventorySystem;
using BlossomValley.TimeSystem;
using BlossomValley.UISystem;

namespace BlossomValley.IncubationSystem
{
    public class Incubator : InteractableObject
    {
        [SerializeField] public int incubationID;
        [SerializeField] private GameObject displayEgg;

        private bool containsEgg;
        private int timeToIncubate;

        public override void Pickup()
        {
            if (CanIncubate())
                StartIncubation();
        }

        private bool CanIncubate()
        {
            ItemData handSlotItem = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Item);

            if (handSlotItem == null || containsEgg) return false;
            if (handSlotItem.name != item.name) return false;

            return true;
        }

        private void StartIncubation()
        {
            InventoryManager.Instance.ConsumeItem(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item));
            int hours = GameTimestamp.DaysToHours(IncubationManager.daysToIncubate);
            SetIncubationState(true, GameTimestamp.HoursToMinutes(hours));
            IncubationManager.eggsIncubating.Add(new EggIncubationSaveState(incubationID, timeToIncubate));
        }

        public void SetIncubationState(bool containsEgg, int timeToIncubate)
        {
            this.containsEgg = containsEgg;
            this.timeToIncubate = timeToIncubate;
            displayEgg.SetActive(containsEgg);
        }
    }
}