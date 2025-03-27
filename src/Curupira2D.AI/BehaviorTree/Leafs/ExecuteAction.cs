namespace Curupira2D.AI.BehaviorTree.Leafs
{
    /// <summary>
    /// <see cref="ExecuteAction"/> are nodes that define a task to be performed simple actions without the need for subclass.
    /// </summary>
    public class ExecuteAction(Func<IBlackboard, BehaviorState> action) : Leaf
    {
        public override BehaviorState Update(IBlackboard blackboard)
        {
            State = action(blackboard);
            return State;
        }
    }
}
