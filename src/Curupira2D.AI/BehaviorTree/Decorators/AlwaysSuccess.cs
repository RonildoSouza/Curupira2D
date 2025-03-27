namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
	/// Will always return <see cref="BehaviorState.Success"/> except when the child node is <see cref="BehaviorState.Running"/>
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
                BehaviorState.Running => BehaviorState.Running,
                _ => BehaviorState.Success
            };

            return State;
        }
    }
}
