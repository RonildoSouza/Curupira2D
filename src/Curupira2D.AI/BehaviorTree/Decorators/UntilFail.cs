namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// <see cref="UntilFail"/> will keep executing its child task until the child node returns <see cref="State.Failure"/>
    /// </summary>
    public class UntilFail(Node child) : Decorator(child)
    {
        public override State Tick(IBlackboard blackboard)
        {
            if (_child != _runningChild)
                _child.OnBeforeRun(blackboard);

            var childState = _child.Tick(blackboard);

            if (childState == State.Failure)
                return State.Success;

            _runningChild = _child;

            return State.Running;
        }
    }
}
