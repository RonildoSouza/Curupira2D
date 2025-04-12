namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// Decorator nodes are used to transform the result received by its child
    /// </summary>
    public abstract class Decorator(Behavior child) : Behavior
    {
        protected internal Behavior Child { get; set; } = child;

        public override void OnTerminate(IBlackboard blackboard)
        {
            base.OnTerminate(blackboard);
            Child.State = BehaviorState.Invalid;
        }
    }
}
