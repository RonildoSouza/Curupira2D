namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>
    /// Same as <see cref="Sequence"/> except it shuffles the children when started
    /// </summary>
    public class RandomSequence : Sequence
    {
        public override void OnStart(IBlackboard blackboard)
        {
            var children = Children.ToArray();
            Random.Shared.Shuffle(children);
            Children = children;
        }
    }
}
