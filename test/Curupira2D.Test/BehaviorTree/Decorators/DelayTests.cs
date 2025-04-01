using Curupira2D.AI.BehaviorTree;

namespace Curupira2D.Test.BehaviorTree.Decorators
{
    public class DelayTests
    {
        readonly Blackboard _blackboard;

        public DelayTests()
        {
            _blackboard = new Blackboard();
        }

        [Fact(DisplayName = "Return action state when delay end")]
        [Trait("Delay", "Decorators")]
        public void Should_ReturnLastStateSuccess_When_DelayEnd()
        {
            // Arrange
            var milliseconds = 100;
            var behaviorTreeBuilder = BehaviorTreeBuilder.GetInstance();

            behaviorTreeBuilder
                .Delay(milliseconds)
                    .ExecuteAction((bb) => BehaviorState.Success);

            var tree = behaviorTreeBuilder.Build(_blackboard);

            // Act
            tree.Tick();
            Thread.Sleep(milliseconds + 1);
            tree.Tick();
            var lastState = _blackboard.Get<BehaviorState>("_LastState");

            // Assert
            Assert.Equal(BehaviorState.Success, lastState);
        }

        [Fact(DisplayName = "Return running")]
        [Trait("Delay", "Decorators")]
        public void Should_ReturnLastStateRunning_When_LeafRunning()
        {
            // Arrange
            var behaviorTreeBuilder = BehaviorTreeBuilder.GetInstance();

            behaviorTreeBuilder
                .Delay(100)
                    .ExecuteAction((bb) => BehaviorState.Success);

            var tree = behaviorTreeBuilder.Build(_blackboard);

            // Act
            tree.Tick();
            var lastState = _blackboard.Get<BehaviorState>("_LastState");

            // Assert
            Assert.Equal(BehaviorState.Running, lastState);
        }

        [Fact(DisplayName = "Throw ArgumentException")]
        [Trait("Delay", "Decorators")]
        public void Should_ThrowArgumentException_When_NotAllowedMilliseconds()
        {
            // Arrange, Act & Assert
            Assert.Throws<ArgumentException>(
                () => BehaviorTreeBuilder.GetInstance().Delay(99));
        }
    }
}