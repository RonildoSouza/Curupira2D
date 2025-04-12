namespace Curupira2D.AI.BehaviorTree.Leafs
{
    /// <summary>
    /// Returns <see cref="BehaviorState.Success"/> when the random probability is less than or equal <paramref name="probability"/>, otherwise return <see cref="BehaviorState.Failure"/>.
    /// <paramref name="probability"/> must be between 0 and 100.
    /// </summary>
    public class RandomProbabilityCondition : Leaf
    {
        private readonly int _probability;

        public RandomProbabilityCondition(int probability)
        {
            if (probability < 0 || probability > 100)
                throw new ArgumentException("Probability must be between 0 and 100");

            _probability = probability;
        }

        public override BehaviorState Update(IBlackboard blackboard)
        {
            State = Random.Shared.Next(0, 100) <= _probability ? BehaviorState.Success : BehaviorState.Failure;
            return State;
        }
    }
}
