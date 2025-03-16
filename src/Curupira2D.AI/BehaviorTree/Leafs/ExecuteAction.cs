namespace Curupira2D.AI.BehaviorTree.Leafs
{
    /// <summary>
    /// <see cref="ExecuteAction"/> are nodes that define a task to be performed simple actions without the need for subclass.
    /// </summary>
    public class ExecuteAction(Func<IBlackboard, State> action) : ActionLeaf
    {
        public override State Tick(IBlackboard blackboard) => action(blackboard);
    }
}
