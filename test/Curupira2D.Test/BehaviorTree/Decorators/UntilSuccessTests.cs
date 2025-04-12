using Curupira2D.AI.BehaviorTree;

namespace Curupira2D.Test.BehaviorTree.Decorators
{
    public class UntilSuccessTests
    {
        readonly Blackboard _blackboard;

        public UntilSuccessTests()
        {
            _blackboard = new Blackboard();
        }

        [Fact(DisplayName = "Return success when action success")]
        [Trait("UntilSuccess", "Decorators")]
        public void Should_ReturnSuccess_When_LeafSuccess()
        {
            // Arrange
            var behaviorTreeBuilder = BehaviorTreeBuilder.GetInstance();

            behaviorTreeBuilder
                .UntilSuccess()
                    .ExecuteAction((bb) => BehaviorState.Success);

            var tree = behaviorTreeBuilder.Build(_blackboard);

            // Act
            tree.Tick();
            var lastState = _blackboard.Get<BehaviorState>("_LastState");

            // Assert
            Assert.Equal(BehaviorState.Success, lastState);
        }

        [Fact(DisplayName = "Return running")]
        [Trait("UntilSuccess", "Decorators")]
        public void Should_ReturnLastStateRunning_When_LeafRunning()
        {
            // Arrange
            var behaviorTreeBuilder = BehaviorTreeBuilder.GetInstance();

            behaviorTreeBuilder
                .UntilSuccess()
                    .ExecuteAction((bb) => BehaviorState.Failure);

            var tree = behaviorTreeBuilder.Build(_blackboard);

            // Act
            tree.Tick();
            var lastState = _blackboard.Get<BehaviorState>("_LastState");

            // Assert
            Assert.Equal(BehaviorState.Running, lastState);
        }
    }
}