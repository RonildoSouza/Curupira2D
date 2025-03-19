using System.Diagnostics;

namespace Curupira2D.AI.BehaviorTree.Leafs
{
    /// <summary>
	/// <see cref="DebugLogAction"/> will output the specified text and return <see cref="NodeState.Success"/>
	/// </summary>
    public class DebugLogAction(string text) : ActionLeaf
    {
        public string Text { get; set; } = text;
        public bool IsError { get; set; }

        public override NodeState Update(IBlackboard blackboard)
        {
            if (IsError)
                Debug.WriteLine(Text, "Error");
            else
                Debug.WriteLine(Text, "Log");

            return NodeState.Success;
        }
    }
}
