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
        /// <summary>
        /// Always return <see cref="BehaviorState.Failure"/> except when the child node is <see cref="BehaviorState.Running"/>
        /// </summary>
        public BehaviorTreeBuilder AlwaysFailure() => PushParent(new AlwaysFailure());

        /// <summary>
        /// Always return <see cref="BehaviorState.Success"/> except when the child node is <see cref="BehaviorState.Running"/>
        /// </summary>
        public BehaviorTreeBuilder AlwaysSuccess() => PushParent(new AlwaysSuccess());

        /// <summary>
        /// Return <see cref="BehaviorState.Running"/> for a set amount of time before executing its child.
        /// The timer resets when both it and its child are not <see cref="BehaviorState.Running"/>
        /// </summary>
        public BehaviorTreeBuilder Delay(int milliseconds) => PushParent(new Delay(milliseconds));

        /// <summary>
        /// Return <see cref="BehaviorState.Running"/> for a set amount of time before executing its child.
        /// The timer resets when both it and its child are not <see cref="BehaviorState.Running"/>
        /// </summary>
        public BehaviorTreeBuilder Delay(TimeSpan timeSpan) => Delay(timeSpan.Milliseconds);

        /// <summary>
        /// Return <see cref="BehaviorState.Success"/> if its child returns <see cref="BehaviorState.Failure"/> and vice versa.
        /// </summary>
        public BehaviorTreeBuilder Inverter() => PushParent(new Inverter());

        /// <summary>
        /// Repeat execution of its child task until the child task has been run a specified number of times. 
        /// It has the option of continuing to execute the child task even if the child task returns a <see cref="BehaviorState.Failure"/>. 
        /// </summary>
        /// <param name="repeatCount">The number of times to repeat the execution of its child</param>
        /// <param name="repeatForever">Allows the repeater to repeat forever</param>
        /// <param name="endOnFailure">Should the task return if the child task returns a <see cref="BehaviorState.Failure"/></param>
        public BehaviorTreeBuilder Repeater(int repeatCount, bool repeatForever = false, bool endOnFailure = false)
            => PushParent(new Repeater(repeatCount, repeatForever, endOnFailure));

        /// <summary>
        /// Keep executing its child task until the child node returns <see cref="BehaviorState.Failure"/>
        /// </summary>
        public BehaviorTreeBuilder UntilFailure() => PushParent(new UntilFailure());

        /// <summary>
        /// Keep executing its child task until the child node returns <see cref="BehaviorState.Success"/>
        /// </summary>
        public BehaviorTreeBuilder UntilSuccess() => PushParent(new UntilSuccess());
        #endregion

        #region Composites
        /// <summary>
        /// <Return <see cref="BehaviorState.Success"/> once any of its children have returned <see cref="BehaviorState.Success"/>.
        /// If all children returns <see cref="BehaviorState.Failure"/> the <see cref="Composites.ParallelSelector"/> node will end all and return <see cref="BehaviorState.Failure"/>.
        /// </summary>
        public BehaviorTreeBuilder ParallelSelector() => PushParent(new ParallelSelector());

        /// <summary>
        /// Return <see cref="BehaviorState.Success"/> once all of its children have returned <see cref="BehaviorState.Success"/>.
        /// If one children returns <see cref="BehaviorState.Failure"/> the <see cref="Composites.ParallelSequence"/> node will end all and return <see cref="BehaviorState.Failure"/>.
        /// </summary>
        public BehaviorTreeBuilder ParallelSequence() => PushParent(new ParallelSequence());

        /// <summary>
        /// Same as <see cref="Composites.Selector"/> except that it shuffles the children when initialized
        /// </summary>
        public BehaviorTreeBuilder RandomSelector() => PushParent(new RandomSelector());

        /// <summary>
        /// Same as <see cref="Composites.Sequence"/> except that it shuffles the children when initialized
        /// </summary>
        public BehaviorTreeBuilder RandomSequence() => PushParent(new RandomSequence());

        /// <summary>
        /// Attempt to execute each of its children until one of them return <see cref="BehaviorState.Success"/>. 
        /// If all children return <see cref="BehaviorState.Failure"/>, this node will also return <see cref="BehaviorState.Failure"/>.
        /// If a child returns <see cref="BehaviorState.Running"/> it will tick again.
        /// </summary>
        public BehaviorTreeBuilder Selector() => PushParent(new Selector());

        /// <summary>    
        /// Attempt to execute all of its children and report <see cref="BehaviorState.Success"/> in case all of the children report a <see cref="BehaviorState.Success"/>.
        /// If at least one child reports a <see cref="BehaviorState.Failure"/>, this node will also return <see cref="BehaviorState.Failure"/> and restart.
        /// In case a child returns <see cref="BehaviorState.Running"/> this node will tick again.
        /// </summary>
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

        /// <summary>
        /// Create a <see cref="BehaviorTree"/> instance
        /// </summary>
        public BehaviorTree Build(IBlackboard blackboard, int updateIntervalInMilliseconds = 0)
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