using Curupira2D.AI.BehaviorTree.Leafs;
using System.Diagnostics;

namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// The <see cref="Delay"/> will return <see cref="State.Running"/> for a set amount of time before executing its child.
    /// The timer resets when both it and its child are not <see cref="State.Running"/>
    /// </summary>
    public class Delay : Decorator
    {
        private readonly int _milliseconds;
        private readonly Stopwatch _stopwatch = new();

        public Delay(Node child, int milliseconds) : base(child)
        {
            if (milliseconds < 100)
                throw new ArgumentException("Milliseconds must be greater than 100");

            _milliseconds = milliseconds;
        }

        internal Delay(int milliseconds) : this(null!, milliseconds) { }

        public override State Tick(IBlackboard blackboard)
        {
            if (Child != RunningChild)
                Child.OnBeforeRun(blackboard);

            if (!_stopwatch.IsRunning)
                _stopwatch.Start();

            if (_stopwatch.Elapsed.TotalMilliseconds >= _milliseconds)
            {
                _stopwatch.Reset();
                var childState = Child.Tick(blackboard);

                if (childState == State.Running && Child is ActionLeaf)
                    RunningChild = Child;

                return childState;
            }

            return State.Running;
        }
    }
}
