using Curupira2D.AI.BehaviorTree.Leafs;
using System.Diagnostics;

namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// The <see cref="Delay"/> will return <see cref="State.Running"/> for a set amount of time before executing its child.
    /// The timer resets when both it and its child are not <see cref="State.Running"/>
    /// </summary>
    public class Delay(Node child, int milliseconds) : Decorator(child)
    {
        private readonly Stopwatch _stopwatch = new();

        public override State Tick(IBlackboard blackboard)
        {
            if (_child != _runningChild)
                _child.OnBeforeRun(blackboard);

            if (!_stopwatch.IsRunning)
                _stopwatch.Start();

            if (_stopwatch.Elapsed.TotalMilliseconds >= milliseconds)
            {
                _stopwatch.Reset();
                var childState = _child.Tick(blackboard);

                if (childState == State.Running && _child is ActionLeaf)
                    _runningChild = _child;

                return childState;
            }

            return State.Running;
        }
    }
}
