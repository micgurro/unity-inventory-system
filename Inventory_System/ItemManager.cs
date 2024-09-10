using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace meekobytes
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager Instance;
        public static UnityAction<SaveData> OnLoadGame;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            SaveLoad.OnLoadGame += LoadActiveItems;
        }
        public static void OnEnable()
        {
        }

        public static void OnDisable()
        {
            SaveLoad.OnLoadGame -= LoadActiveItems;
        }

        public static void LoadActiveItems(SaveData saveData)
        {
            //Remove active items if they've been collected.
            RefreshActiveItemsInScene(saveData);
        }

        private static void RefreshActiveItemsInScene(SaveData saveData)
        {
            foreach (var id in saveData.activeItemsList)
            {
                saveData.activeItemsDictionary.TryGetValue(id, out ItemPickUpSaveData data);
                Instantiate(data.ItemData.ItemPrefab, data.Position, data.Rotation);
            }
        }
    }
}
