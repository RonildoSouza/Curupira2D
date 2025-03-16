namespace Curupira2D.AI.BehaviorTree.Composites
{
    /// <summary>
    /// Composite nodes manage child nodes and determine how they are executed.
    /// </summary>
    public abstract class Composite : Node
    {
        protected Node _runningChild = null!;
        protected IList<Node> _children = [];

        public Composite AddChild(Node child)
        {
            _children.Add(child);
            return this;
        }

        public override void Interrupt(IBlackboard blackboard)
        {
            if (_runningChild != null)
            {
                _runningChild.Interrupt(blackboard);
                _runningChild = null!;
            }

            base.Interrupt(blackboard);
        }

        public override void OnAfterRun(IBlackboard blackboard) => _runningChild = null!;
    }
}
