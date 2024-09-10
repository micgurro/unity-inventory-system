using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace meekobytes
{
    /// <summary>
    /// This is a scriptable object that defines what an item is in our game.
    /// It could be inherited from; to have branched versions of items, for examples potions and equipment.
    /// </summary>

    [CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
    public class InventoryItemData : ScriptableObject
    {
        public int ID = -1;
        public string DisplayName;
        [TextArea(4, 4)]
        public string Description;
        public Sprite Icon;
        public int MaxStackSize;
        public GameObject ItemPrefab;
        public bool IsAutoPickup;
        public void UseItem()
        {
            Debug.Log($"Using {DisplayName}");
        }

    }
}