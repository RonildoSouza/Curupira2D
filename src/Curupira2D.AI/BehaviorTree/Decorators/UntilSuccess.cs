namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// <see cref="UntilFail"/> will keep executing its child task until the child node returns <see cref="NodeState.Success"/>
    /// </summary>
    public class UntilSuccess : Decorator
    {
        public UntilSuccess(Node child) : base(child) { }
        internal UntilSuccess() : base(null!) { }

        public override NodeState Update(IBlackboard blackboard)
        {
            var childState = Child.Update(blackboard);

            if (childState != NodeState.Success)
                return NodeState.Running;

            return NodeState.Success;
        }
    }
}
