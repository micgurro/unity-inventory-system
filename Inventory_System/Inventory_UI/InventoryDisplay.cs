using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace meekobytes
{
    public abstract class InventoryDisplay : MonoBehaviour
    {
        [SerializeField] MouseItemData mouseInventoryItem;

        protected InventorySystem inventorySystem;
        protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary; // Pair up the UI slots with the system slots in dictionary.

        public InventorySystem InventorySystem => inventorySystem;
        public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;

        protected virtual void Start()
        {

        }

        public abstract void AssignSlot(InventorySystem invToDisplay, int offset); // Implemented in child classes.

        protected virtual void UpdateSlot(InventorySlot updatedSlot)
        {
            foreach (var slot in SlotDictionary)
            {
                if (slot.Value == updatedSlot) // Slot value - the backend inventory slot.
                {
                    slot.Key.UpdateUISlot(updatedSlot); //Slot key - UI representation of the value
                }
            }
        }

        public void SlotClicked(InventorySlot_UI clickedUISlot)
        {
            bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed;

            //Does the clicked slot have the item data - DOes the mouse have no item data
            if (clickedUISlot.AssignedInventorySlot.ItemData != null && 
                mouseInventoryItem.AssignedInventorySlot.ItemData == null)
            {
                // If player is holding shift key? Split the stack.

                if (isShiftPressed && clickedUISlot.AssignedInventorySlot.SplitStack(out InventorySlot halfStackSlot)) //split stack
                {
                    mouseInventoryItem.UpdateMouseSlot(halfStackSlot);
                    clickedUISlot.UpdateUISlot();
                    return;
                }
                else //Pick up the item in the clicked slot.
                {
                    mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
                    clickedUISlot.ClearSlot();
                    return;
                }
            }

            // Clicked slot doesn't have an item - Mouse does have an item - place mouse item into empty slot

            if (clickedUISlot.AssignedInventorySlot.ItemData == null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
            {
                clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
                clickedUISlot.UpdateUISlot();

                mouseInventoryItem.ClearSlot();
                return;
            }


            // Are both items the same? If so combine

            // Is the slot stack size + mouse stack size > the slot max stack size? if so, take from mouse.

            // If different items, then swap the items.
            // Both slots have an item - decide what to do...
            if (clickedUISlot.AssignedInventorySlot.ItemData != null && mouseInventoryItem.AssignedInventorySlot.ItemData != null)
            {
                bool isSameItem = clickedUISlot.AssignedInventorySlot.ItemData == mouseInventoryItem.AssignedInventorySlot.ItemData;

                if (isSameItem && 
                    clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize))
                {
                    clickedUISlot.AssignedInventorySlot.AssignItem(mouseInventoryItem.AssignedInventorySlot);
                    clickedUISlot.UpdateUISlot();

                    mouseInventoryItem.ClearSlot();
                    return;
                }
                else if (isSameItem && 
                    !clickedUISlot.AssignedInventorySlot.EnoughRoomLeftInStack(mouseInventoryItem.AssignedInventorySlot.StackSize,
                    out int leftInStack))
                {
                    if (leftInStack < 1) SwapSlots(clickedUISlot); // Stack is full so swap items.
                    else //Slot is not at max, so take what's needed from the mouse inventory
                    {
                        int remainingOnMouse = mouseInventoryItem.AssignedInventorySlot.StackSize - leftInStack;

                        clickedUISlot.AssignedInventorySlot.AddToStack(leftInStack);
                        clickedUISlot.UpdateUISlot();

                        var newItem = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, remainingOnMouse);
                        mouseInventoryItem.ClearSlot();
                        mouseInventoryItem.UpdateMouseSlot(newItem);
                        return;
                    }
                }
                else if (!isSameItem)
                {
                    SwapSlots(clickedUISlot);
                    return;
                }
            }
        }

        private void SwapSlots(InventorySlot_UI clickedUISlot)
        {
            var clonedSlot = new InventorySlot(mouseInventoryItem.AssignedInventorySlot.ItemData, mouseInventoryItem.AssignedInventorySlot.StackSize);
            mouseInventoryItem.ClearSlot();

            mouseInventoryItem.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);

            clickedUISlot.ClearSlot();
            clickedUISlot.AssignedInventorySlot.AssignItem(clonedSlot);
            clickedUISlot.UpdateUISlot();
        }
    }

}