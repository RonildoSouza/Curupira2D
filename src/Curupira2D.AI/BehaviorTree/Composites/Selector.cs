namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>
    /// <see cref="Selector"/> nodes will attempt to execute each of its children until one of them return <see cref="NodeState.Success"/>. 
    /// If all children return <see cref="NodeState.Failure"/>, this node will also return <see cref="NodeState.Failure"/>.
    /// If a child returns <see cref="NodeState.Running"/> it will tick again.
    /// </summary>
    public class Selector : Composite
    {
        public Selector(AbortTypes abortType = AbortTypes.None) => AbortType = abortType;

        public override NodeState Update(IBlackboard blackboard)
        {
            // first, we handle conditinoal aborts if we are not already on the first child
            if (CurrentChildIndex != 0)
                HandleConditionalAborts(blackboard);

            var child = Children[CurrentChildIndex];
            var childState = child.Tick(blackboard);

            // if the child succeeds or is still running, early return.
            if (childState != NodeState.Failure)
                return childState;

            CurrentChildIndex++;

            // if the end of the children is hit, that means the whole thing fails.
            if (CurrentChildIndex == Children.Count)
            {
                // reset index otherwise it will crash on next run through
                CurrentChildIndex = 0;
                return NodeState.Failure;
            }

            return NodeState.Running;
        }

        void HandleConditionalAborts(IBlackboard blackboard)
        {
            if (HasLowerPriorityConditionalAbort)
                UpdateLowerPriorityAbortConditional(blackboard, NodeState.Failure);

            if (AbortType.Has(AbortTypes.Self))
                UpdateSelfAbortConditional(blackboard, NodeState.Failure);
        }
    }
}
