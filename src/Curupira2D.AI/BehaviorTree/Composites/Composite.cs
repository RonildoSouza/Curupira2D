using Curupira2D.AI.BehaviorTree.Decorators;

namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>
    /// Composite nodes manage child nodes and determine how they are executed.
    /// </summary>
    public abstract class Composite : Node
    {
        protected internal IList<Node> Children { get; set; } = [];
        protected bool HasLowerPriorityConditionalAbort { get; set; }
        protected int CurrentChildIndex { get; set; } = 0;

        public AbortTypes AbortType { get; internal set; } = AbortTypes.None;

        public override void Invalidate(IBlackboard blackboard)
        {
            base.Invalidate(blackboard);

            for (var i = 0; i < Children.Count; i++)
                Children[i].Invalidate(blackboard);
        }

        public override void OnStart(IBlackboard blackboard)
        {
            // LowerPriority aborts happen one level down so we check for any here
            HasLowerPriorityConditionalAbort = HasLowerPriorityConditionalAbortInChildren();
            CurrentChildIndex = 0;
        }

        public override void OnEnd(IBlackboard blackboard)
        {
            // we are done so invalidate our children so they are ready for the next tick
            for (var i = 0; i < Children.Count; i++)
                Children[i].Invalidate(blackboard);
        }

        /// <summary>
		/// Adds a child to this <see cref="Composite"/>.
		/// </summary>
        public void AddChild(Node child) => Children.Add(child);

        /// <summary>
		/// Checks if first child of a <see cref="Composite"/> is a <see cref="IConditional"/>. 
        /// Usef for dealing with conditional aborts.
		/// </summary>
		public bool IsFirstChildConditional() => Children[0] is IConditional;

        /// <summary>
		/// Checks the children of the <see cref="Composite"/> to see if any are a <see cref="Composite"/> with a <see cref="AbortTypes.LowerPriority"/> set.
		/// </summary>
		bool HasLowerPriorityConditionalAbortInChildren() =>
            Children.OfType<Composite>().Any(composite => composite.AbortType.Has(AbortTypes.LowerPriority) && composite.IsFirstChildConditional());

        /// <summary>
		/// Checks any child Composites that have a <see cref="AbortTypes.LowerPriority"/> set and a <see cref="IConditional"/> as the first child. 
        /// If it finds one it will tick the <see cref="IConditional"/> and if the status is not equal to <paramref name="stateCheck"/> the <c>_currentChildIndex</c> will be updated, ie the currently running
		/// Action will be aborted.
		/// </summary>
		protected void UpdateLowerPriorityAbortConditional(IBlackboard blackboard, NodeState stateCheck)
        {
            // check any lower priority tasks to see if they changed status
            for (var i = 0; i < CurrentChildIndex; i++)
            {
                if (Children[i] is Composite composite && composite.AbortType.Has(AbortTypes.LowerPriority))
                {
                    // now we get the status of only the Conditional (update instead of tick) to see if it changed taking care with ConditionalDecorators
                    var child = composite.Children[0];
                    var state = UpdateConditionalNode(blackboard, child);

                    if (state != stateCheck)
                    {
                        CurrentChildIndex = i;

                        // we have an abort so we invalidate the children so they get reevaluated
                        for (var j = i; j < Children.Count; j++)
                            Children[j].Invalidate(blackboard);

                        break;
                    }
                }
            }
        }

        /// <summary>
		/// Checks any <see cref="IConditional"/> children to see if they have changed state
		/// </summary>
		protected void UpdateSelfAbortConditional(IBlackboard blackboard, NodeState stateCheck)
        {
            // check any IConditional child tasks to see if they changed status
            for (var i = 0; i < CurrentChildIndex; i++)
            {
                var child = Children[i];

                if (child is not IConditional)
                    continue;

                var status = UpdateConditionalNode(blackboard, child);

                if (status != stateCheck)
                {
                    CurrentChildIndex = i;

                    // we have an abort so we invalidate the children so they get reevaluated
                    for (var j = i; j < Children.Count; j++)
                        Children[j].Invalidate(blackboard);

                    break;
                }
            }
        }

        /// <summary>
		/// Helper that gets the NodeState of either a Conditional or a ConditionalDecorator
		/// </summary>
		static NodeState UpdateConditionalNode(IBlackboard blackboard, Node node)
        {
            if (node is ConditionalDecorator conditionalDecorator)
                return conditionalDecorator.ExecuteConditional(blackboard, true);

            return node.Update(blackboard);
        }
    }
}
