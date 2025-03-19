namespace Curupira2D.AI.BehaviorTree.Leafs
{
    /// <summary>
    /// Returns <see cref="NodeState.Success"/> when the random probability is less than or equal <paramref name="probability"/>, otherwise return <see cref="NodeState.Failure"/>.
    /// <paramref name="probability"/> must be between 0 and 100.
    /// </summary>
    public class RandomProbabilityCondition : ConditionLeaf
    {
        private readonly int _probability;

        public RandomProbabilityCondition(int probability)
        {
            if (probability < 0 || probability > 100)
                throw new ArgumentException("Probability must be between 0 and 100");

            _probability = probability;
        }

        public override NodeState Update(IBlackboard blackboard) => Random.Shared.Next(0, 100) <= _probability ? NodeState.Success : NodeState.Failure;
    }
}
