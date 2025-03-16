namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>
    /// <see cref="Selector"/> nodes will attempt to execute each of its children until one of them return <see cref="State.Success"/>. 
    /// If all children return <see cref="State.Failure"/>, this node will also return <see cref="State.Failure"/>.
    /// If a child returns <see cref="State.Running"/> it will tick again.
    /// </summary>
    public class Selector : Composite
    {
        public Selector() { }
        public Selector(IList<Node> children) => _children = children;

        public override State Tick(IBlackboard blackboard)
        {
            foreach (var child in _children)
            {
                if (child != _runningChild)
                    child.OnBeforeRun(blackboard);

                var childState = child.Tick(blackboard);

                if (childState == State.Success)
                {
                    child.OnAfterRun(blackboard);
                    return State.Success;
                }

                if (childState == State.Running)
                {
                    _runningChild = child;
                    return State.Running;
                }

                child.OnAfterRun(blackboard);
            }

            return State.Failure;
        }
    }
}
