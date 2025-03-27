using Curupira2D.AI.BehaviorTree;

namespace Curupira2D.Test.BehaviorTree.Composites
{
    public class SequenceTests
    {
        readonly Blackboard _blackboard;

        public SequenceTests()
        {
            _blackboard = new Blackboard();
        }

        [Theory(DisplayName = "Execute success actions")]
        [Trait("Tick", "Sequence")]
        [InlineData(false)]
        [InlineData(true)]
        public void Should_ReturnLastStateSuccess_When_AllLeafSuccess(bool randomComposite)
        {
            // Arrange
            var behaviorTreeBuilder = randomComposite
                ? BehaviorTreeBuilder.GetInstance().RandomSequence()
                : BehaviorTreeBuilder.GetInstance().Sequence();

            behaviorTreeBuilder
                .ExecuteAction((bb) => BehaviorState.Success)
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

        [Theory(DisplayName = "Execute any failure action")]
        [Trait("Tick", "Sequence")]
        [InlineData(false)]
        [InlineData(true)]
        public void Should_ReturnLastStateFailure_When_AnyLeafFailure(bool randomComposite)
        {
            // Arrange
            var behaviorTreeBuilder = randomComposite
                ? BehaviorTreeBuilder.GetInstance().RandomSequence()
                : BehaviorTreeBuilder.GetInstance().Sequence();

            behaviorTreeBuilder
                .ExecuteAction((bb) => randomComposite ? BehaviorState.Failure : BehaviorState.Success)
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
        [Trait("Tick", "Sequence")]
        [InlineData(false)]
        [InlineData(true)]
        public void Should_ReturnLastStateRunning_When_LeafRunning(bool randomComposite)
        {
            // Arrange
            var behaviorTreeBuilder = randomComposite
                ? BehaviorTreeBuilder.GetInstance().RandomSequence()
                : BehaviorTreeBuilder.GetInstance().Sequence();

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