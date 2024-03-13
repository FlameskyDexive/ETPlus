using System.Reflection;
using UnityEngine.UIElements;
namespace BehaviorTree.Editor
{
    public class InfoView : VisualElement
    {
        public InfoView(string info)
        {
            Clear();
            IMGUIContainer container = new();
            container.Add(new Label(info));
            Add(container);
        }
        public void UpdateSelection(IBehaviorTreeNode node)
        {
            Clear();
            IMGUIContainer container = new();
            BtInfoAttribute infoAttribute;
            if ((infoAttribute = node.GetBehavior().GetCustomAttribute<BtInfoAttribute>()) != null)
            {
                container.Add(new Label(infoAttribute.Description));
            }
            Add(container);
        }
    }
}