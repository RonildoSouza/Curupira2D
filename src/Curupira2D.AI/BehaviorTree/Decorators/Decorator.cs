namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// Decorator nodes are used to transform the result received by its child
    /// </summary>
    public abstract class Decorator(Node child) : Node
    {
        protected internal Node Child { get; set; } = child;

        public override void Invalidate(IBlackboard blackboard)
        {
            base.Invalidate(blackboard);
            Child.Invalidate(blackboard);
        }
    }
}
