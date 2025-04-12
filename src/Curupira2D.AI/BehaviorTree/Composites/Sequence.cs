namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>    
    /// <see cref="Sequence"/> nodes will attempt to execute all of its children and report <see cref="BehaviorState.Success"/> in case all of the children report a <see cref="BehaviorState.Success"/>.
    /// If at least one child reports a <see cref="BehaviorState.Failure"/>, this node will also return <see cref="BehaviorState.Failure"/> and restart.
    /// In case a child returns <see cref="BehaviorState.Running"/> this node will tick again.
    /// </summary>
    public class Sequence : Composite
    {
        public override BehaviorState Update(IBlackboard blackboard)
        {
            var child = Children[CurrentChildIndex];
            State = child.Update(blackboard);

            if (State != BehaviorState.Success)
                return State;

            if (++CurrentChildIndex == Children.Count)
            {
                Reset();
                return BehaviorState.Success;
            }

            return BehaviorState.Running;
        }
    }
}
