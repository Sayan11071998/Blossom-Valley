using UnityEngine;

[CreateAssetMenu(fileName = "ItemScriptableObject", menuName = "Items/Item")]
public class ItemData : ScriptableObject
{
    public string description;
    public Sprite thumbnail;
    public GameObject gameModel;
    public int cost;
}