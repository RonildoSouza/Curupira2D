namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
	/// Will always return <see cref="State.Success"/> except when the child node is <see cref="State.Running"/>
	/// </summary>
    public class AlwaysSuccess : Decorator
    {
        public AlwaysSuccess(Node child) : base(child) { }
        internal AlwaysSuccess() : base(null!) { }

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

            return State.Success;
        }
    }
}
