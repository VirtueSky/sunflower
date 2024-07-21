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
        [NonSerialized] private Dictionary<TKey, TValue> m_dictionary = new Dictionary<TKey, TValue>();
        private ICollection _keys;
        private ICollection _values;
        private int _count;
        private bool _isReadOnly;
        private ICollection _keys1;
        private ICollection _values1;
        private int _count1;
        private bool _isReadOnly1;
        private ICollection<TKey> _keys2;
        private ICollection<TValue> _values2;
        public Dictionary<TKey, TValue> GetDictionary => m_dictionary;

        public void OnAfterDeserialize()
        {
            if (dictionaryData is { Count: > 0 })
            {
                if (m_dictionary is { Count: > 0 })
                {
                    m_dictionary.Clear();
                }

                foreach (var data in dictionaryData)
                {
                    if (data.key != null && data.value != null)
                    {
                        m_dictionary[data.key] = data.value;
                    }
                }

                //dictionaryData.Clear();
                // dictionaryData = null;
            }
        }

        public void OnBeforeSerialize()
        {
        }

        private void OnValueChanged()
        {
            if (m_dictionary is { Count: > 0 })
            {
                dictionaryData.Clear();
                foreach (var kvp in m_dictionary)
                {
                    dictionaryData.Add(new DictionaryCustomData<TKey, TValue>(kvp.Key, kvp.Value));
                }
            }
        }

        public void Add(object key, object value)
        {
            m_dictionary.Add((TKey)key, (TValue)value);
            OnValueChanged();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            m_dictionary.Add(item.Key, item.Value);
            OnValueChanged();
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            m_dictionary.Clear();
            OnValueChanged();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary)m_dictionary).Contains((TKey)item.Key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)m_dictionary).CopyTo(array, arrayIndex);
            OnValueChanged();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)m_dictionary).Remove(item);
        }

        int ICollection<KeyValuePair<TKey, TValue>>.Count => _count1;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => _isReadOnly1;

        void IDictionary.Clear()
        {
            ((IDictionary<TKey, TValue>)m_dictionary).Clear();
        }

        public bool Contains(object key)
        {
            return ((IDictionary<TKey, TValue>)m_dictionary).Contains((KeyValuePair<TKey, TValue>)key);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return ((IDictionary<TKey, TValue>)m_dictionary).GetEnumerator();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary)m_dictionary).GetEnumerator();
        }

        public void Remove(object key)
        {
            m_dictionary.Remove((TKey)key);
            OnValueChanged();
        }

        public bool IsFixedSize => ((IDictionary)m_dictionary).IsFixedSize;

        bool IDictionary.IsReadOnly => _isReadOnly;

        public object this[object key]
        {
            get => ((IDictionary)m_dictionary)[key];
            set => ((IDictionary)m_dictionary)[key] = value;
        }

        public void Add(TKey key, TValue value)
        {
            m_dictionary.Add(key, value);
            OnValueChanged();
        }

        public bool ContainsKey(TKey key)
        {
            return m_dictionary.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            return m_dictionary.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return m_dictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get => ((IDictionary<TKey, TValue>)m_dictionary)[key];
            set => ((IDictionary<TKey, TValue>)m_dictionary)[key] = value;
        }

        ICollection IDictionary.Keys => _keys1;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => _values2;

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => _keys2;

        ICollection IDictionary.Values => _values1;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary)m_dictionary).GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            ((IDictionary)m_dictionary).CopyTo(array, index);
        }

        int ICollection.Count => _count;

        public bool IsSynchronized => ((IDictionary)m_dictionary).IsSynchronized;
        public object SyncRoot => ((IDictionary)m_dictionary).SyncRoot;
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