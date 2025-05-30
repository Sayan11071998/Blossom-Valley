using BlossomValley.PlayerSystem;
using UnityEngine;

[System.Serializable]
public class PlayerSaveState
{
    public int money;

    public PlayerSaveState(int moneyValue) => money = moneyValue;

    public void LoadData(PlayerModel playerModelToLoad) => playerModelToLoad.LoadStats(money);

    public static PlayerSaveState Export()
    {
        PlayerModel playerModel = Object.FindAnyObjectByType<PlayerView>().PlayerModel;
        return new PlayerSaveState(playerModel.Money);
    }
}