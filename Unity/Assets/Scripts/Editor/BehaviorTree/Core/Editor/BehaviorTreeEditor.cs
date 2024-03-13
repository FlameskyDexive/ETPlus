using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
namespace BehaviorTree.Editor
{
    [CustomEditor(typeof(BehaviorTree))]
    public class BehaviorTreeEditor : UnityEditor.Editor
    {
        private const string LabelText = "BehaviorTree <size=12>Version1.4.2</size>";
        private const string ButtonText = "Edit BehaviorTree";
        private const string DebugText = "Debug BehaviorTree";
        private IBehaviorTree Tree => target as IBehaviorTree;
        public override VisualElement CreateInspectorGUI()
        {
            var myInspector = new VisualElement();
            var label = new Label(LabelText);
            label.style.fontSize = 20;
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            myInspector.Add(label);
            myInspector.styleSheets.Add(BehaviorTreeSetting.GetInspectorStyle("BT"));
            var toggle = new PropertyField(serializedObject.FindProperty("updateType"), "Update Type");
            myInspector.Add(toggle);
            var field = new PropertyField(serializedObject.FindProperty("externalBehaviorTree"), "External BehaviorTree");
            field.SetEnabled(!Application.isPlaying);
            myInspector.Add(field);
            if (Tree.SharedVariables.Count != 0)
            {
                myInspector.Add(new SharedVariablesFoldout(Tree, target, this));
            }
            var button = BehaviorTreeEditorUtility.GetButton(() => { GraphEditorWindow.Show(Tree); });
            if (!Application.isPlaying)
            {
                button.style.backgroundColor = new StyleColor(new Color(140 / 255f, 160 / 255f, 250 / 255f));
                button.text = ButtonText;
            }
            else
            {
                button.text = DebugText;
                button.style.backgroundColor = new StyleColor(new Color(253 / 255f, 163 / 255f, 255 / 255f));
            }
            myInspector.Add(button);
            return myInspector;
        }
    }
    [CustomEditor(typeof(BehaviorTreeAsset))]
    public class BehaviorTreeAssetEditor : UnityEditor.Editor
    {
        private IBehaviorTree Tree => target as IBehaviorTree;
        private const string LabelText = "BehaviorTreeAsset <size=12>Version1.4.2</size>";
        private const string ButtonText = "Edit BehaviorTreeAsset";
        private const string DebugText = "Debug BehaviorTreeAsset";
        public override VisualElement CreateInspectorGUI()
        {
            var myInspector = new VisualElement();
            var label = new Label(LabelText);
            label.style.fontSize = 20;
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            myInspector.Add(label);
            myInspector.styleSheets.Add(BehaviorTreeSetting.GetInspectorStyle("BT"));
            var description = new PropertyField(serializedObject.FindProperty("Description"), string.Empty);
            myInspector.Add(description);
            if (Tree.SharedVariables.Count != 0)
            {
                myInspector.Add(new SharedVariablesFoldout(Tree, target, this));
            }
            var button = BehaviorTreeEditorUtility.GetButton(() => { GraphEditorWindow.Show(Tree); });
            if (!Application.isPlaying)
            {
                button.style.backgroundColor = new StyleColor(new Color(140 / 255f, 160 / 255f, 250 / 255f));
                button.text = ButtonText;
            }
            else
            {
                button.text = DebugText;
                button.style.backgroundColor = new StyleColor(new Color(253 / 255f, 163 / 255f, 255 / 255f));
            }
            myInspector.Add(button);
            return myInspector;
        }
    }
}