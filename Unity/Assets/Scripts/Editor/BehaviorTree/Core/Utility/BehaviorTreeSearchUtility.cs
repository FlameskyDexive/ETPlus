using System;
using System.Collections.Generic;
using UnityEditor;
namespace BehaviorTree.Editor
{
    public class BehaviorTreeSearchUtility
    {
        public static List<BehaviorTreeSerializationPair> SearchBehaviorTreeAsset(Type searchType)
        {
            return SearchBehaviorTreeAsset(searchType, BehaviorTreeSetting.GetOrCreateSettings().ServiceData, GetAllBehaviorTreeAssets());
        }
        public static List<BehaviorTreeSerializationPair> SearchBehaviorTreeAsset(Type searchType, BehaviorTreeServiceData serviceData, List<BehaviorTreeAsset> searchList)
        {
            if (serviceData == null) serviceData = BehaviorTreeSetting.GetOrCreateSettings().ServiceData;
            searchList ??= GetAllBehaviorTreeAssets();
            List<BehaviorTreeAsset> treeAssets = new();
            List<BehaviorTreeSerializationPair> pairs = new();
            foreach (var treeAsset in searchList)
            {
                SearchBehavior(treeAsset, searchType, treeAssets);
            }
            foreach (var so in treeAssets)
            {
                var pair = serviceData.serializationCollection.FindSerializationPair(so);
                pairs.Add(new BehaviorTreeSerializationPair(so, pair.serializedData));
            }
            return pairs;
        }
        public static List<BehaviorTreeAsset> GetAllBehaviorTreeAssets()
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(BehaviorTreeAsset)}");
            List<BehaviorTreeAsset> behaviorTreeAssets = new();
            foreach (var guid in guids)
            {
                behaviorTreeAssets.Add(AssetDatabase.LoadAssetAtPath<BehaviorTreeAsset>(AssetDatabase.GUIDToAssetPath(guid)));
            }
            return behaviorTreeAssets;
        }
        private static void SearchBehavior(BehaviorTreeAsset treeAsset, Type checkType, List<BehaviorTreeAsset> behaviorTreeAssets)
        {
            if (checkType == null)
            {
                behaviorTreeAssets.Add(treeAsset);
                return;
            }
            foreach (var node in treeAsset.Traverse())
            {
                if (node.GetType() == checkType)
                {
                    behaviorTreeAssets.Add(treeAsset);
                    return;
                }
            }
        }
    }
}
