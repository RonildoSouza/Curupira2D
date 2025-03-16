namespace Curupira2D.AI.BehaviorTree.Leafs
{
    public class RandomProbabilityCondition : ConditionLeaf
    {
        private readonly int _probability;

        /// <summary>
        /// Returns <see cref="State.Success"/> when the random probability is less than or equal <paramref name="probability"/>, otherwise return <see cref="State.Failure"/>.
        /// <paramref name="probability"/> must be between 0 and 100.
        /// </summary>
        /// <param name="probability">Probability to <see cref="State.Success"/></param>
        public RandomProbabilityCondition(int probability)
        {
            if (probability < 0 || probability > 100)
                throw new ArgumentException("Probability must be between 0 and 100");

            _probability = probability;
        }

        public override State Tick(IBlackboard blackboard) => Random.Shared.Next(0, 100) <= _probability ? State.Success : State.Failure;
    }
}
