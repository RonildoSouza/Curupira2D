namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// <see cref="UntilFail"/> will keep executing its child task until the child node returns <see cref="NodeState.Failure"/>
    /// </summary>
    public class UntilFail : Decorator
    {
        public UntilFail(Node child) : base(child) { }
        internal UntilFail() : base(null!) { }

        public override NodeState Update(IBlackboard blackboard)
        {
            var childState = Child.Tick(blackboard);

            if (childState != NodeState.Failure)
                return NodeState.Running;

            return NodeState.Success;
        }
    }
}
