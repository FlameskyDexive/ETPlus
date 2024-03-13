using UnityEngine;
namespace BehaviorTree.Extend
{
    [BtInfo("Action : Log some text")]
    [BtLabel("Debug : Log")]
    [BtGroup("Debug")]
    public class DebugLog : Action
    {
        [SerializeField]
        private SharedString logText;
        public override void Awake()
        {
            InitVariable(logText);
        }
        protected override Status OnUpdate()
        {
            Debug.Log(logText.Value, GameObject);
            return Status.Success;
        }
    }
}
