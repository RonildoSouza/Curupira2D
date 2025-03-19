namespace Curupira2D.AI.BehaviorTree
{
    public interface IConditional
    {
        NodeState Update(IBlackboard blackboard);
    }
}