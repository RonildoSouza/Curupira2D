using Curupira2D.AI.BehaviorTree;

namespace Curupira2D.Test.BehaviorTree.Composites
{
    public class SelectorTests
    {
        readonly Blackboard _blackboard;

        public SelectorTests()
        {
            _blackboard = new Blackboard();
        }

        [Theory(DisplayName = "Execute any success action")]
        [Trait("Tick", "Selector")]
        [InlineData(false)]
        [InlineData(true)]
        public void Should_ReturnLastStateSuccess_When_AnyLeafSuccess(bool randomComposite)
        {
            // Arrange
            var behaviorTreeBuilder = randomComposite
                ? BehaviorTreeBuilder.GetInstance().RandomSelector()
                : BehaviorTreeBuilder.GetInstance().Selector();

            behaviorTreeBuilder
                .ExecuteAction((bb) => randomComposite ? BehaviorState.Success : BehaviorState.Failure)
                .ExecuteAction((bb) => BehaviorState.Success)
            .Close();

            var tree = behaviorTreeBuilder.Build(_blackboard);

            // Act
            tree.Tick(); // Tick action 1
            tree.Tick(); // Tick action 2
            var lastState = _blackboard.Get<BehaviorState>("_LastState");

            // Assert
            Assert.Equal(BehaviorState.Success, lastState);
        }

        [Theory(DisplayName = "Execute failure actions")]
        [Trait("Tick", "Selector")]
        [InlineData(false)]
        [InlineData(true)]
        public void Should_ReturnLastStateFailure_When_AllLeafFailure(bool randomComposite)
        {
            // Arrange
            var behaviorTreeBuilder = randomComposite
                ? BehaviorTreeBuilder.GetInstance().RandomSelector()
                : BehaviorTreeBuilder.GetInstance().Selector();

            behaviorTreeBuilder
                .ExecuteAction((bb) => BehaviorState.Failure)
                .ExecuteAction((bb) => BehaviorState.Failure)
            .Close();

            var tree = behaviorTreeBuilder.Build(_blackboard);

            // Act
            tree.Tick(); // Tick action 1
            tree.Tick(); // Tick action 2
            var lastState = _blackboard.Get<BehaviorState>("_LastState");

            // Assert
            Assert.Equal(BehaviorState.Failure, lastState);
        }

        [Theory(DisplayName = "Execute running action")]
        [Trait("Tick", "Selector")]
        [InlineData(false)]
        [InlineData(true)]
        public void Should_ReturnLastStateRunning_When_LeafRunning(bool randomComposite)
        {
            // Arrange
            var behaviorTreeBuilder = randomComposite
                ? BehaviorTreeBuilder.GetInstance().RandomSelector()
                : BehaviorTreeBuilder.GetInstance().Selector();

            behaviorTreeBuilder
                .ExecuteAction((bb) => BehaviorState.Running)
                .ExecuteAction((bb) => randomComposite ? BehaviorState.Running : BehaviorState.Invalid)
            .Close();

            var tree = behaviorTreeBuilder.Build(_blackboard);

            // Act
            tree.Tick(); // Tick action 1
            tree.Tick(); // Tick action 2
            var lastState = _blackboard.Get<BehaviorState>("_LastState");

            // Assert
            Assert.Equal(BehaviorState.Running, lastState);
        }
    }
}