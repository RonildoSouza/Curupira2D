using System.Diagnostics;

namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// The <see cref="Delay"/> will return <see cref="BehaviorState.Running"/> for a set amount of time before executing its child.
    /// The timer resets when both it and its child are not <see cref="BehaviorState.Running"/>
    /// </summary>
    public class Delay : Decorator
    {
        private readonly int _milliseconds;
        private readonly Stopwatch _stopwatch = new();

        public Delay(Behavior child, int milliseconds) : base(child)
        {
            if (milliseconds < 100)
                throw new ArgumentException("Milliseconds must be greater than 100");

            _milliseconds = milliseconds;
        }

        internal Delay(int milliseconds) : this(null!, milliseconds) { }

        public override BehaviorState Update(IBlackboard blackboard)
        {
            if (!_stopwatch.IsRunning)
                _stopwatch.Start();

            if (_stopwatch.Elapsed.TotalMilliseconds >= _milliseconds)
            {
                _stopwatch.Reset();
                State = Child.Update(blackboard);

                return State;
            }

            return State = BehaviorState.Running;
        }
    }
}
