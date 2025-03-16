namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// <see cref="Inverter"/> will return <see cref="State.Success"/> if its child returns <see cref="State.Failure"/> and vice versa.
    /// </summary>
    public class Inverter(Node child) : Decorator(child)
    {
        public override State Tick(IBlackboard blackboard)
        {
            if (_child != _runningChild)
                _child.OnBeforeRun(blackboard);

            var childState = _child.Tick(blackboard);

            if (childState == State.Success)
            {
                _child.OnAfterRun(blackboard);
                return State.Failure;
            }

            if (childState == State.Failure)
            {
                _child.OnAfterRun(blackboard);
                return State.Success;
            }

            _runningChild = _child;

            return State.Running;
        }
    }
}
