using UnityEditor.Experimental.GraphView;
namespace BehaviorTree.Editor
{
    public interface IChildPortable
    {
        Port Child { get; }
    }
}
