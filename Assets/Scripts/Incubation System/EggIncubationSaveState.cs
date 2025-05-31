using UnityEngine;

namespace BlossomValley.IncubationSystem
{
    [System.Serializable]
    public class EggIncubationSaveState
    {
        [SerializeField] public int incubatorID;
        [SerializeField] public int timeToIncubate;

        public EggIncubationSaveState(int incubatorIDValue, int timeToIncubateValue)
        {
            incubatorID = incubatorIDValue;
            timeToIncubate = timeToIncubateValue;
        }

        public void Tick() => timeToIncubate--;
    }
}