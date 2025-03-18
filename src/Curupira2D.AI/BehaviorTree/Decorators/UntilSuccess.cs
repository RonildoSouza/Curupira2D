namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// <see cref="UntilFail"/> will keep executing its child task until the child node returns <see cref="State.Success"/>
    /// </summary>
    public class UntilSuccess : Decorator
    {
        public UntilSuccess(Node child) : base(child) { }
        internal UntilSuccess() : base(null!) { }

        public override State Tick(IBlackboard blackboard)
        {
            if (Child != RunningChild)
                Child.OnBeforeRun(blackboard);

            var childState = Child.Tick(blackboard);

            if (childState == State.Success)
                return State.Success;

            RunningChild = Child;

            return State.Running;
        }
    }
}
