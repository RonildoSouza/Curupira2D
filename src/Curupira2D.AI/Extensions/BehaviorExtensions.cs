using Curupira2D.AI.BehaviorTree;
using Curupira2D.AI.BehaviorTree.Composites;
using Curupira2D.AI.BehaviorTree.Decorators;
using System.Text;

namespace Curupira2D.AI.Extensions
{
    public static class BehaviorExtensions
    {
        public static string GetBehaviorTreeStructure(this Behavior node, bool withState = false) => node.GetBehaviorTreeStructureWithState(withState);

        internal static string GetBehaviorTreeStructureWithState(this Behavior node, bool withState = true)
        {
            return BuildStringTree(node, withState);

            static string BuildStringTree(Behavior node, bool withState, string prefix = "", bool isLast = true)
            {
                var stateFirstLetter = withState ? $"({node.State.ToString()[0]})" : string.Empty;
                var sb = new StringBuilder($"{prefix}{(isLast ? "└── " : "├── ")}{node.GetType().Name} {stateFirstLetter}\n");
                var childPath = $"{prefix}{(isLast ? "    " : "│   ")}";

                if (node is Composite composite)
                {
                    for (int i = 0; i < composite.Children.Count; i++)
                        sb.Append(BuildStringTree(composite.Children[i], withState, childPath, i == composite.Children.Count - 1));

                    return sb.ToString();
                }

                if (node is Decorator decorator)
                {
                    sb.Append(BuildStringTree(decorator.Child, withState, childPath));
                    return sb.ToString();
                }

                return sb.ToString();
            }
        }
    }
}
