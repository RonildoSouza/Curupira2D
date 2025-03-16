namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>
	/// <see cref="ParallelSelector"/> nodes will return <see cref="State.Success"/> once any of its children have returned <see cref="State.Success"/>.
    /// If all children returns <see cref="State.Failure"/> the <see cref="ParallelSelector"/> node will end all and return <see cref="State.Failure"/>.
	/// </summary>
    public class ParallelSelector : Composite
    {
        public ParallelSelector() { }
        public ParallelSelector(IList<Node> children) => _children = children;

        public override State Tick(IBlackboard blackboard)
        {
            var didAllFail = true;

            foreach (var child in _children)
            {
                if (child != _runningChild)
                    child.OnBeforeRun(blackboard);

                var childState = child.Tick(blackboard);

                // if any child succeeds we return success
                if (childState == State.Success)
                {
                    child.OnAfterRun(blackboard);
                    return State.Success;
                }

                // if all children didn't fail, we're not done yet
                if (childState != State.Failure)
                    didAllFail = false;
            }

            if (didAllFail)
            {
                foreach (var child in _children)
                    child.OnAfterRun(blackboard);

                return State.Failure;
            }

            return State.Running;
        }
    }
}
