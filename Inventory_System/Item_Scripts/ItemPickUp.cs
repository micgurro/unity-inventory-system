using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace meekobytes
{
    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(UniqueID))]
    public class ItemPickUp : MonoBehaviour, IInteractable
    {
        public float PickUpRadius = 1f;
        public InventoryItemData ItemData;

        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private float _amplitude = 0.09f;
        [SerializeField] private float _frequency = .5f;

        Vector3 positionOffset = new Vector3();
        Vector3 temporaryOffset = new Vector3();

        private SphereCollider myCollider;
        public UnityAction<IInteractable> OnInteractionComplete { get; set; }
        public string InteractionPrompt => $"Pick Up '{ItemData.DisplayName}'";

        [SerializeField] private ItemPickUpSaveData itemSaveData;
        private string id;

        private void Awake()
        {
            id = GetComponent<UniqueID>().ID;

            myCollider = GetComponent<SphereCollider>();
            myCollider.isTrigger = true;
            if (!ItemData.IsAutoPickup)
                myCollider.isTrigger = false;
            myCollider.radius = PickUpRadius;

            itemSaveData = new ItemPickUpSaveData(ItemData, transform.position, transform.rotation);
            SaveLoad.OnLoadGame += LoadGame;
        }

        private void Start()
        {
            if (!SaveGameManager.Data.activeItemsList.Contains(id))
            {
                SaveGameManager.Data.activeItemsDictionary.Add(id, itemSaveData);
                SaveGameManager.Data.activeItemsList.Add(id);
            }

            positionOffset = transform.position;
        }

        private void Update()
        {
            AnimateFloatAndRotate();
        }

        private void OnTriggerEnter(Collider other)
        {
            var inventory = other.transform.GetComponent<PlayerInventoryHolder>();
            HandlePickUpItem(inventory);
            Destroy(gameObject);
        }

    
        private void OnDestroy()
        {
            RemoveActiveItemsInCache();
        }

        private void OnDisable()
        {
            SaveLoad.OnLoadGame -= LoadGame;
        }

        private void LoadGame(SaveData data)
        {
            Destroy(this.gameObject);
        }

        private void RemoveActiveItemsInCache()
        {
            if (SaveGameManager.Data.activeItemsDictionary.ContainsKey(id))
            {
                SaveGameManager.Data.activeItemsDictionary.Remove(id);
                SaveGameManager.Data.activeItemsList.Remove(id);
            }
        }
        private void HandlePickUpItem(PlayerInventoryHolder inventory)
        {

            if (!inventory) return;
            if (!ItemData.IsAutoPickup) return;
            if (!inventory.AddToInventory(ItemData, 1)) return;
            SaveGameManager.Data.collectedItems.Add(id);
        }

        private void AnimateFloatAndRotate()
        {
            transform.Rotate(new Vector3(0f, Time.deltaTime * _rotationSpeed, 0f), Space.World);

            temporaryOffset = positionOffset;
            temporaryOffset.y += Mathf.Sin(Time.fixedTime * Mathf.PI * _frequency) * _amplitude;
            transform.position = temporaryOffset;
        }

        public void Interact(Interactor interactor, out bool interactSuccessful)
        {
            var inventory = interactor.GetComponent<PlayerInventoryHolder>();
            if (!inventory.AddToInventory(ItemData, 1))
                interactSuccessful = false;
            else
            {
                HandlePickUpItem(inventory);
                Destroy(gameObject);
                interactSuccessful = true;
            }
        }

        public void EndInteraction()
        {        }
    }

    [System.Serializable]
    public struct ItemPickUpSaveData
    {
        public InventoryItemData ItemData;
        public Vector3 Position;
        public Quaternion Rotation;

        public ItemPickUpSaveData(InventoryItemData _data, Vector3 _position, Quaternion _rotation)
        {
            ItemData = _data;
            Position = _position;
            Rotation = _rotation;
        }
    }
}


