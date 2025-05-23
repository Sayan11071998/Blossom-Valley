using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSaveState
{
    //Farm Data
    public FarmSaveState farmSaveState;
    //Inventory
    public InventorySaveState inventorySaveState;
    //Time
    public GameTimestamp timestamp;

    //PlayerStats
    public PlayerSaveState playerSaveState;

    //Relationships
    public RelationshipSaveState relationshipSaveState;

    public GameSaveState(
        FarmSaveState farmSaveState,
        InventorySaveState inventorySaveState,
        GameTimestamp timestamp,
        PlayerSaveState playerSaveState,
        RelationshipSaveState relationshipSaveState
        )
        
    {
        this.farmSaveState = farmSaveState;
        this.inventorySaveState = inventorySaveState;
        this.timestamp = timestamp;
        this.playerSaveState = playerSaveState;
        this.relationshipSaveState = relationshipSaveState;
    }
}
