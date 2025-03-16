namespace Curupira2D.AI.BehaviorTree
{
    /// <summary>
    /// Is a shared memory structure used in behavior tree to store and retrieve data across different nodes
    /// </summary>
    public sealed class Blackboard : IBlackboard
    {
        private readonly Dictionary<string, object> _data = [];

        public T Get<T>(string key, T defaultValue = default!) where T : notnull => (T)_data.GetValueOrDefault(key, defaultValue);

        public bool HasKey(string key) => _data.ContainsKey(key);

        public bool HasValue(object value) => _data.ContainsValue(value);

        public IEnumerable<string> Keys() => _data.Keys;

        public void Remove(string key) => _data.Remove(key);

        public void Set<T>(string key, T value) where T : notnull => _data[key] = value;
    }
}
