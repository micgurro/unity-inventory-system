using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace meekobytes
{
        [CreateAssetMenu(menuName = "Character System/NPC/NPC Actor Database")]
        public class NPCActorDatabase : ScriptableObject
        {
            [SerializeField] private List<NPCActorData> _npcActorDatabase;

            public List<NPCActorData> NPCActorDB => _npcActorDatabase;

            [ContextMenu("Set IDs")]
            public void SetItemIDs()
            {
                _npcActorDatabase = new List<NPCActorData>();

                var foundNpcActors = UnityEngine.Resources.LoadAll<NPCActorData>("NPCActorData").OrderBy(i => i.ID).ToList();

                var hasIDInRange = foundNpcActors.Where(i => i.ID != -1 && i.ID < foundNpcActors.Count).OrderBy(i => i.ID).ToList();
                var hasIDNotInRange = foundNpcActors.Where(i => i.ID != -1 && i.ID >= foundNpcActors.Count).OrderBy(i => i.ID).ToList();
                var noID = foundNpcActors.Where(i => i.ID == -1).ToList();

                var index = 0;
                for (int i = 0; i < foundNpcActors.Count; i++)
                {
                    Debug.Log($"Checking ID {i}");
                    var npcActorToAdd = hasIDInRange.Find(d => d.ID == i);

                    if (npcActorToAdd != null)
                    {
                        Debug.Log($"Found NPC Actor {npcActorToAdd} which has an id of {npcActorToAdd.ID}");
                        _npcActorDatabase.Add(npcActorToAdd);
                    }
                    else if (index < noID.Count)
                    {
                        noID[index].ID = i;
                        Debug.Log($"Setting NPC actor {noID[index]} which has an invalid id to the id of {i}");
                        npcActorToAdd = noID[index];
                        index++;
                        _npcActorDatabase.Add(npcActorToAdd);
                    }

#if UNITY_EDITOR
                if (npcActorToAdd) EditorUtility.SetDirty(npcActorToAdd);
#endif

                }

                foreach (var item in hasIDNotInRange)
                {
                    _npcActorDatabase.Add(item);
#if UNITY_EDITOR
                if (item) EditorUtility.SetDirty(item);
#endif
                }
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
#endif

            }

            public NPCActorData GetActor(int id)
            {
                return _npcActorDatabase.Find(i => i.ID == id);
            }

            public NPCActorData GetActor(string displayName)
            {
                return _npcActorDatabase.Find(i => i.ActorName == displayName);
            }
        }
}