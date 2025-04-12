namespace Curupira2D.AI.BehaviorTree.Decorators
{
    /// <summary>
    /// Will repeat execution of its child task until the child task has been run a specified number of times. 
    /// It has the option of continuing to execute the child task even if the child task returns a <see cref="BehaviorState.Failure"/>. 
    /// </summary>
    /// <param name="child"></param>
    /// <param name="repeatCount">The number of times to repeat the execution of its child</param>
    /// <param name="repeatForever">Allows the repeater to repeat forever</param>
    /// <param name="endOnFailure">Should the task return if the child task returns a <see cref="BehaviorState.Failure"/></param>
    public class Repeater(Behavior child, int repeatCount, bool repeatForever = false, bool endOnFailure = false) : Decorator(child)
    {
        int _currentCount;

        internal Repeater(int repeatCount, bool repeatForever = false, bool endOnFailure = false)
            : this(null!, repeatCount, repeatForever, endOnFailure) { }

        public override void OnInitialize(IBlackboard blackboard) => _currentCount = 0;

        public override BehaviorState Update(IBlackboard blackboard)
        {
            // early out if we are done. we check here and after running just in case the count is 0
            if (!repeatForever && _currentCount == repeatCount)
                return State = BehaviorState.Success;

            var childState = Child.Update(blackboard);
            _currentCount++;

            if (endOnFailure && childState == BehaviorState.Failure)
                return State = BehaviorState.Success;

            if (!repeatForever && _currentCount == repeatCount)
                return State = BehaviorState.Success;

            return State = BehaviorState.Running;
        }
    }
}
