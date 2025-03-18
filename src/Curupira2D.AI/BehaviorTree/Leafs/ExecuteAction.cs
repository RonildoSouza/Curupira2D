namespace Curupira2D.AI.BehaviorTree.Leafs
{
    /// <summary>
    /// <see cref="ExecuteAction"/> are nodes that define a task to be performed simple actions without the need for subclass.
    /// </summary>
    public class ExecuteAction : ActionLeaf
    {
        private readonly Func<IBlackboard, State> _action;

        public ExecuteAction(Func<IBlackboard, State> action)
        {
            ArgumentNullException.ThrowIfNull(action, nameof(action));
            _action = action;
        }

        public override State Tick(IBlackboard blackboard) => _action(blackboard);
    }
}
