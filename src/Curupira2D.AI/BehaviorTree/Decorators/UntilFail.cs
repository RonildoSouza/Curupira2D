namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// <see cref="UntilFail"/> will keep executing its child task until the child node returns <see cref="BehaviorState.Failure"/>
    /// </summary>
    public class UntilFail : Decorator
    {
        public UntilFail(Behavior child) : base(child) { }
        internal UntilFail() : base(null!) { }

        public override BehaviorState Update(IBlackboard blackboard)
        {
            var childState = Child.Update(blackboard);

            if (childState != BehaviorState.Failure)
                return State = BehaviorState.Running;

            return State = BehaviorState.Success;
        }
    }
}
