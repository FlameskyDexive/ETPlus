using UnityEditor.Experimental.GraphView;
namespace BehaviorTree.Editor
{
    public interface IBlackBoard
    {
        /// <summary>
        /// 添加共享变量
        /// </summary>
        /// <param name="variable"></param>
        void AddSharedVariable(SharedVariable variable);
        Blackboard View { get; }
    }
}