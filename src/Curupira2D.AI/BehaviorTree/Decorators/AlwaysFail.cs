namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// Will always return <see cref="NodeState.Failure"/> except when the child node is <see cref="NodeState.Running"/>
    /// </summary>
    public class AlwaysFail : Decorator
    {
        public AlwaysFail(Node child) : base(child) { }
        internal AlwaysFail() : base(null!) { }

        public override NodeState Update(IBlackboard blackboard)
        {
            var state = Child.Update(blackboard);

            if (state == NodeState.Running)
                return NodeState.Running;

            return NodeState.Failure;
        }
    }
}
