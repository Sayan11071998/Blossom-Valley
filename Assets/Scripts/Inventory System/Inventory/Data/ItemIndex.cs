using System.Collections.Generic;
using UnityEngine;

namespace BlossomValley.InventorySystem
{
    [CreateAssetMenu(fileName = "ItemIndexScriptableObject", menuName = "Items/Item Index")]
    public class ItemIndex : ScriptableObject
    {
        public List<ItemData> items;

        public ItemData GetItemFromString(string name) => items.Find(i => i.name == name);
    }
}