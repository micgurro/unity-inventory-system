using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace meekobytes
{
        [CreateAssetMenu(menuName = "Inventory System/Item Database")]
        public class ItemDatabase : ScriptableObject
        {
            [SerializeField] private List<InventoryItemData> _itemDatabase;

            public List<InventoryItemData> ItemDB => _itemDatabase;

            [ContextMenu("Set IDs")]
            public void SetItemIDs()
            {
                _itemDatabase = new List<InventoryItemData>();

                var foundItems = UnityEngine.Resources.LoadAll<InventoryItemData>("ItemData").OrderBy(i => i.ID).ToList();

                var hasIDInRange = foundItems.Where(i => i.ID != -1 && i.ID < foundItems.Count).OrderBy(i => i.ID).ToList();
                var hasIDNotInRange = foundItems.Where(i => i.ID != -1 && i.ID >= foundItems.Count).OrderBy(i => i.ID).ToList();
                var noID = foundItems.Where(i => i.ID == -1).ToList();

                var index = 0;
                for (int i = 0; i < foundItems.Count; i++)
                {
                    Debug.Log($"Checking ID {i}");
                    var itemToAdd = hasIDInRange.Find(d => d.ID == i);

                    if (itemToAdd != null)
                    {
                        Debug.Log($"Found item {itemToAdd} which has an id of {itemToAdd.ID}");
                        _itemDatabase.Add(itemToAdd);
                    }
                    else if (index < noID.Count)
                    {
                        noID[index].ID = i;
                        Debug.Log($"Setting item {noID[index]} which has an invalid id to the id of {i}");
                        itemToAdd = noID[index];
                        index++;
                        _itemDatabase.Add(itemToAdd);
                    }

#if UNITY_EDITOR
                if (itemToAdd) EditorUtility.SetDirty(itemToAdd);
#endif

                }

                foreach (var item in hasIDNotInRange)
                {
                    _itemDatabase.Add(item);
#if UNITY_EDITOR
                if (item) EditorUtility.SetDirty(item);
#endif
                }
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
#endif

            }

            public InventoryItemData GetItem(int id)
            {
                return _itemDatabase.Find(i => i.ID == id);
            }

            public InventoryItemData GetItem(string displayName)
            {
                return _itemDatabase.Find(i => i.DisplayName == displayName);
            }
        }
}