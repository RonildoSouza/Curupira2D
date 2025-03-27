namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// Will always return <see cref="BehaviorState.Failure"/> except when the child node is <see cref="BehaviorState.Running"/>
    /// </summary>
    public class AlwaysFailure : Decorator
    {
        public AlwaysFailure(Behavior child) : base(child) { }
        internal AlwaysFailure() : base(null!) { }

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
