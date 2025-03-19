namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// decorator that will only run its child if a condition is met. By default, the condition will be reevaluated every tick.
    /// </summary>
    public class ConditionalDecorator : Decorator, IConditional
    {
        private readonly IConditional _conditional;
        private readonly bool _shouldReevalute;
        NodeState _conditionalStatus;

        public ConditionalDecorator(Node child, IConditional conditional, bool shouldReevalute) : base(child)
        {
            ArgumentNullException.ThrowIfNull(conditional, nameof(conditional));

            _conditional = conditional;
            _shouldReevalute = shouldReevalute;
        }

        public ConditionalDecorator(Node child, IConditional conditional) : this(child, conditional, true) { }

        internal ConditionalDecorator(IConditional conditional, bool shouldReevalute) : this(null!, conditional, shouldReevalute) { }

        public override void Invalidate(IBlackboard blackboard)
        {
            base.Invalidate(blackboard);
            _conditionalStatus = NodeState.Invalid;
        }

        public override void OnStart(IBlackboard blackboard) => _conditionalStatus = NodeState.Invalid;

        public override NodeState Update(IBlackboard blackboard)
        {
            ArgumentNullException.ThrowIfNull(Child, nameof(Child));

            // evalute the condition if we need to
            _conditionalStatus = ExecuteConditional(blackboard);

            if (_conditionalStatus == NodeState.Success)
                return Child.Tick(blackboard);

            return NodeState.Failure;
        }

        /// <summary>
        /// Executes the conditional either following the shouldReevaluate flag or with an option to force an update. Aborts will force the
        /// update to make sure they get the proper data if a Conditional changes.
        /// </summary>
        internal NodeState ExecuteConditional(IBlackboard blackboard, bool forceUpdate = false)
        {
            if (forceUpdate || _shouldReevalute || _conditionalStatus == NodeState.Invalid)
                _conditionalStatus = _conditional.Update(blackboard);

            return _conditionalStatus;
        }
    }
}