namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// <see cref="Inverter"/> will return <see cref="NodeState.Success"/> if its child returns <see cref="NodeState.Failure"/> and vice versa.
    /// </summary>
    public class Inverter : Decorator
    {
        public Inverter(Node child) : base(child) { }
        internal Inverter() : base(null!) { }

        public override NodeState Update(IBlackboard blackboard)
        {
            var childState = Child.Tick(blackboard);

            if (childState == NodeState.Success)
                return NodeState.Failure;

            if (childState == NodeState.Failure)
                return NodeState.Success;

            return NodeState.Running;
        }
    }
}
