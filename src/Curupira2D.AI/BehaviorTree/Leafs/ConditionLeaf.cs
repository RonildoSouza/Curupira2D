namespace Curupira2D.AI.BehaviorTree.Leafs
{
    /// <summary>
    /// <see cref="ConditionLeaf"/> are nodes that either return <see cref="State.Success"/> or <see cref="State.Failure"/> depending on a single simple condition.
    /// They should never return <see cref="State.Running"/>.
    /// </summary>
    public abstract class ConditionLeaf : Node, ILeaf
    {
    }
}
