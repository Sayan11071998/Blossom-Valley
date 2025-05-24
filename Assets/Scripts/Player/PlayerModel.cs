public class PlayerModel
{
    public event System.Action MoneyChanged;
    public const string CURRENCY = " $";

    private int money;
    public int Money
    {
        get => money;
        private set
        {
            money = value;
            MoneyChanged?.Invoke();
        }
    }

    public PlayerModel() => money = 0;

    public void Spend(int cost)
    {
        if (cost > Money)
        {
            UnityEngine.Debug.LogError("Player does not have enough money");
            return;
        }
        Money -= cost;
    }

    public void Earn(int income) => Money += income;

    public void LoadStats(int money) => Money = money;
}