using Curupira2D.AI.BehaviorTree;

namespace Curupira2D.Test.BehaviorTree.Decorators
{
    public class AlwaysFailureTests
    {
        readonly Blackboard _blackboard;

        public AlwaysFailureTests()
        {
            _blackboard = new Blackboard();
        }

        [Fact(DisplayName = "Always return failure")]
        [Trait("AlwaysFailure", "Decorators")]
        public void Should_AlwaysReturnFailure()
        {
            // Arrange
            var behaviorTreeBuilder = BehaviorTreeBuilder.GetInstance();

            behaviorTreeBuilder
                .AlwaysFailure()
                    .ExecuteAction((bb) => BehaviorState.Success);

            var tree = behaviorTreeBuilder.Build(_blackboard);

            // Act
            tree.Tick();
            var lastState = _blackboard.Get<BehaviorState>("_LastState");

            // Assert
            Assert.Equal(BehaviorState.Failure, lastState);
        }

        [Fact(DisplayName = "Return running")]
        [Trait("AlwaysFailure", "Decorators")]
        public void Should_ReturnLastStateRunning_When_LeafRunning()
        {
            // Arrange
            var behaviorTreeBuilder = BehaviorTreeBuilder.GetInstance();

            behaviorTreeBuilder
                .AlwaysFailure()
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