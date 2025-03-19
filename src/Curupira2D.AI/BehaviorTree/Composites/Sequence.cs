namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>    
    /// <see cref="Sequence"/> nodes will attempt to execute all of its children and report <see cref="NodeState.Success"/> in case all of the children report a <see cref="NodeState.Success"/>.
    /// If at least one child reports a <see cref="NodeState.Failure"/>, this node will also return <see cref="NodeState.Failure"/> and restart.
    /// In case a child returns <see cref="NodeState.Running"/> this node will tick again.
    /// </summary>
    public class Sequence : Composite
    {
        public Sequence(AbortTypes abortType = AbortTypes.None) => AbortType = abortType;

        public override NodeState Update(IBlackboard blackboard)
        {
            // first, we handle conditional aborts if we are not already on the first child
            if (CurrentChildIndex != 0)
                HandleConditionalAborts(blackboard);

            var child = Children[CurrentChildIndex];
            var childState = child.Tick(blackboard);

            // if the child failed or is still running, early return
            if (childState != NodeState.Success)
                return childState;

            CurrentChildIndex++;

            // if the end of the children is hit the whole sequence suceeded
            if (CurrentChildIndex == Children.Count)
            {
                // reset index for next run
                CurrentChildIndex = 0;
                return NodeState.Success;
            }

            return NodeState.Running;
        }

        void HandleConditionalAborts(IBlackboard blackboard)
        {
            if (HasLowerPriorityConditionalAbort)
                UpdateLowerPriorityAbortConditional(blackboard, NodeState.Success);

            if (AbortType.Has(AbortTypes.Self))
                UpdateSelfAbortConditional(blackboard, NodeState.Success);
        }
    }
}
