using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace VirtueSky.UtilsEditor
{
    public class ExSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        [Flags]
        public enum SortType
        {
            None = 0,
            Directory = 1,
            Alphabet = 2
        }

        private struct Entry
        {
            public readonly GUIContent content;
            public readonly object data;
            public readonly Action<object> onSelect;

            public Entry(GUIContent content, object data, Action<object> onSelect)
            {
                this.content = content;
                this.data = data;
                this.onSelect = onSelect;
            }
        }

        private string title = string.Empty;
        private Texture2D emptyIcon;
        private SortType sortType = SortType.Directory | SortType.Alphabet;
        private List<Entry> entries = new List<Entry>();

        /// <summary>
        /// Generates data to populate the search window.
        /// </summary>
        /// <param name="context">Contextual data initially passed the window when first created.</param>
        /// <returns>Returns the list of SearchTreeEntry objects displayed in the search window.</returns>
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            if (sortType != SortType.None)
            {
                entries.Sort(SortEntriesByGroup);
            }

            List<SearchTreeEntry> treeEntries = new List<SearchTreeEntry>() {new SearchTreeGroupEntry(new GUIContent(title), 0)};

            List<string> groups = new List<string>();
            for (int i = 0; i < entries.Count; i++)
            {
                Entry entry = entries[i];

                string group = string.Empty;
                string[] paths = entry.content.text.Split('/');
                int length = paths.Length - 1;
                for (int j = 0; j < length; j++)
                {
                    string path = paths[j];

                    group += path;
                    if (!groups.Contains(group))
                    {
                        treeEntries.Add(new SearchTreeGroupEntry(new GUIContent(path), j + 1));
                        groups.Add(group);
                    }

                    group += "/";
                }

                entry.content.text = paths[length];
                SearchTreeEntry searchTreeEntry = new SearchTreeEntry(entry.content);
                searchTreeEntry.userData = i;
                searchTreeEntry.level = paths.Length;
                treeEntries.Add(searchTreeEntry);
            }

            return treeEntries;
        }

        /// <summary>
        /// Selects an entry in the search tree list.
        /// </summary>
        /// <param name="searchTreeEntry">The selected entry.</param>
        /// <param name="context">Contextual data to pass to the search window when it is first created.</param>
        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            Entry entry = entries[(int) searchTreeEntry.userData];
            if (entry.onSelect != null)
            {
                entry.onSelect.Invoke(entry.data);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Add new entry.
        /// </summary>
        /// <param name="content">The text and icon of the search entry.</param>
        /// <param name="data">A user specified object for attaching application specific data to a search tree entry.</param>
        /// <param name="onSelect">Action with data argument, which called after entry is selected.</param>
        public void AddEntry(GUIContent content, object data, Action<object> onSelect) { entries.Add(new Entry(content, data, onSelect)); }

        /// <summary>
        /// Add new entry.
        /// </summary>
        /// <param name="content">The text and icon of the search entry.</param>
        /// <param name="onSelect">Action which called after entry is selected.</param>
        public void AddEntry(GUIContent content, Action onSelect) { entries.Add(new Entry(content, null, (data) => onSelect?.Invoke())); }

        /// <summary>
        /// Add new entry.
        /// </summary>
        /// <param name="name">The name of the search entry.</param>
        /// <param name="data">A user specified object for attaching application specific data to a search tree entry.</param>
        /// <param name="onSelect">Action with data argument, which called after entry is selected.</param>
        public void AddEntry(string name, object data, Action<object> onSelect) { AddEntry(new GUIContent(name), data, onSelect); }

        /// <summary>
        /// Add new entry.
        /// </summary>
        /// <param name="name">The name of the search entry.</param>
        /// <param name="onSelect">Action which called after entry is selected.</param>
        public void AddEntry(string name, Action onSelect) { AddEntry(new GUIContent(name), null, (data) => onSelect?.Invoke()); }

        /// <summary>
        /// Add new indented entity.
        /// </summary>
        /// <param name="name">Name of none label.</param>
        /// <param name="onSelect">Action which called after entry is selected.</param>
        public void AddEntityIndented(string name, Action onSelect)
        {
            GUIContent content = new GUIContent(name, GetEmptyIcon());
            AddEntry(content, onSelect);
        }

        /// <summary>
        /// Add new indented entity.
        /// </summary>
        /// <param name="name">Name of none label.</param>
        /// <param name="data">A user specified object for attaching application specific data to a search tree entry.</param>
        /// <param name="onSelect">Action with data argument, which called after entry is selected.</param>
        public void AddEntityIndented(string name, object data, Action<object> onSelect)
        {
            GUIContent content = new GUIContent(name, GetEmptyIcon());
            AddEntry(content, data, onSelect);
        }


        /// <summary>
        /// Open search window.
        /// </summary>
        /// <param name="position">Window position in screen space.</param>
        /// <param name="width">Requested width of the window. Set to 0.0f to use the default width.</param>
        /// <param name="height">Requested height of the window. Set to 0.0f to use the default height.</param>
        public void Open(Vector2 position, float width = 0, float height = 0) { SearchWindow.Open(new SearchWindowContext(position, width, height), this); }

        /// <summary>
        /// Open search window in mouse position.
        /// </summary>
        /// <param name="width">Requested width of the window. Set to 0.0f to use the default width.</param>
        /// <param name="height">Requested height of the window. Set to 0.0f to use the default height.</param>
        public void Open(float width = 0, float height = 0)
        {
            Vector2 position = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            Open(position, width, height);
        }

        /// <summary>
        /// Open search window in button position.
        /// </summary>
        /// <param name="buttonRect">Rectangle of GUI button.</param>
        /// <param name="width">Requested width of the window. Set to 0.0f to use the button width.</param>
        /// <param name="height">Requested height of the window. Set to 0.0f to use the default height.</param>
        public void Open(Rect buttonRect, float width = 0, float height = 0)
        {
            Rect screenRect = GUIUtility.GUIToScreenRect(buttonRect);

            Vector2 position = screenRect.position;
            position.x += screenRect.width / 2;
            position.y += screenRect.height + 15;

            width = Mathf.Max(0, width);
            width = width != 0 ? width : screenRect.width;

            Open(position, width != 0 ? width : screenRect.width, height);
        }

        /// <summary>
        /// Sort entries by paths.
        /// </summary>
        /// <param name="lhs">Left hand side entry.</param>
        /// <param name="rhs">Right hand side entry.</param>
        private int SortEntriesByGroup(Entry lhs, Entry rhs)
        {
            string[] lhsPaths = lhs.content.text.Split('/');
            string[] rhsPaths = rhs.content.text.Split('/');

            int lhsLength = lhsPaths.Length;
            int rhsLength = rhsPaths.Length;
            int minLength = Mathf.Min(lhsLength, rhsLength);

            for (int i = 0; i < minLength; i++)
            {
                if ((sortType & SortType.Directory) != 0)
                {
                    if (minLength - 1 == i)
                    {
                        int compareDepth = rhsLength.CompareTo(lhsLength);
                        if (compareDepth != 0)
                        {
                            return compareDepth;
                        }
                    }
                }

                if ((sortType & SortType.Alphabet) != 0)
                {
                    int compareText = lhsPaths[i].CompareTo(rhsPaths[i]);
                    if (compareText != 0)
                    {
                        return compareText;
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// Get empty icon.
        /// </summary>
        private Texture GetEmptyIcon()
        {
            if (emptyIcon == null)
            {
                emptyIcon = new Texture2D(1, 1);
                emptyIcon.SetPixel(0, 0, Color.clear);
                emptyIcon.Apply();
            }

            return emptyIcon;
        }

        #region [Static Methods]

        /// <summary>
        /// Create ExLib search window.
        /// </summary>
        /// <param name="title">Window title.</param>
        /// <returns>ExSearchWindow instance.</returns>
        public static ExSearchWindow Create(string title)
        {
            ExSearchWindow window = CreateInstance<ExSearchWindow>();
            window.SetTitle(title);
            return window;
        }

        /// <summary>
        /// Create ExLib search window.
        /// </summary>
        /// <returns>ExSearchWindow instance.</returns>
        public static ExSearchWindow Create()
        {
            const string TITLE = "Entries";
            return Create(TITLE);
        }

        #endregion

        #region [Getter / Setter]

        public string GetTitle() { return title; }

        public void SetTitle(string value) { title = value; }

        public SortType GetSortType() { return sortType; }

        public void SetSortType(SortType value) { sortType = value; }

        #endregion
    }
}