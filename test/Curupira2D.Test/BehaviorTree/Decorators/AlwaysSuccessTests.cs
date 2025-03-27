using Curupira2D.AI.BehaviorTree;

namespace Curupira2D.Test.BehaviorTree.Decorators
{
    public class AlwaysSuccessTests
    {
        readonly Blackboard _blackboard;

        public AlwaysSuccessTests()
        {
            _blackboard = new Blackboard();
        }

        [Fact(DisplayName = "Always return success")]
        [Trait("AlwaysSuccess", "Decorators")]
        public void Should_AlwaysReturnSuccess()
        {
            // Arrange
            var behaviorTreeBuilder = BehaviorTreeBuilder.GetInstance();

            behaviorTreeBuilder
                .AlwaysSuccess()
                    .ExecuteAction((bb) => BehaviorState.Failure);

            var tree = behaviorTreeBuilder.Build(_blackboard);

            // Act
            tree.Tick();
            var lastState = _blackboard.Get<BehaviorState>("_LastState");

            // Assert
            Assert.Equal(BehaviorState.Success, lastState);
        }

        [Fact(DisplayName = "Return running")]
        [Trait("AlwaysSuccess", "Decorators")]
        public void Should_ReturnLastStateRunning_When_LeafRunning()
        {
            // Arrange
            var behaviorTreeBuilder = BehaviorTreeBuilder.GetInstance();

            behaviorTreeBuilder
                .AlwaysSuccess()
                    .ExecuteAction((bb) => BehaviorState.Running);

            var tree = behaviorTreeBuilder.Build(_blackboard);

            // Act
            tree.Tick();
            var lastState = _blackboard.Get<BehaviorState>("_LastState");

            // Assert
            Assert.Equal(BehaviorState.Running, lastState);
        }
    }
}