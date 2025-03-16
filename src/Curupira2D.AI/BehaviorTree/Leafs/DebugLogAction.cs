using System.Diagnostics;

namespace Curupira2D.AI.BehaviorTree.Leafs
{
    /// <summary>
	/// <see cref="DebugLogAction"/> will output the specified text and return <see cref="State.Success"/>
	/// </summary>
    public class DebugLogAction(string text) : ActionLeaf
    {
        public string Text { get; set; } = text;
        public bool IsError { get; set; }

        public override State Tick(IBlackboard blackboard)
        {
            if (IsError)
                Debug.WriteLine(Text, "Error");
            else
                Debug.WriteLine(Text, "Log");

            return State.Success;
        }
    }
}
