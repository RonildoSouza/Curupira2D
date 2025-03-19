using Curupira2D.AI.BehaviorTree.Composites;
using Curupira2D.AI.BehaviorTree.Decorators;
using System.Diagnostics;
using System.Text;

namespace Curupira2D.AI.BehaviorTree
{
    /// <summary>
    /// Controls the flow of execution of the entire behavior tree
    /// </summary>
    public class BehaviorTree
    {
        private readonly IBlackboard _blackboard;
        private readonly Node _root;
        private readonly Stopwatch _stopwatch = new();
        private float _updateIntervalInMilliseconds;

        public BehaviorTree(IBlackboard blackboard, Node root, int updateIntervalInMilliseconds = 200)
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
            // updatePeriod less than or equal to 0 will tick every frame
            if (_updateIntervalInMilliseconds <= 0f)
            {
                _root.Tick(_blackboard);
                return;
            }

            if (!_stopwatch.IsRunning)
                _stopwatch.Start();

            // ensure we only tick once for long frames
            if (_stopwatch.Elapsed.TotalMilliseconds >= _updateIntervalInMilliseconds)
            {
                _stopwatch.Reset();
                _root.Tick(_blackboard);
            }
        }

        public string GetStringTree(string prefix = "")
        {
            return BuildStringTree(_root, prefix);

            static string BuildStringTree(Node node, string prefix = "", bool isLast = true)
            {
                var sb = new StringBuilder($"{prefix}{(isLast ? "└── " : "├── ")}{node.GetType().Name} ({node.State.ToString()[0]})\n");
                var childPath = $"{prefix}{(isLast ? "    " : "│   ")}";

                if (node is Composite composite)
                {
                    for (int i = 0; i < composite.Children.Count; i++)
                        sb.Append(BuildStringTree(composite.Children[i], childPath, i == composite.Children.Count - 1));

                    return sb.ToString();
                }

                if (node is Decorator decorator)
                {
                    sb.AppendLine(BuildStringTree(decorator.Child, childPath));
                    return sb.ToString();
                }

                return sb.ToString();
            }
        }
    }
}
