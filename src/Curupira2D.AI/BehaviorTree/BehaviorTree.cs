using Curupira2D.AI.BehaviorTree.Composites;
using Curupira2D.AI.BehaviorTree.Decorators;
using System.Diagnostics;

namespace Curupira2D.AI.BehaviorTree
{
    /// <summary>
    /// Controls the flow of execution of the entire behavior tree
    /// </summary>
    public class BehaviorTree
    {
        private static IBlackboard _blackboard;
        private static Node _root;
        private static readonly Stopwatch _stopwatch = new();
        private static float _updateIntervalInMilliseconds;
        private State _state = State.Running;

        private BehaviorTree() { }

        public BehaviorTree(IBlackboard blackboard, Node root, int updateIntervalInMilliseconds = 1000)
        {
            ArgumentNullException.ThrowIfNull(blackboard, nameof(blackboard));
            ArgumentNullException.ThrowIfNull(root, nameof(root));

            if (root is not Composite && root is not Decorator)
                throw new ArgumentException("Root node must be a Composite or Decorator");

            _blackboard = blackboard;
            _root = root;
            _updateIntervalInMilliseconds = updateIntervalInMilliseconds;
        }

        public BehaviorTree SetUpdateIntervalInMilliseconds(int updateIntervalInMilliseconds)
        {
            _updateIntervalInMilliseconds = updateIntervalInMilliseconds;
            return this;
        }

        public void Tick()
        {
            if (_state != State.Running)
                _root.OnBeforeRun(_blackboard);

            if (_updateIntervalInMilliseconds == 0f)
            {
                _state = _root.Tick(_blackboard);

                if (_state != State.Running)
                    _root.OnAfterRun(_blackboard);

                return;
            }

            if (!_stopwatch.IsRunning)
                _stopwatch.Start();

            if (_stopwatch.Elapsed.TotalMilliseconds >= _updateIntervalInMilliseconds)
            {
                _stopwatch.Reset();
                _state = _root.Tick(_blackboard);
            }

            if (_state != State.Running)
                _root.OnAfterRun(_blackboard);
        }

        public void Interrupt()
        {
            _root.Interrupt(_blackboard);
            _state = State.Running;
        }
    }
}
