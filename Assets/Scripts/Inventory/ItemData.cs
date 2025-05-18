using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
public class ItemData : ScriptableObject
{
    public string description;
    public Sprite thumbnail;
    public GameObject gameModel;
}