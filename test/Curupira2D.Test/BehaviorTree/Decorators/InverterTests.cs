using Curupira2D.AI.BehaviorTree;

namespace Curupira2D.Test.BehaviorTree.Decorators
{
    public class InverterTests
    {
        readonly Blackboard _blackboard;

        public InverterTests()
        {
            _blackboard = new Blackboard();
        }

        [Theory(DisplayName = "Return action state when delay end")]
        [Trait("Inverter", "Decorators")]
        [InlineData(BehaviorState.Success, BehaviorState.Failure)]
        [InlineData(BehaviorState.Failure, BehaviorState.Success)]
        public void Should_InverterState(BehaviorState actionState, BehaviorState expectedState)
        {
            // Arrange
            var behaviorTreeBuilder = BehaviorTreeBuilder.GetInstance();

            behaviorTreeBuilder
                .Inverter()
                    .ExecuteAction((bb) => actionState);

            var tree = behaviorTreeBuilder.Build(_blackboard);

            // Act
            tree.Tick();
            var lastState = _blackboard.Get<BehaviorState>("_LastState");

            // Assert
            Assert.Equal(expectedState, lastState);
        }

        [Fact(DisplayName = "Return running")]
        [Trait("Inverter", "Decorators")]
        public void Should_ReturnLastStateRunning_When_LeafRunning()
        {
            // Arrange
            var behaviorTreeBuilder = BehaviorTreeBuilder.GetInstance();

            behaviorTreeBuilder
                .Inverter()
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