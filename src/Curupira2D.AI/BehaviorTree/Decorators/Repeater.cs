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

        internal Repeater(int repeatCount) : this(null!, repeatCount) { }

        public override State Tick(IBlackboard blackboard)
        {
            if (_currentCount < repeatCount)
            {
                if (RunningChild == null)
                    Child.OnBeforeRun(blackboard);

                var childState = Child.Tick(blackboard);

                if (childState == State.Running)
                {
                    RunningChild = Child;
                    return State.Running;
                }

                _currentCount++;
                Child.OnAfterRun(blackboard);

                if (RunningChild != null)
                    RunningChild = null!;

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
