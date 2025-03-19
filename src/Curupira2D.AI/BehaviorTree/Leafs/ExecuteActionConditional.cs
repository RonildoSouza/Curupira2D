namespace Curupira2D.AI.BehaviorTree.Leafs
{
    /// <summary>
    /// Wraps an ExecuteAction so that it can be used as a Conditional
    /// </summary>
    public class ExecuteActionConditional(Func<IBlackboard, NodeState> action) : ExecuteAction(action), IConditional { }
}