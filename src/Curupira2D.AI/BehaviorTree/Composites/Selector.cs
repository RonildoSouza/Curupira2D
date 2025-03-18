namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>
    /// <see cref="Selector"/> nodes will attempt to execute each of its children until one of them return <see cref="State.Success"/>. 
    /// If all children return <see cref="State.Failure"/>, this node will also return <see cref="State.Failure"/>.
    /// If a child returns <see cref="State.Running"/> it will tick again.
    /// </summary>
    public class Selector : Composite
    {
        public override State Tick(IBlackboard blackboard)
        {
            foreach (var child in Children)
            {
                if (child != RunningChild)
                    child.OnBeforeRun(blackboard);

                var childState = child.Tick(blackboard);

                if (childState == State.Success)
                {
                    child.OnAfterRun(blackboard);
                    return State.Success;
                }

                if (childState == State.Running)
                {
                    RunningChild = child;
                    return State.Running;
                }

                child.OnAfterRun(blackboard);
            }

            return State.Failure;
        }
    }
}
