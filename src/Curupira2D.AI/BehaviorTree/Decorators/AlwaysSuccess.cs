namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
	/// Will always return <see cref="NodeState.Success"/> except when the child node is <see cref="NodeState.Running"/>
	/// </summary>
    public class AlwaysSuccess : Decorator
    {
        public AlwaysSuccess(Node child) : base(child) { }
        internal AlwaysSuccess() : base(null!) { }

        public override NodeState Update(IBlackboard blackboard)
        {
            var state = Child.Update(blackboard);

            if (state == NodeState.Running)
                return NodeState.Running;

            return NodeState.Success;
        }
    }
}
