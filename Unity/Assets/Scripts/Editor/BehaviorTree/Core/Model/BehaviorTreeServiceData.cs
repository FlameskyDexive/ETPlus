using UnityEditor;
using UnityEngine;
namespace BehaviorTree.Editor
{
    public class BehaviorTreeServiceData : ScriptableObject
    {
        public BehaviorTreeSerializationCollection serializationCollection = new();
        public void ForceSetUp()
        {
            serializationCollection.SetUp();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
