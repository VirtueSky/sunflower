using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetFinderNavigationHistory
    {
        private readonly List<UnityObject[]> history = new List<UnityObject[]>();
        private int currentIndex = -1;
        private const int MAX_HISTORY_SIZE = 20;
        private AssetFinderWindowAll window;
        private bool isNavigating = false;
        
        public bool CanGoBack => currentIndex > 0 && GetValidHistoryCount() > 1;
        public bool CanGoForward => currentIndex < history.Count - 1 && GetValidHistoryCount() > 1;
        
        public void SetWindow(AssetFinderWindowAll windowAll)
        {
            window = windowAll;
        }
        
        public void RecordSelection(UnityObject[] selection)
        {
            if (selection == null || selection.Length == 0) return;
            if (isNavigating) return;
            
            var validSelection = selection.Where(obj => obj != null).ToArray();
            if (validSelection.Length == 0) return;
            
            if (currentIndex >= 0 && currentIndex < history.Count)
            {
                UnityObject[] current = CleanHistoryEntry(history[currentIndex]);
                if (AreSelectionsEqual(current, validSelection)) return;
            }
            
            if (currentIndex < history.Count - 1)
            {
                history.RemoveRange(currentIndex + 1, history.Count - currentIndex - 1);
            }
            
            history.Add(validSelection.ToArray());
            currentIndex = history.Count - 1;
            
            if (history.Count > MAX_HISTORY_SIZE)
            {
                history.RemoveAt(0);
                currentIndex--;
            }
        }
        
        public bool GoBack()
        {
            if (!CanGoBack) return false;
            
            CleanInvalidHistoryEntries();
            
            if (currentIndex <= 0) return false;
            
            currentIndex--;
            var validSelection = CleanHistoryEntry(history[currentIndex]);
            
            if (validSelection.Length == 0)
            {
                history.RemoveAt(currentIndex);
                if (currentIndex >= history.Count) currentIndex = history.Count - 1;
                return GoBack();
            }
            
            isNavigating = true;
            UpdateFR2SelectionDirectly(validSelection);
            isNavigating = false;
            return true;
        }
        
        public bool GoForward()
        {
            if (!CanGoForward) return false;
            
            CleanInvalidHistoryEntries();
            
            if (currentIndex >= history.Count - 1) return false;
            
            currentIndex++;
            var validSelection = CleanHistoryEntry(history[currentIndex]);
            
            if (validSelection.Length == 0)
            {
                history.RemoveAt(currentIndex);
                currentIndex--;
                return GoForward();
            }
            
            isNavigating = true;
            UpdateFR2SelectionDirectly(validSelection);
            isNavigating = false;
            return true;
        }
        
        private void CleanInvalidHistoryEntries()
        {
            for (int i = history.Count - 1; i >= 0; i--)
            {
                var cleanedEntry = CleanHistoryEntry(history[i]);
                if (cleanedEntry.Length == 0)
                {
                    history.RemoveAt(i);
                    if (currentIndex >= i) currentIndex--;
                }
                else
                {
                    history[i] = cleanedEntry;
                }
            }
            
            if (currentIndex < 0 && history.Count > 0) currentIndex = 0;
            if (currentIndex >= history.Count) currentIndex = history.Count - 1;
        }
        
        private UnityObject[] CleanHistoryEntry(UnityObject[] entry)
        {
            return entry?.Where(obj => obj != null).ToArray() ?? new UnityObject[0];
        }
        
        private int GetValidHistoryCount()
        {
            return history.Count(entry => CleanHistoryEntry(entry).Length > 0);
        }
        
        private void UpdateFR2SelectionDirectly(UnityObject[] selection)
        {
            if (window == null) return;
            
            var validSelection = selection?.Where(obj => obj != null).ToArray() ?? new UnityObject[0];
            
            // For navigation history, we want to directly set both Unity and FR2 selection
            // We bypass the smart lock mechanism entirely by setting FR2 selection directly
            window.SetFR2Selection(validSelection);
            Selection.objects = validSelection;
        }
        
        private static bool AreSelectionsEqual(UnityObject[] a, UnityObject[] b)
        {
            if (a.Length != b.Length) return false;
            
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }
            
            return true;
        }
    }
} 