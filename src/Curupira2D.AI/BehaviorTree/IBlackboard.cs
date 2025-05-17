
namespace Curupira2D.AI.BehaviorTree
{
    /// <summary>
    /// Is a shared structure used in behavior tree to store and retrieve data across different nodes
    /// </summary>
    public interface IBlackboard
    {
        T Get<T>(string key, T defaultValue = default!) where T : notnull;
        bool HasKey(string key);
        bool HasValue(object value);
        IEnumerable<string> Keys();
        void Remove(string key, bool exactly = true);
        void Set<T>(string key, T value) where T : notnull;
    }
}