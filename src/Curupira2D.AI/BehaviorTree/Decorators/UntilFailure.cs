namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// <see cref="UntilFailure"/> will keep executing its child task until the child node returns <see cref="BehaviorState.Failure"/>
    /// </summary>
    public class UntilFailure : Decorator
    {
        public UntilFailure(Behavior child) : base(child) { }
        internal UntilFailure() : base(null!) { }

        public override BehaviorState Update(IBlackboard blackboard)
        {
            var childState = Child.Update(blackboard);

            if (childState != BehaviorState.Failure)
                return State = BehaviorState.Running;

            return State = BehaviorState.Success;
        }
    }
}
