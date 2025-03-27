using Curupira2D.AI.BehaviorTree;

namespace Curupira2D.Test.BehaviorTree.Composites
{
    public class ParallelSelectorTests
    {
        readonly Blackboard _blackboard;

        public ParallelSelectorTests()
        {
            _blackboard = new Blackboard();
        }

        [Fact(DisplayName = "Execute any success action")]
        [Trait("Tick", "ParallelSelector")]
        public void Should_ReturnLastStateSuccess_When_AnyLeafSuccessInOneTick()
        {
            // Arrange
            var behaviorTreeBuilder = BehaviorTreeBuilder.GetInstance();

            behaviorTreeBuilder
                .ParallelSelector()
                    .ExecuteAction((bb) => BehaviorState.Failure)
                    .ExecuteAction((bb) => BehaviorState.Success)
                .Close();

            var tree = behaviorTreeBuilder.Build(_blackboard);

            // Act
            tree.Tick();
            var lastState = _blackboard.Get<BehaviorState>("_LastState");

            // Assert
            Assert.Equal(BehaviorState.Success, lastState);
        }

        [Fact(DisplayName = "Execute failure actions")]
        [Trait("Tick", "ParallelSelector")]
        public void Should_ReturnLastStateFailure_When_AllLeafFailureInOneTick()
        {
            // Arrange
            var behaviorTreeBuilder = BehaviorTreeBuilder.GetInstance();

            behaviorTreeBuilder
                .ParallelSelector()
                    .ExecuteAction((bb) => BehaviorState.Failure)
                    .ExecuteAction((bb) => BehaviorState.Failure)
                .Close();

            var tree = behaviorTreeBuilder.Build(_blackboard);

            // Act
            tree.Tick();
            var lastState = _blackboard.Get<BehaviorState>("_LastState");

            // Assert
            Assert.Equal(BehaviorState.Failure, lastState);
        }

        [Fact(DisplayName = "Execute running action")]
        [Trait("Tick", "ParallelSelector")]
        public void Should_ReturnLastStateRunning_When_LeafRunningInOneTick()
        {
            // Arrange
            var behaviorTreeBuilder = BehaviorTreeBuilder.GetInstance();

            behaviorTreeBuilder
                .ParallelSelector()
                    .ExecuteAction((bb) => BehaviorState.Running)
                    .ExecuteAction((bb) => BehaviorState.Invalid)
                .Close();

            var tree = behaviorTreeBuilder.Build(_blackboard);

            // Act
            tree.Tick();
            var lastState = _blackboard.Get<BehaviorState>("_LastState");

            // Assert
            Assert.Equal(BehaviorState.Running, lastState);
        }
    }
}