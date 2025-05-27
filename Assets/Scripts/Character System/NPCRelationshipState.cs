[System.Serializable]
public class NPCRelationshipState
{
    public string name;
    public int friendshipPoints;

    public bool hasTalkedToday;
    public bool giftGivenToday;

    public NPCRelationshipState(string nameValue, int friendshipPointsValue)
    {
        name = nameValue;
        friendshipPoints = friendshipPointsValue;
    }

    public NPCRelationshipState(string nameValue)
    {
        name = nameValue;
        friendshipPoints = 0;
    }

    public float Hearts() => friendshipPoints / 250;
}