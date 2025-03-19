namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>
	/// <see cref="ParallelSequence"/> nodes will return <see cref="NodeState.Success"/> once all of its children have returned <see cref="NodeState.Success"/>.
    /// If one children returns <see cref="NodeState.Failure"/> the <see cref="ParallelSequence"/> node will end all and return <see cref="NodeState.Failure"/>.
	/// </summary>
    public class ParallelSequence : Composite
    {
        public override NodeState Update(IBlackboard blackboard)
        {
            var didAllSucceed = true;

            foreach (var child in Children)
            {
                child.Tick(blackboard);

                // if any child fails the whole branch fails
                if (child.State == NodeState.Failure)
                    return NodeState.Failure;

                // if all children didn't succeed, we're not done yet
                else if (child.State != NodeState.Success)
                    didAllSucceed = false;
            }

            if (didAllSucceed)
                return NodeState.Success;

            return NodeState.Running;
        }
    }
}
