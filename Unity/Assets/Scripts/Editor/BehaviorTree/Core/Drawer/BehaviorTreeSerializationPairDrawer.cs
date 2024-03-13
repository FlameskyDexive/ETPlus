using System.IO;
using UnityEditor;
using UnityEngine;
namespace BehaviorTree.Editor
{
    [CustomPropertyDrawer(typeof(BehaviorTreeSerializationPair))]
    public class BehaviorTreeSerializationPairDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var serializedProperty = property.FindPropertyRelative("behaviorTreeAsset");
            var textProperty = property.FindPropertyRelative("serializedData");
            position.width /= 3;
            GUI.enabled = false;
            EditorGUI.PropertyField(position, serializedProperty, GUIContent.none);
            GUI.enabled = true;
            position.x += position.width;
            EditorGUI.PropertyField(position, textProperty, GUIContent.none);
            position.x += position.width;
            position.width /= 2;
            GUI.enabled = textProperty.objectReferenceValue != null;
            if (GUI.Button(position, "Serialize"))
            {
                var serializedData = SerializeUtility.SerializeTree(serializedProperty.objectReferenceValue as BehaviorTreeAsset, true, true);
                var textPath = Application.dataPath + AssetDatabase.GetAssetPath(textProperty.objectReferenceValue).Replace("Assets", string.Empty);
                File.WriteAllText(textPath, serializedData);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            position.x += position.width;
            if (GUI.Button(position, "Deserialize"))
            {
                var treeAsset = serializedProperty.objectReferenceValue as BehaviorTreeAsset;
                Undo.RecordObject(treeAsset, "Deserialize from json file");
                treeAsset.Deserialize((textProperty.objectReferenceValue as TextAsset).text);
                EditorUtility.SetDirty(treeAsset);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            GUI.enabled = true;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var soProperty = property.FindPropertyRelative("behaviorTreeAsset");
            return base.GetPropertyHeight(soProperty, label);
        }
    }
}
