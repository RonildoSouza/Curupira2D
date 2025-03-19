namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>
	/// Same as <see cref="Selector"/> except it shuffles the children when started
	/// </summary>
    public class RandomSelector : Selector
    {
        public override void OnStart(IBlackboard blackboard)
        {
            var children = Children.ToArray();
            Random.Shared.Shuffle(children);
            Children = children;
        }
    }
}
