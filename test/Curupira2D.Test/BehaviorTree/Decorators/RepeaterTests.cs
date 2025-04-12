using Curupira2D.AI.BehaviorTree;

namespace Curupira2D.Test.BehaviorTree.Decorators
{
    public class RepeaterTests
    {
        readonly Blackboard _blackboard;

        public RepeaterTests()
        {
            _blackboard = new Blackboard();
        }

        [Theory(DisplayName = "Return success when repeat end")]
        [Trait("Repeater", "Decorators")]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void Should_ReturnLastStateSuccess_When_RepetEnd(bool repeatForever, bool endOnFailure)
        {
            // Arrange
            var behaviorTreeBuilder = BehaviorTreeBuilder.GetInstance();

            behaviorTreeBuilder
                .Repeater(2, repeatForever, endOnFailure)
                    .ExecuteAction((bb) => BehaviorState.Failure);

            var tree = behaviorTreeBuilder.Build(_blackboard);

            // Act
            tree.Tick();
            tree.Tick();
            var lastState = _blackboard.Get<BehaviorState>("_LastState");

            // Assert
            Assert.Equal(BehaviorState.Success, lastState);
        }

        [Theory(DisplayName = "Return running")]
        [Trait("Repeater", "Decorators")]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, false)]
        [InlineData(true, true)]
        public void Should_ReturnLastStateRunning_When_LeafRunning(bool repeatForever, bool endOnFailure)
        {
            // Arrange
            var behaviorTreeBuilder = BehaviorTreeBuilder.GetInstance();

            behaviorTreeBuilder
                .Repeater(2, repeatForever, endOnFailure)
                    .ExecuteAction((bb) => BehaviorState.Success);

            var tree = behaviorTreeBuilder.Build(_blackboard);

            // Act
            tree.Tick();
            var lastState = _blackboard.Get<BehaviorState>("_LastState");

            // Assert
            Assert.Equal(BehaviorState.Running, lastState);
        }
    }
}