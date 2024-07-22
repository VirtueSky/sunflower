using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Inspector;


namespace VirtueSky.DataType
{
    [Serializable]
    public class DictionaryCustom<TKey, TValue> : ISerializationCallbackReceiver, IDictionary, IDictionary<TKey, TValue>
    {
        [TableList, SerializeField] private List<DictionaryCustomData<TKey, TValue>> dictionaryData;

        [NonSerialized] private Dictionary<TKey, TValue> m_dict = new Dictionary<TKey, TValue>();

        public void OnAfterDeserialize()
        {
            UpdateDict();
        }

        public void OnBeforeSerialize()
        {
            UpdateList();
        }

        private void UpdateDict()
        {
            if (dictionaryData is { Count: > 0 })
            {
                if (m_dict is { Count: > 0 })
                {
                    m_dict.Clear();
                }

                foreach (var data in dictionaryData)
                {
                    if (data.key != null && data.value != null)
                    {
                        m_dict[data.key] = data.value;
                    }
                }
            }
        }

        private void UpdateList()
        {
            if (Application.isPlaying)
            {
                if (m_dict is { Count: > 0 })
                {
                    dictionaryData.Clear();
                    foreach (var kvp in m_dict)
                    {
                        dictionaryData.Add(new DictionaryCustomData<TKey, TValue>(kvp.Key, kvp.Value));
                    }
                }
            }
        }

        public void Add(object key, object value)
        {
            m_dict.Add((TKey)key, (TValue)value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            m_dict.Add(item.Key, item.Value);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            m_dict.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary)m_dict).Contains((TKey)item.Key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)m_dict).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)m_dict).Remove(item);
        }

        public int Count => m_dict.Count;

        public bool IsReadOnly => ((IDictionary)m_dict).IsReadOnly;


        void IDictionary.Clear()
        {
            ((IDictionary<TKey, TValue>)m_dict).Clear();
        }

        public bool Contains(object key)
        {
            return ((IDictionary<TKey, TValue>)m_dict).Contains((KeyValuePair<TKey, TValue>)key);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return ((IDictionary<TKey, TValue>)m_dict).GetEnumerator();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary)m_dict).GetEnumerator();
        }

        public void Remove(object key)
        {
            m_dict.Remove((TKey)key);
        }

        public bool IsFixedSize => ((IDictionary)m_dict).IsFixedSize;


        public object this[object key]
        {
            get => ((IDictionary)m_dict)[key];
            set => ((IDictionary)m_dict)[key] = value;
        }

        ICollection IDictionary.Keys => ((IDictionary)m_dict).Keys;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => (ICollection<TValue>)((IDictionary)m_dict).Values;

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => (ICollection<TKey>)((IDictionary)m_dict).Keys;

        ICollection IDictionary.Values => ((IDictionary)m_dict).Values;

        public void Add(TKey key, TValue value)
        {
            m_dict.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return m_dict.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            return m_dict.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return m_dict.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get => ((IDictionary<TKey, TValue>)m_dict)[key];
            set => ((IDictionary<TKey, TValue>)m_dict)[key] = value;
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary)m_dict).GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            ((IDictionary)m_dict).CopyTo(array, index);
        }


        public bool IsSynchronized => ((IDictionary)m_dict).IsSynchronized;
        public object SyncRoot => ((IDictionary)m_dict).SyncRoot;
    }

    [Serializable]
    public class DictionaryCustomData<TKey, TValue>
    {
        public TKey key;
        public TValue value;

        public DictionaryCustomData(TKey _key, TValue _value)
        {
            key = _key;
            value = _value;
        }
    }
}