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

        public void Remove(string key, bool exactly = true)
        {
            if (exactly)
            {
                _data.Remove(key);
                return;
            }

            foreach (var keyToRemove in _data.Keys.Where(_ => _.Contains(key)))
                _data.Remove(keyToRemove);
        }

        public void Set<T>(string key, T value) where T : notnull => _data[key] = value;
    }
}
