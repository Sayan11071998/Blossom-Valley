using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSaveState
{
    public int money;

    public PlayerSaveState(int money)
    {
        this.money = money;
    }

    public void LoadData(PlayerModel playerModel)
    {
        playerModel.LoadStats(money);
    }

    public static PlayerSaveState Export()
    {
        PlayerModel playerModel = Object.FindAnyObjectByType<PlayerView>().GetPlayerModel();
        return new PlayerSaveState(playerModel.Money);
    }
}