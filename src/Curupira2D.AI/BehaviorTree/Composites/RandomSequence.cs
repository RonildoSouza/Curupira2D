namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>
    /// Same as <see cref="Sequence"/> except that it shuffles the children when initialized
    /// </summary>
    public class RandomSequence : Composite
    {
        bool _initialized;
        Behavior[] _children = [];

        public override void OnInitialize(IBlackboard blackboard)
        {
            base.OnInitialize(blackboard);

            _children = [.. Children];
            Random.Shared.Shuffle(_children);

            _initialized = true;
        }

        public override BehaviorState Update(IBlackboard blackboard)
        {
            if (!_initialized)
                OnInitialize(blackboard);

            var child = _children[CurrentChildIndex];
            State = child.Update(blackboard);

            if (State != BehaviorState.Success)
                return State;

            if (++CurrentChildIndex == _children.Length)
            {
                Reset();
                return BehaviorState.Success;
            }

            return State = BehaviorState.Running;
        }

        protected internal override void Reset()
        {
            _initialized = false;
            _children = [];
            base.Reset();
        }
    }
}
