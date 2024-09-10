using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace meekobytes
{
    [System.Serializable]
    public class InventorySlot : ISerializationCallbackReceiver
    {
        [NonSerialized] private InventoryItemData itemData; // Reference to the data.
        [SerializeField] private int _itemID = -1;
        [SerializeField] private int stackSize; // Current stack size - how many of the data do we have?

        public InventoryItemData ItemData => itemData;
        public int StackSize => stackSize;

        public InventorySlot(InventoryItemData source, int amount) //Constructor to make an occupied inventory slot
        {
            itemData = source;
            _itemID = itemData.ID;
            stackSize = amount;
        }

        public InventorySlot() // Constructor to make an empty inventory slot
        {
            ClearSlot();
        }

        public void ClearSlot() // Clears the slot
        {
            itemData = null;
            _itemID = -1;
            stackSize = -1;
        }

        public void AssignItem(InventorySlot invSlot) // Assigns an item to the slot
        {
            if (itemData == invSlot.ItemData) AddToStack(invSlot.stackSize); // Does the slot contain the same item? Add to stack if so.
            else // Overwrite slot with the inventory slot we're passing in.
            {
                itemData = invSlot.itemData;
                _itemID = itemData.ID;
                stackSize = 0;
                AddToStack(invSlot.stackSize);
            }
        }

        public void UpdateInventorySlot(InventoryItemData data, int amount) //Assigns the data and amount directly.
        {
            itemData = data;
            _itemID = itemData.ID;
            stackSize = amount;
        }

        public bool EnoughRoomLeftInStack(int amountToAdd, out int amountRemaining) // Would there be enough room in the stack for the amount we're trying to add?
        {
            amountRemaining = itemData.MaxStackSize - stackSize;
            return EnoughRoomLeftInStack(amountToAdd);
        }

        public bool EnoughRoomLeftInStack(int amountToAdd)
        {

            if (itemData == null || itemData != null && stackSize + amountToAdd <= itemData.MaxStackSize) return true;
            else return false;
        }

        public void AddToStack(int amount)
        {
            stackSize += amount;
        }

        public void RemoveFromStack(int amount)
        {
            stackSize -= amount;
        }

        public bool SplitStack(out InventorySlot splitStack) //Is there enough to split? If not return false.
        {
            if (stackSize <= 1)
            {
                splitStack = null;
                return false;
            }

            int halfStack = Mathf.RoundToInt(stackSize / 2); // Get half the stack.
            RemoveFromStack(halfStack);

            splitStack = new InventorySlot(itemData, halfStack); //Creates copy of this slot with half the stack size.
            return true;
        }

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {
            if (_itemID == -1) return;

            var db = Resources.Load<ItemDatabase>("ItemDatabase");
            itemData = db.GetItem(_itemID);
        }
    }

}