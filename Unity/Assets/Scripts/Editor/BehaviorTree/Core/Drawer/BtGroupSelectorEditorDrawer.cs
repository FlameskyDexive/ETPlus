using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace BehaviorTree.Editor
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    internal class BtGroupSelectorAttribute : PropertyAttribute { }
    [CustomPropertyDrawer(typeof(BtGroupSelectorAttribute))]
    internal class BtGroupSelectorEditorDrawer : PropertyDrawer
    {
        private static readonly Type[] _Types = { typeof(Action), typeof(Conditional), typeof(Composite), typeof(Decorator) };
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            if (EditorGUI.DropdownButton(position, new GUIContent(property.stringValue, property.tooltip), FocusType.Passive))
            {
                var groups = SubclassSearchUtility.FindSubClassTypes(_Types)
                .Where(x => x.GetCustomAttribute<BtGroupAttribute>() != null)
                .Select(x => SubclassSearchUtility.GetSplittedGroupName(x.GetCustomAttribute<BtGroupAttribute>().Group)[0])
                .Distinct()
                .ToList();
                var menu = new GenericMenu();
                foreach (var group in groups)
                {
                    menu.AddItem(new GUIContent(group), false, () => property.stringValue = group);
                }
                menu.ShowAsContext();
            }
            EditorGUI.EndProperty();
        }
    }
}
