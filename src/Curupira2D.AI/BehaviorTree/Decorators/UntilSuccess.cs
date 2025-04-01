namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// <see cref="UntilFailure"/> will keep executing its child task until the child node returns <see cref="BehaviorState.Success"/>
    /// </summary>
    public class UntilSuccess : Decorator
    {
        public UntilSuccess(Behavior child) : base(child) { }
        internal UntilSuccess() : base(null!) { }

        public override BehaviorState Update(IBlackboard blackboard)
        {
            var childState = Child.Update(blackboard);

            if (childState != BehaviorState.Success)
                return State = BehaviorState.Running;

            return State = BehaviorState.Success;
        }
    }
}
