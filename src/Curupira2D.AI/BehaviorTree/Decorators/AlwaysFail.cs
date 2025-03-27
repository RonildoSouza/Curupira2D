namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// Will always return <see cref="BehaviorState.Failure"/> except when the child node is <see cref="BehaviorState.Running"/>
    /// </summary>
    public class AlwaysFail : Decorator
    {
        public AlwaysFail(Behavior child) : base(child) { }
        internal AlwaysFail() : base(null!) { }

        public override BehaviorState Update(IBlackboard blackboard)
        {
            var childState = Child.Update(blackboard);

            State = childState switch
            {
                BehaviorState.Running => BehaviorState.Running,
                _ => BehaviorState.Failure
            };

            return State;
        }
    }
}
