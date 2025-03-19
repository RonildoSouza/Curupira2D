namespace Curupira2D.AI.BehaviorTree
{
    /// <summary>
    /// Represents a node in a behavior tree. 
    /// Every node must return a <see cref="NodeState"/> value when ticked
    /// </summary>
    public abstract class Node
    {
        public NodeState State { get; internal set; } = NodeState.Invalid;

        /// <summary>
        /// Executes this node and returns a <see cref="NodeState"/> value
        /// </summary>
        public abstract NodeState Update(IBlackboard blackboard);

        /// <summary>
		/// Invalidate the status of the node. 
        /// Composites can override this and invalidate all of their children.
		/// </summary>
        public virtual void Invalidate(IBlackboard blackboard) => State = NodeState.Invalid;

        /// <summary>
		/// Called immediately before execution. 
        /// It is used to setup any variables that need to be reset from the previous run.
		/// </summary>
        public virtual void OnStart(IBlackboard blackboard) { }

        /// <summary>
		/// Called when a task changes state to something other than <see cref="NodeState.Running"/>
		/// </summary>
        public virtual void OnEnd(IBlackboard blackboard) { }

        /// <summary>
		/// Tick handles calling through to update where the actual work is done. 
        /// It exists so that it can call <see cref="OnStart(IBlackboard)"/>/<see cref="OnEnd(IBlackboard)"/> when necessary.
		/// </summary>
		internal NodeState Tick(IBlackboard blackboard)
        {
            if (State == NodeState.Invalid)
                OnStart(blackboard);

            State = Update(blackboard);

            if (State != NodeState.Running)
                OnEnd(blackboard);

            return State;
        }
    }
}
