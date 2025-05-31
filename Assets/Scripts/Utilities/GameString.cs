namespace BlossomValley.Utilities
{
    public static class GameString
    {
        #region Tags
        public const string Player = "Player";
        public const string Land = "Land";
        public const string Item = "Item";
        #endregion

        #region Player
        public const string currency = " $";

        public const string Horizontal = "Horizontal";
        public const string Vertical = "Vertical";
        public const string Sprint = "Sprint";

        public const string InteractFire_1 = "Fire1";
        public const string InteractFire_2 = "Fire2";
        public const string InteractFire_3 = "Fire3";

        public const string SpeedAnimationFloat = "Speed";
        public const string RunAminationBool = "Running";
        #endregion

        #region UI Prompts
        public const string SellPrompt = "Do you want to sell {0} ? ";
        public const string ShipPrompt = "How many {0} would you like to ship?";
        public const string AnimalNamingPrompt = "Give your new {0} a name.";
        public const string BuyConfirmationPrompt = "Buy {0}?";
        public const string MultiplyString = "x";
        public const string InsufficientFunds = "You do not have enough money to buy this item.";
        public const string CostCalculation = "{0}$ > {1}$";
        #endregion

        #region NPC Dialogue Prompts
        public const string CharacterNotUnlockedPrompt = "You have not unlocked this character yet.";
        public const string AlreadyGiftedMessage = "You have already given {0} a gift today.";
        #endregion

        #region  Animal
        public const string AnimalAnimationWalk = "Walk";
        public const string RelationshipStatusPrefix = "{0} seems ";
        public const string MoodHappy = "really happy today!";
        public const string MoodNeutral = "fine.";
        public const string MoodSad = "sad.";
        #endregion

        #region Calendar
        public const string OrdinaryDay = "Just an ordinary day";
        public const string CalendarHeader = "Year {0} {1}";
        #endregion
    }
}