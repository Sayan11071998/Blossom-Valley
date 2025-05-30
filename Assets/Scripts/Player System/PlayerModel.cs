using System;
using BlossomValley.GameStrings;

namespace BlossomValley.PlayerSystem
{
    public class PlayerModel
    {
        public event Action MoneyChanged;
        public const string CURRENCY = GameString.currency;

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

        public float WalkSpeed { get; private set; }
        public float RunSpeed { get; private set; }
        public float Gravity { get; private set; }

        public PlayerModel(float walkSpeed, float runSpeed, float gravity)
        {
            Money = 0;
            WalkSpeed = walkSpeed;
            RunSpeed = runSpeed;
            Gravity = gravity;
        }

        public void Spend(int cost)
        {
            if (cost > Money) return;

            Money -= cost;
        }

        public void Earn(int income) => Money += income;

        public void LoadStats(int money) => Money = money;
    }
}