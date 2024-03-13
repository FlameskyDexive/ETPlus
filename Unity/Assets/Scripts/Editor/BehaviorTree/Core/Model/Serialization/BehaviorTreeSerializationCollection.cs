using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using System.Linq;
namespace BehaviorTree.Editor
{
    [Serializable]
    public class BehaviorTreeSerializationPair
    {
        public BehaviorTreeAsset behaviorTreeAsset;
        public TextAsset serializedData;
        public BehaviorTreeSerializationPair(BehaviorTreeAsset behaviorTreeAsset, TextAsset serializedData)
        {
            this.behaviorTreeAsset = behaviorTreeAsset;
            this.serializedData = serializedData;
        }
    }
    [Serializable]
    public class BehaviorTreeSerializationCollection
    {
        public List<BehaviorTreeSerializationPair> serializationPairs;
        public List<string> guids;
        public void SetUp()
        {
            HashSet<TextAsset> serializedDataSet;
            if (serializationPairs != null)
                serializedDataSet = serializationPairs.Select(x => x.serializedData).Where(x => x != null).ToHashSet();
            else
                serializedDataSet = new();
            serializationPairs = new();
            guids = AssetDatabase.FindAssets($"t:{typeof(BehaviorTreeAsset)}").ToList();
            var list = guids.Select(x => AssetDatabase.LoadAssetAtPath<BehaviorTreeAsset>(AssetDatabase.GUIDToAssetPath(x))).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var so = list[i];
                serializationPairs.Add(new BehaviorTreeSerializationPair(
                    so,
                    serializedDataSet.FirstOrDefault(x => x.name == $"{so.name}_{guids[i]}")
                ));
            }
        }
        public void InjectJsonFiles(HashSet<TextAsset> dataSet)
        {
            for (int i = 0; i < serializationPairs.Count; i++)
            {
                serializationPairs[i].serializedData = dataSet.FirstOrDefault(x => x.name == $"{serializationPairs[i].behaviorTreeAsset.name}_{guids[i]}");
            }
        }
        public BehaviorTreeSerializationPair FindSerializationPair(BehaviorTreeAsset behaviorTreeAsset)
        {
            return serializationPairs
            .FirstOrDefault(x => x.behaviorTreeAsset == behaviorTreeAsset);
        }
    }
}
