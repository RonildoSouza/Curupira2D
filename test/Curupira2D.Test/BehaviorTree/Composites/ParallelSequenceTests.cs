using Curupira2D.AI.BehaviorTree;

namespace Curupira2D.Test.BehaviorTree.Composites
{
    public class ParallelSequenceTests
    {
        readonly Blackboard _blackboard;

        public ParallelSequenceTests()
        {
            _blackboard = new Blackboard();
        }

        [Fact(DisplayName = "Execute success actions")]
        [Trait("Tick", "ParallelSequence")]
        public void Should_ReturnLastStateSuccess_When_AllLeafSuccessInOneTick()
        {
            // Arrange
            var behaviorTreeBuilder = BehaviorTreeBuilder.GetInstance();

            behaviorTreeBuilder
                .ParallelSequence()
                    .ExecuteAction((bb) => BehaviorState.Success)
                    .ExecuteAction((bb) => BehaviorState.Success)
                .Close();

            var tree = behaviorTreeBuilder.Build(_blackboard);

            // Act
            tree.Tick();
            var lastState = _blackboard.Get<BehaviorState>("_LastState");

            // Assert
            Assert.Equal(BehaviorState.Success, lastState);
        }

        [Fact(DisplayName = "Execute any failure action")]
        [Trait("Tick", "ParallelSequence")]
        public void Should_ReturnLastStateFailure_When_AnyLeafFailureInOneTick()
        {
            // Arrange
            var behaviorTreeBuilder = BehaviorTreeBuilder.GetInstance();

            behaviorTreeBuilder
                .ParallelSequence()
                    .ExecuteAction((bb) => BehaviorState.Success)
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
        [Trait("Tick", "ParallelSequence")]
        public void Should_ReturnLastStateRunning_When_LeafRunningInOneTick()
        {
            // Arrange
            var behaviorTreeBuilder = BehaviorTreeBuilder.GetInstance();

            behaviorTreeBuilder
                .ParallelSequence()
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