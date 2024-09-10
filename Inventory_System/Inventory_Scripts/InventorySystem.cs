using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace meekobytes
{
    [System.Serializable]
    public class InventorySystem
    {
        [SerializeField] private List<InventorySlot> inventorySlots;

        public List<InventorySlot> InventorySlots => inventorySlots;

        public int InventorySize => InventorySlots.Count;

        public UnityAction<InventorySlot> OnInventorySlotChanged;

        public InventorySystem(int size) // Constructor that sets amount of slots.
        {
            inventorySlots = new List<InventorySlot>(size);

            for (int i = 0; i < size; i++)
            {
                inventorySlots.Add(new InventorySlot());
            }
        }

        public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
        {
            if (ContainsItem(itemToAdd, out List<InventorySlot> invSlot)) // Check if item is in inventory
            {
                foreach (var slot in invSlot)
                {
                    if (slot.EnoughRoomLeftInStack(amountToAdd))
                    {
                        slot.AddToStack(amountToAdd);
                        OnInventorySlotChanged?.Invoke(slot);
                        return true;
                    }
                }
            }

            if (HasFreeSlot(out InventorySlot freeSlot)) // Get first available slot
            {
                if (freeSlot.EnoughRoomLeftInStack(amountToAdd))
                {
                    freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
                    OnInventorySlotChanged?.Invoke(freeSlot);
                    return true;
                }
                // Add implementation to only take what can fill the stack, and check for another free slot to put the remainder in.
            }

            return false;
        }

        public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlot) 
        // Do any of our slots have the item to add in them. If they do get a list of all of them.
        {
            invSlot = InventorySlots.Where(i => i.ItemData == itemToAdd).ToList();
            return invSlot == null ? false : true; // If they do return true if not false.
        }

        public bool HasFreeSlot(out InventorySlot freeSlot)
        {
            freeSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null); // Get the first free slot.
            return freeSlot == null ? false : true;
        }
    }
}