namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// <see cref="UntilFail"/> will keep executing its child task until the child node returns <see cref="State.Failure"/>
    /// </summary>
    public class UntilFail : Decorator
    {
        public UntilFail(Node child) : base(child) { }
        internal UntilFail() : base(null!) { }

        public override State Tick(IBlackboard blackboard)
        {
            if (Child != RunningChild)
                Child.OnBeforeRun(blackboard);

            var childState = Child.Tick(blackboard);

            if (childState == State.Failure)
                return State.Success;

            RunningChild = Child;

            return State.Running;
        }
    }
}
