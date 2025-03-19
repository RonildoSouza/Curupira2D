namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>
	/// <see cref="ParallelSelector"/> nodes will return <see cref="NodeState.Success"/> once any of its children have returned <see cref="NodeState.Success"/>.
    /// If all children returns <see cref="NodeState.Failure"/> the <see cref="ParallelSelector"/> node will end all and return <see cref="NodeState.Failure"/>.
	/// </summary>
    public class ParallelSelector : Composite
    {
        public override NodeState Update(IBlackboard blackboard)
        {
            var didAllFail = true;

            foreach (var child in Children)
            {
                child.Tick(blackboard);

                // if any child succeeds we return success
                if (child.State == NodeState.Success)
                    return NodeState.Success;

                // if all children didn't fail, we're not done yet
                if (child.State != NodeState.Failure)
                    didAllFail = false;
            }

            if (didAllFail)
                return NodeState.Failure;

            return NodeState.Running;
        }
    }
}
