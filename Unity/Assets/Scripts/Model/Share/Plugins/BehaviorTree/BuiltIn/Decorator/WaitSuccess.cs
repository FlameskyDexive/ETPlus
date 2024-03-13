namespace BehaviorTree.Extend
{
    [BtInfo("Decorator : Return Running until the child node returns Success")]
    public class WaitSuccess : Decorator
    {
        protected override Status OnDecorate(Status childStatus)
        {
            if (childStatus == Status.Success)
                return Status.Success;
            else
                return Status.Running;
        }
    }
}