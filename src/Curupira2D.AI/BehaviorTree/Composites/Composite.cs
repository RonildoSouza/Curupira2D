using System.Text;

namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>
    /// Composite nodes manage child nodes and determine how they are executed.
    /// </summary>
    public abstract class Composite : Node
    {
        protected internal IList<Node> Children { get; set; } = [];
        protected Node? RunningChild { get; set; }

        public Composite AddChild(Node child)
        {
            Children.Add(child);
            return this;
        }

        public override void Interrupt(IBlackboard blackboard)
        {
            if (RunningChild != null)
            {
                RunningChild.Interrupt(blackboard);
                RunningChild = null!;
            }

            base.Interrupt(blackboard);
        }

        public override void OnAfterRun(IBlackboard blackboard) => RunningChild = null!;

        internal override string BuildStringTree(string prefix = "", bool isLast = true)
        {
            var sb = new StringBuilder($"{prefix}{(isLast ? "└── " : "├── ")}{GetType().Name}\n");

            for (int i = 0; i < Children.Count; i++)
                sb.Append(Children[i].BuildStringTree($"{prefix}{(isLast ? "    " : "│   ")}", i == Children.Count - 1));

            return sb.ToString();
        }
    }
}
