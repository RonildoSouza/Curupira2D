namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// Decorator nodes are used to transform the result received by its child
    /// </summary>
    public abstract class Decorator(Node child) : Node
    {
        protected Node _runningChild = null!;
        protected Node _child = child;

        public override void Interrupt(IBlackboard blackboard)
        {
            _child.Interrupt(blackboard);
            base.Interrupt(blackboard);
        }

        public override void OnAfterRun(IBlackboard blackboard) => _runningChild = null!;
    }
}
