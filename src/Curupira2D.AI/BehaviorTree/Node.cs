namespace Curupira2D.AI.BehaviorTree
{
    /// <summary>
    /// Represents a node in a behavior tree. Every node must return a <see cref="State"/> value when ticked
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// Executes this node and returns a <see cref="State"/> value
        /// </summary>
        public abstract State Tick(IBlackboard blackboard);

        /// <summary>
        /// Called when this node needs to be interrupted before it can return <see cref="State.Failure"/> or <see cref="State.Success"/>.
        /// </summary>
        public virtual void Interrupt(IBlackboard blackboard) { }

        /// <summary>
        /// Called before the first time this node is ticked
        /// </summary>
        public virtual void OnBeforeRun(IBlackboard blackboard) { }

        /// <summary>
        /// Called after the last time this node is ticked and returns <see cref="State.Failure"/> or <see cref="State.Success"/>
        /// </summary>
        public virtual void OnAfterRun(IBlackboard blackboard) { }

        internal virtual string BuildStringTree(string prefix = "", bool isLast = true)
            => $"{prefix}{(isLast ? "└── " : "├── ")}{GetType().Name}\n";
    }
}
