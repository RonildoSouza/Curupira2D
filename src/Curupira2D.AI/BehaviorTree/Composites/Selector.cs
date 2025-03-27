namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>
    /// <see cref="Selector"/> nodes will attempt to execute each of its children until one of them return <see cref="BehaviorState.Success"/>. 
    /// If all children return <see cref="BehaviorState.Failure"/>, this node will also return <see cref="BehaviorState.Failure"/>.
    /// If a child returns <see cref="BehaviorState.Running"/> it will tick again.
    /// </summary>
    public class Selector : Composite
    {
        public override BehaviorState Update(IBlackboard blackboard)
        {
            var child = Children[CurrentChildIndex];
            State = child.Update(blackboard);

            if (State != BehaviorState.Failure)
                return State;

            if (++CurrentChildIndex == Children.Count)
            {
                Reset();
                return BehaviorState.Failure;
            }

            return BehaviorState.Running;
        }
    }
}
