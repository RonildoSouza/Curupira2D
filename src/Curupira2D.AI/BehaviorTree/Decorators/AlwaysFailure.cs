namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// Will always return <see cref="State.Failure"/> except when the child node is <see cref="State.Running"/>
    /// </summary>
    public class AlwaysFailure : Decorator
    {
        public AlwaysFailure(Node child) : base(child) { }
        internal AlwaysFailure() : base(null!) { }

        public override State Tick(IBlackboard blackboard)
        {
            if (Child != RunningChild)
                Child.OnBeforeRun(blackboard);

            var state = Child.Tick(blackboard);

            if (state == State.Running)
            {
                RunningChild = Child;
                return State.Running;
            }

            Child.OnAfterRun(blackboard);

            return State.Failure;
        }
    }
}
