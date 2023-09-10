using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace VirtueSky.DataStorage
{
    public interface IDataPersistent
    {
        string Key { get; }
        void LoadData(PersistentData data);
        void StoreData(PersistentData data);
    }

    [Serializable]
    public class PersistentData : AbstractPersistentData<string, object>
    {
        public T GetOrCreate<T>(string key) where T : new()
        {
            if (ContainsKey(key)) return Get<T>(key);

            var value = new T();
            Set(key, value);
            return value;
        }

        public void Store(IDataPersistent target, bool root = false)
        {
            var data = root ? this : GetOrCreate<PersistentData>(target.Key);
            target.StoreData(data);
        }

        public void Load(IDataPersistent target, bool root = false)
        {
            var data = root ? this : Get<PersistentData>(target.Key);
            if (data != null) target.LoadData(data);
        }

        public void Store(string key, IDataPersistent target)
        {
            var pd = new PersistentData();
            target.StoreData(pd);
            Set(key, pd);
        }

        public void Load(string key, IDataPersistent target)
        {
            var pd = Get<PersistentData>(key);
            if (pd != null) target.LoadData(pd);
        }

        public void ClearMaintainChildren()
        {
            var keys = this.Select(p => p.Key).ToList();
            foreach (var k in keys)
            {
                if (TryGetValue(k, out var val))
                {
                    if (val is PersistentData childPD) childPD.ClearMaintainChildren();
                    else Remove(k);
                }
            }
        }
    }

    [Serializable, JsonObject(MemberSerialization.OptIn)]
    public class AbstractPersistentData<K, V> : IEnumerable<KeyValuePair<K, V>>
    {
        [JsonProperty] Dictionary<K, V> data;

        public AbstractPersistentData()
        {
            data = new Dictionary<K, V>();
        }

        public bool TryGetValue(K key, out V value)
        {
            return data.TryGetValue(key, out value);
        }

        public bool TryGet<T>(K key, out T value)
        {
            value = default;
            if (!data.TryGetValue(key, out V v)) return false;
            value = To(v, default(T));
            return true;
        }

        public T Get<T>(K key, T defaultValue = default)
        {
            return data.TryGetValue(key, out var value) ? To(value, defaultValue) : defaultValue;
        }

        public void Set(K key, V value) => data[key] = value;

        public bool ContainsKey(K key) => data.ContainsKey(key);
        public void Remove(K key) => data.Remove(key);
        public void Clear() => data.Clear();

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator() => data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static T To<T>(object input, T defaultValue)
        {
            T result;
            if (typeof(T).IsEnum)
            {
                result = (T)Enum.ToObject(typeof(T), To(input, Convert.ToInt32(defaultValue)));
            }
            else
            {
                result = (T)Convert.ChangeType(input, typeof(T));
            }

            return result;
        }
    }
}