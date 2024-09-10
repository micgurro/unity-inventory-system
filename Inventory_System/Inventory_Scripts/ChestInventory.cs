using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace meekobytes
{
    [RequireComponent(typeof(UniqueID))]
    public class ChestInventory : InventoryHolder, IInteractable
    {
        [SerializeField] private string _prompt;
        public string InteractionPrompt => _prompt;
        public UnityAction<IInteractable> OnInteractionComplete { get; set; }

        protected override void Awake()
        {
            base.Awake();
            SaveLoad.OnLoadGame += LoadInventory;
        }

        private void Start()
        {
            var chestSaveData = new InventorySaveData(primaryInventorySystem, transform.position, transform.rotation);

            SaveGameManager.Data.chestDictionary.Add(GetComponent<UniqueID>().ID, chestSaveData);
        }

        protected override void LoadInventory(SaveData data)
        {
            // Check the save data for this specific chest's inventory, and if it exists load it in.
            if(data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out InventorySaveData chestData))
            {
                this.primaryInventorySystem = chestData.InvSystem;
                this.transform.position = chestData.Position;
                this.transform.rotation = chestData.Rotation;
            }
        }

        public void Interact(Interactor interactor, out bool interactSuccessful)
        {
            OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem, 0);
            interactSuccessful = true;
        }

        public void EndInteraction()
        {

        }
    }
}