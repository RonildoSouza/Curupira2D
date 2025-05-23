﻿namespace Curupira2D.AI.BehaviorTree
{
    public enum BehaviorState
    {
        /// <summary>
        /// Indicates that the node has not been ticked yet
        /// </summary>
        Invalid,

        /// <summary>
        /// Indicates that the node has completed successfully
        /// </summary>
        Success,

        /// <summary>
        /// Indicates that the node has failed
        /// </summary>
        Failure,

        /// <summary>
        /// Indicates that the node is running
        /// </summary>
        Running
    }

    public abstract partial class Behavior
    {
        public BehaviorState Invalid() => State = BehaviorState.Invalid;
        public BehaviorState Success() => State = BehaviorState.Success;
        public BehaviorState Failure() => State = BehaviorState.Failure;
        public BehaviorState Running() => State = BehaviorState.Running;
    }
}
