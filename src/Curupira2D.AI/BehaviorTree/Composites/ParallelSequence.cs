namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>
	/// <see cref="ParallelSequence"/> nodes will return <see cref="BehaviorState.Success"/> once all of its children have returned <see cref="BehaviorState.Success"/>.
    /// If one children returns <see cref="BehaviorState.Failure"/> the <see cref="ParallelSequence"/> node will end all and return <see cref="BehaviorState.Failure"/>.
	/// </summary>
    public class ParallelSequence : Composite
    {
        public override BehaviorState Update(IBlackboard blackboard)
        {
            var didAllSucceed = true;

            foreach (var child in Children)
            {
                State = child.Tick(blackboard);

                // if any child fails the whole branch fails
                if (child.State == BehaviorState.Failure)
                    return BehaviorState.Failure;

                // if all children didn't succeed, we're not done yet
                else if (child.State != BehaviorState.Success)
                    didAllSucceed = false;
            }

            if (didAllSucceed)
                return State = BehaviorState.Success;

            return State = BehaviorState.Running;
        }
    }
}
