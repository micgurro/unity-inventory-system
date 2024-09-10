using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace meekobytes
{
    public class PlayerInventoryHolder : InventoryHolder
    {
        public static UnityAction OnPlayerInventoryChanged;
        public static UnityAction<InventorySystem, int> OnPlayerInventoryDisplayRequested;

        private void Start()
        {
            SaveGameManager.Data.playerInventory = new InventorySaveData(primaryInventorySystem);
        }

        protected override void LoadInventory(SaveData data)
        {
            // Check the save data for this specific chest's inventory, and if it exists load it in.
            if (data.playerInventory.InvSystem != null)
            {
                this.primaryInventorySystem = data.playerInventory.InvSystem;
                OnPlayerInventoryChanged?.Invoke();
            }
        }

        void Update()
        {
            if (Keyboard.current.iKey.wasPressedThisFrame) OnPlayerInventoryDisplayRequested?.Invoke(primaryInventorySystem, offset);
        }

        public bool AddToInventory(InventoryItemData data, int amount)
        {
            if (primaryInventorySystem.AddToInventory(data, amount))
            {
                return true;
            }

            return false;
        }
    }
}
