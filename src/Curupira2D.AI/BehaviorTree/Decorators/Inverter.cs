namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// <see cref="Inverter"/> will return <see cref="State.Success"/> if its child returns <see cref="State.Failure"/> and vice versa.
    /// </summary>
    public class Inverter : Decorator
    {
        public Inverter(Node child) : base(child) { }
        internal Inverter() : base(null!) { }

        public override State Tick(IBlackboard blackboard)
        {
            if (Child != RunningChild)
                Child.OnBeforeRun(blackboard);

            var childState = Child.Tick(blackboard);

            if (childState == State.Success)
            {
                Child.OnAfterRun(blackboard);
                return State.Failure;
            }

            if (childState == State.Failure)
            {
                Child.OnAfterRun(blackboard);
                return State.Success;
            }

            RunningChild = Child;

            return State.Running;
        }
    }
}
