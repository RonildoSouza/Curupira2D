using Curupira2D.AI.Extensions;

namespace Curupira2D.AI.BehaviorTree
{
    /// <summary>
    /// Represents a node in a behavior tree. 
    /// Every node must return a <see cref="BehaviorState"/> value when ticked
    /// </summary>
    public abstract class Behavior
    {
        public BehaviorState State { get; set; } = BehaviorState.Invalid;

        /// <summary>
		/// Called immediately before execution. 
        /// It is used to setup any variables that need to be reset from the previous run.
		/// </summary>
        public virtual void OnInitialize(IBlackboard blackboard) { }

        /// <summary>
        /// Executes this node and returns a <see cref="BehaviorState"/> value
        /// </summary>
        public abstract BehaviorState Update(IBlackboard blackboard);

        /// <summary>
		/// Called when a task changes state to something other than <see cref="BehaviorState.Running"/>
		/// </summary>
        public virtual void OnTerminate(IBlackboard blackboard) => State = BehaviorState.Invalid;

        /// <summary>
		/// Tick handles calling through to update where the actual work is done. 
        /// It exists so that it can call <see cref="OnInitialize(IBlackboard)"/>/<see cref="OnTerminate(IBlackboard)"/> when necessary.
		/// </summary>
		internal BehaviorState Tick(IBlackboard blackboard)
        {
            if (State == BehaviorState.Invalid)
                OnInitialize(blackboard);

            State = Update(blackboard);

#if DEBUG
            blackboard.Set("BehaviorTreeStructureWithState", this.GetBehaviorTreeStructureWithState());
#endif

            if (State != BehaviorState.Running)
                OnTerminate(blackboard);

            return State;
        }
    }
}
