using System.Text;

namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// Decorator nodes are used to transform the result received by its child
    /// </summary>
    public abstract class Decorator(Node child) : Node
    {
        protected internal Node Child { get; set; } = child;
        protected Node? RunningChild { get; set; }

        public override void Interrupt(IBlackboard blackboard)
        {
            Child.Interrupt(blackboard);
            base.Interrupt(blackboard);
        }

        public override void OnAfterRun(IBlackboard blackboard) => RunningChild = null!;

        internal override string BuildStringTree(string prefix = "", bool isLast = true)
        {
            var sb = new StringBuilder($"{prefix}{(isLast ? "└── " : "├── ")}{GetType().Name}\n");
            sb.AppendLine(Child.BuildStringTree($"{prefix}{(isLast ? "    " : "│   ")}"));

            return sb.ToString();
        }
    }
}
