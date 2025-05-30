namespace BlossomValley.SaveSystem
{
    [System.Serializable]
    public class GameSaveState
    {
        public FarmSaveState farmSaveState;
        public InventorySaveState inventorySaveState;
        public GameTimestamp timestamp;
        public PlayerSaveState playerSaveState;
        public RelationshipSaveState relationshipSaveState;

        public GameSaveState(FarmSaveState farmSaveStateValue, InventorySaveState inventorySaveStateValue, GameTimestamp timestampValue, PlayerSaveState playerSaveStateValue, RelationshipSaveState relationshipSaveStateValue)
        {
            farmSaveState = farmSaveStateValue;
            inventorySaveState = inventorySaveStateValue;
            timestamp = timestampValue;
            playerSaveState = playerSaveStateValue;
            relationshipSaveState = relationshipSaveStateValue;
        }
    }
}