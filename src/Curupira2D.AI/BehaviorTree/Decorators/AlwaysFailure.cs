namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
	/// Will always return <see cref="State.Failure"/> except when the child node is <see cref="State.Running"/>
	/// </summary>
    public class AlwaysFailure(Node child) : Decorator(child)
    {
        public override State Tick(IBlackboard blackboard)
        {
            if (_child != _runningChild)
                _child.OnBeforeRun(blackboard);

            var state = _child.Tick(blackboard);

            if (state == State.Running)
            {
                _runningChild = _child;
                return State.Running;
            }

            _child.OnAfterRun(blackboard);

            return State.Failure;
        }
    }
}
