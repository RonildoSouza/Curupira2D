namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>
	/// <see cref="ParallelSequence"/> nodes will return <see cref="State.Success"/> once all of its children have returned <see cref="State.Success"/>.
    /// If one children returns <see cref="State.Failure"/> the <see cref="ParallelSequence"/> node will end all and return <see cref="State.Failure"/>.
	/// </summary>
    public class ParallelSequence : Composite
    {
        public override State Tick(IBlackboard blackboard)
        {
            var didAllSucceed = true;

            foreach (var child in Children)
            {
                if (child != RunningChild)
                    child.OnBeforeRun(blackboard);

                var childState = child.Tick(blackboard);

                // if any child fails the whole branch fails
                if (childState == State.Failure)
                {
                    // Interrupt any child that was RUNNING before
                    Interrupt(blackboard);
                    child.OnAfterRun(blackboard);

                    return State.Failure;
                }

                // if all children didn't succeed, we're not done yet
                else if (childState != State.Success)
                    didAllSucceed = false;
            }

            if (didAllSucceed)
            {
                foreach (var child in Children)
                    child.OnAfterRun(blackboard);

                return State.Success;
            }

            return State.Running;
        }
    }
}
