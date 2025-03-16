namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>
	/// Same as <see cref="Selector"/> except it shuffles the children when started
	/// </summary>
    public class RandomSelector : Selector
    {
        public override void OnBeforeRun(IBlackboard blackboard) => Random.Shared.Shuffle(_children.ToArray());
    }
}
