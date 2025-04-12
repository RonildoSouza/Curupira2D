namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>
    /// Composite nodes manage child nodes and determine how they are executed.
    /// </summary>
    public abstract class Composite : Behavior
    {
        protected int CurrentChildIndex { get; set; }
        protected internal IList<Behavior> Children { get; set; } = [];

        /// <summary>
		/// Adds a child to this <see cref="Composite"/>.
		/// </summary>
        public Composite AddChild(Behavior child)
        {
            Children.Add(child);
            return this;
        }

        public override void OnInitialize(IBlackboard blackboard)
        {
            base.OnInitialize(blackboard);
            Reset();
        }

        public override void OnTerminate(IBlackboard blackboard)
        {
            base.OnTerminate(blackboard);
            Reset();

            for (int i = 0; i < Children.Count; i++)
                Children[i].OnTerminate(blackboard);
        }

        /// <summary>
        /// Reset child for next tick
        /// </summary>
        protected internal virtual void Reset() => CurrentChildIndex = 0;
    }
}
