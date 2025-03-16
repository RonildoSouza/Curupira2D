namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
	/// Will always return <see cref="State.Success"/> except when the child node is <see cref="State.Running"/>
	/// </summary>
    public class AlwaysSuccess(Node child) : Decorator(child)
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

            return State.Success;
        }
    }
}
