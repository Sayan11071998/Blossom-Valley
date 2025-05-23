using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveState
{
    //In future we will collect information like achievements, shipping history, building assets/upgrades and the like here. 
    public int money; 

    public PlayerSaveState(int money)
    {
        this.money = money;
    }

    public void LoadData()
    {
        PlayerStats.LoadStats(money);
    }

    public static PlayerSaveState Export()
    {
        return new PlayerSaveState(PlayerStats.Money); 
    }
}
