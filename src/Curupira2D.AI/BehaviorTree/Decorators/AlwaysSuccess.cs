namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
	/// Will always return <see cref="BehaviorState.Success"/>/>
	/// </summary>
    public class AlwaysSuccess : Decorator
    {
        public AlwaysSuccess(Behavior child) : base(child) { }
        internal AlwaysSuccess() : base(null!) { }

        public override BehaviorState Update(IBlackboard blackboard)
        {
            var childState = Child.Update(blackboard);

            State = childState switch
            {
                //BehaviorState.Running => BehaviorState.Running,
                _ => BehaviorState.Success
            };

            return State;
        }
    }
}
