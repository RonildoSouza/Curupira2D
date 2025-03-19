namespace Curupira2D.AI.BehaviorTree.Leafs
{
    /// <summary>
    /// <see cref="ConditionLeaf"/> are nodes that either return <see cref="NodeState.Success"/> or <see cref="NodeState.Failure"/> depending on a single simple condition.
    /// They should never return <see cref="NodeState.Running"/>.
    /// </summary>
    public abstract class ConditionLeaf : Node, ILeaf, IConditional
    {
    }
}
