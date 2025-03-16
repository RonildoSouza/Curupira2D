namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// The <see cref="Repeater"/> will execute its child until it returns <see cref="State.Success"/> a certain amount of times.
    /// When the number of maximum ticks is reached, it will return a <see cref="State.Success"/>.
    /// If the child returns <see cref="State.Failure"/>, the repeater will return <see cref="State.Failure"/> immediately.
    /// </summary>
    public class Repeater(Node child, int repeatCount) : Decorator(child)
    {
        private int _currentCount = 0;

        public override State Tick(IBlackboard blackboard)
        {
            if (_currentCount < repeatCount)
            {
                if (_runningChild == null)
                    _child.OnBeforeRun(blackboard);

                var childState = _child.Tick(blackboard);

                if (childState == State.Running)
                {
                    _runningChild = _child;
                    return State.Running;
                }

                _currentCount++;
                _child.OnAfterRun(blackboard);

                if (_runningChild != null)
                    _runningChild = null!;

                if (childState == State.Failure)
                    return State.Failure;

                if (_currentCount >= repeatCount)
                    return State.Success;

                return State.Running;
            }

            return State.Success;
        }
    }
}
