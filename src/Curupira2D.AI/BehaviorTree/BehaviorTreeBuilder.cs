using Curupira2D.AI.BehaviorTree.Composites;
using Curupira2D.AI.BehaviorTree.Decorators;
using Curupira2D.AI.BehaviorTree.Leafs;

namespace Curupira2D.AI.BehaviorTree
{
    /// <summary>
    /// Helper for building a <see cref="BehaviorTree"/> using a fluent API
    /// </summary>
    public class BehaviorTreeBuilder()
    {
        Node _currentNode = null!;
        readonly Stack<Node> _parentNodeStack = [];

        public static BehaviorTreeBuilder GetInstance() => new();

        #region Leafs
        public BehaviorTreeBuilder Leaf<T>(params object[] args) where T : Node
        {
            var leaf = (T)Activator.CreateInstance(typeof(T), args);

            if (leaf is not ActionLeaf && leaf is not ConditionLeaf)
                throw new InvalidOperationException("Leaf nodes must be ActionLeaf or ConditionLeaf");

            return AddChildOnParent(leaf);
        }

        public BehaviorTreeBuilder Leaf<T>() where T : Node => Leaf<T>(null!);

        public BehaviorTreeBuilder ExecuteAction(Func<IBlackboard, NodeState> action) => Leaf<ExecuteAction>([action]);

        public BehaviorTreeBuilder ExecuteAction(Func<IBlackboard, bool> action) => ExecuteAction(bb => action(bb) ? NodeState.Success : NodeState.Failure);

        public BehaviorTreeBuilder DebugLogAction(string text) => Leaf<DebugLogAction>([text]);

        public BehaviorTreeBuilder Conditional(Func<IBlackboard, NodeState> action) => PushParent(new ExecuteActionConditional(action));

        public BehaviorTreeBuilder Conditional(Func<IBlackboard, bool> action) => Conditional(t => action(t) ? NodeState.Success : NodeState.Failure);
        #endregion

        #region Decorators
        public BehaviorTreeBuilder AlwaysFailure() => PushParent(new AlwaysFail());

        public BehaviorTreeBuilder AlwaysSuccess() => PushParent(new AlwaysSuccess());

        public BehaviorTreeBuilder ConditionalDecorator(Func<IBlackboard, NodeState> action, bool shouldReevaluate = true)
            => PushParent(new ConditionalDecorator(new ExecuteActionConditional(action), shouldReevaluate));

        public BehaviorTreeBuilder ConditionalDecorator(Func<IBlackboard, bool> func, bool shouldReevaluate = true)
            => ConditionalDecorator(t => func(t) ? NodeState.Success : NodeState.Failure, shouldReevaluate);

        public BehaviorTreeBuilder Delay(int milliseconds) => PushParent(new Delay(milliseconds));

        public BehaviorTreeBuilder Inverter() => PushParent(new Inverter());

        public BehaviorTreeBuilder Repeater(int repeatCount, bool repeatForever = false, bool endOnFailure = false)
            => PushParent(new Repeater(repeatCount, repeatForever, endOnFailure));

        public BehaviorTreeBuilder UntilFail() => PushParent(new UntilFail());

        public BehaviorTreeBuilder UntilSuccess() => PushParent(new UntilSuccess());
        #endregion

        #region Composites
        public BehaviorTreeBuilder ParallelSelector() => PushParent(new ParallelSelector());

        public BehaviorTreeBuilder ParallelSequence() => PushParent(new ParallelSequence());

        public BehaviorTreeBuilder RandomSelector() => PushParent(new RandomSelector());

        public BehaviorTreeBuilder RandomSequence() => PushParent(new RandomSequence());

        public BehaviorTreeBuilder Selector(AbortTypes abortType = AbortTypes.None) => PushParent(new Selector(abortType));

        public BehaviorTreeBuilder Sequence(AbortTypes abortType = AbortTypes.None) => PushParent(new Sequence(abortType));

        /// <summary>
        /// Closes a composite node. This is necessary to close the composite and return to the parent node
        /// </summary>
        /// <returns></returns>
        public BehaviorTreeBuilder Close()
        {
            if (_parentNodeStack.Peek() is not Composite)
                throw new InvalidOperationException("Trying to close a node, but the top node is a decorator");

            _currentNode = _parentNodeStack.Pop();
            return this;
        }
        #endregion

        public BehaviorTree Build(IBlackboard blackboard, int updateIntervalInMilliseconds = 1000)
        {
            if (_currentNode is null)
                throw new InvalidOperationException("Can't create a behaviour tree with zero nodes");

            return new BehaviorTree(blackboard, _currentNode, updateIntervalInMilliseconds);
        }

        BehaviorTreeBuilder AddChildOnParent(Node child)
        {
            ArgumentNullException.ThrowIfNull(child, nameof(child));

            if (_parentNodeStack.Count == 0 && child is ILeaf)
                throw new InvalidOperationException("Can't create a leaf node without a parent");

            var parent = _parentNodeStack.Peek();

            if (parent is Composite composite)
                composite.AddChild(child);

            // Decorators have just one child and must be close automatically
            if (parent is Decorator decorator)
            {
                decorator.Child = child;
                _currentNode = _parentNodeStack.Pop(); // close the decorator
            }

            return this;
        }

        /// <summary>
        /// Pushes a <see cref="Composite"/> or <see cref="Decorator"/> on the stack
        /// </summary>
        BehaviorTreeBuilder PushParent(Node node)
        {
            if (_parentNodeStack.Count > 0)
                AddChildOnParent(node);

            _parentNodeStack.Push(node);
            return this;
        }
    }
}