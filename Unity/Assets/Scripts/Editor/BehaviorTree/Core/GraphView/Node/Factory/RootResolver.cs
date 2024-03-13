using System;
namespace BehaviorTree.Editor
{
    public class RootResolver : INodeResolver
    {
        public IBehaviorTreeNode CreateNodeInstance(Type type)
        {
            return new RootNode();
        }
        public static bool IsAcceptable(Type behaviorType) => behaviorType == typeof(Root);
    }
}
