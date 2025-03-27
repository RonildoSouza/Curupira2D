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
        Behavior _currentNode = null!;
        readonly Stack<Behavior> _parentNodeStack = [];

        public static BehaviorTreeBuilder GetInstance() => new();

        #region Leafs
        public BehaviorTreeBuilder Leaf<T>(params object[] args) where T : Behavior
        {
            var leaf = (T)Activator.CreateInstance(typeof(T), args);

            if (leaf is not Leafs.Leaf)
                throw new InvalidOperationException("Leaf nodes must inherit Leaf class!");

            return AddChildOnParent(leaf);
        }

        public BehaviorTreeBuilder Leaf<T>() where T : Behavior => Leaf<T>(null!);

        /// <summary>
        /// Define a task to be performed simple actions without the need for subclass.
        /// </summary>
        public BehaviorTreeBuilder ExecuteAction(Func<IBlackboard, BehaviorState> action) => Leaf<ExecuteAction>([action]);

        /// <summary>
        ///  Conditions are leaf nodes that either return <see cref="BehaviorState.Success"/> or <see cref="BehaviorState.Failure"/> depending on a single simple condition
        /// </summary>
        public BehaviorTreeBuilder Conditional(Func<IBlackboard, bool> action) => ExecuteAction(bb => action(bb) ? BehaviorState.Success : BehaviorState.Failure);

        /// <summary>
        /// Output the specified text and return <see cref="BehaviorState.Success"/>
        /// </summary>
        public BehaviorTreeBuilder DebugLogAction(string text) => Leaf<DebugLogAction>([text]);

        /// <summary>
        /// Returns <see cref="BehaviorState.Success"/> when the random probability is less than or equal <paramref name="probability"/>, otherwise return <see cref="BehaviorState.Failure"/>.
        /// <paramref name="probability"/> must be between 0 and 100.
        /// </summary>
        public BehaviorTreeBuilder RandomProbabilityCondition(int probability = 50) => Leaf<RandomProbabilityCondition>([probability]);
        #endregion

        #region Decorators
        public BehaviorTreeBuilder AlwaysFailure() => PushParent(new AlwaysFail());

        public BehaviorTreeBuilder AlwaysSuccess() => PushParent(new AlwaysSuccess());

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

        public BehaviorTreeBuilder Selector() => PushParent(new Selector());

        public BehaviorTreeBuilder Sequence() => PushParent(new Sequence());

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

        public BehaviorTree Build(IBlackboard blackboard, int updateIntervalInMilliseconds = 20)
        {
            if (_currentNode is null)
                throw new InvalidOperationException("Can't create a behaviour tree with zero nodes");

            return new BehaviorTree(blackboard, _currentNode, updateIntervalInMilliseconds);
        }

        BehaviorTreeBuilder AddChildOnParent(Behavior child)
        {
            ArgumentNullException.ThrowIfNull(child, nameof(child));

            if (_parentNodeStack.Count == 0 && child is Leaf)
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
        BehaviorTreeBuilder PushParent(Behavior node)
        {
            if (_parentNodeStack.Count > 0)
                AddChildOnParent(node);

            _parentNodeStack.Push(node);
            return this;
        }
    }
}