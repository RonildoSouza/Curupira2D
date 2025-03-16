namespace Curupira2D.AI.BehaviorTree.Leafs
{
    /// <summary>
    /// <see cref="ActionLeaf"/> are nodes that define a task to be performed.
    /// Their execution can be long running, potentially being called across multiple frame executions. 
    /// In this case, the node should return <see cref="State.Running"/> until the action is completed.
    /// </summary>
    public abstract class ActionLeaf : Node, ILeaf
    {
    }
}
