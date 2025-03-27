namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>
	/// <see cref="ParallelSelector"/> nodes will return <see cref="BehaviorState.Success"/> once any of its children have returned <see cref="BehaviorState.Success"/>.
    /// If all children returns <see cref="BehaviorState.Failure"/> the <see cref="ParallelSelector"/> node will end all and return <see cref="BehaviorState.Failure"/>.
	/// </summary>
    public class ParallelSelector : Composite
    {
        public override BehaviorState Update(IBlackboard blackboard)
        {
            var didAllFail = true;

            foreach (var child in Children)
            {
                child.Tick(blackboard);

                // if any child succeeds we return success
                if (child.State == BehaviorState.Success)
                    return State = BehaviorState.Success;

                // if all children didn't fail, we're not done yet
                if (child.State != BehaviorState.Failure)
                    didAllFail = false;
            }

            if (didAllFail)
                return State = BehaviorState.Failure;

            return State = BehaviorState.Running;
        }
    }
}
