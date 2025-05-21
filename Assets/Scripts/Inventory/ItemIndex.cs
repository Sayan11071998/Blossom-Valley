using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item Index")]
public class ItemIndex : ScriptableObject
{
    public List<ItemData> items;

    public ItemData GetItemFromString(string name)
    {
        return items.Find(i => i.name == name);
    }
}