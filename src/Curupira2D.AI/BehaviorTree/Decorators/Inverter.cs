namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// <see cref="Inverter"/> will return <see cref="BehaviorState.Success"/> if its child returns <see cref="BehaviorState.Failure"/> and vice versa.
    /// </summary>
    public class Inverter : Decorator
    {
        public Inverter(Behavior child) : base(child) { }
        internal Inverter() : base(null!) { }

        public override BehaviorState Update(IBlackboard blackboard)
        {
            var childState = Child.Update(blackboard);

            State = childState switch
            {
                BehaviorState.Success => BehaviorState.Failure,
                BehaviorState.Failure => BehaviorState.Success,
                _ => childState
            };

            return State;
        }
    }
}
