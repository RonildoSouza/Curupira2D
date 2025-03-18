namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>    
    /// <see cref="Sequence"/> nodes will attempt to execute all of its children and report <see cref="State.Success"/> in case all of the children report a <see cref="State.Success"/>.
    /// If at least one child reports a <see cref="State.Failure"/>, this node will also return <see cref="State.Failure"/> and restart.
    /// In case a child returns <see cref="State.Running"/> this node will tick again.
    /// </summary>
    public class Sequence : Composite
    {
        public override State Tick(IBlackboard blackboard)
        {
            foreach (var child in Children)
            {
                if (child != RunningChild)
                    child.OnBeforeRun(blackboard);

                var childState = child.Tick(blackboard);

                if (childState == State.Failure)
                {
                    // Interrupt any child that was RUNNING before
                    Interrupt(blackboard);
                    child.OnAfterRun(blackboard);

                    return State.Failure;
                }

                if (childState == State.Running)
                {
                    if (child != RunningChild)
                    {
                        RunningChild?.Interrupt(blackboard);
                        RunningChild = child;
                    }

                    return State.Running;
                }

                child.OnAfterRun(blackboard);
            }

            return State.Success;
        }
    }
}
