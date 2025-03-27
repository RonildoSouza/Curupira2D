using System.Diagnostics;

namespace Curupira2D.AI.BehaviorTree.Leafs
{
    /// <summary>
	/// <see cref="DebugLogAction"/> will output the specified text and return <see cref="BehaviorState.Success"/>
	/// </summary>
    public class DebugLogAction(string text) : Leaf
    {
        public string Text { get; set; } = text;
        public bool IsError { get; set; }

        public override BehaviorState Update(IBlackboard blackboard)
        {
            if (IsError)
                Debug.WriteLine(Text, "Error");
            else
                Debug.WriteLine(Text, "Log");

            return State = BehaviorState.Success;
        }
    }
}
