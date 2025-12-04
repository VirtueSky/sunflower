// #define AssetFinderDEBUG


using System;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;


namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetFinderSmartLock
    {
        public enum PingLockState
        {
            None,
            Scene,  // Ping/highlight action triggered from scene context
            Asset   // Ping/highlight action triggered from asset context
        }
        
        private PingLockState pingLockState = PingLockState.None;
        public void SetPingLockState(PingLockState state)
        {
            pingLockState = state;
            #if AssetFinderDEBUG
            if (state != PingLockState.None)
            {
                AssetFinderLOG.Log($"SmartLock: Set ping lock state to {state}");
            }
            #endif
        }
        
        public bool ConsumePingLockState()
        {
            bool hadPingLock = pingLockState != PingLockState.None;
            if (hadPingLock)
            {
                // AssetFinderLOG.Log($"SmartLock: Consuming ping lock state {pingLockState}");
                pingLockState = PingLockState.None;
            }
            return hadPingLock;
        }
        
        public bool ShouldRefreshWithSmartLogic(EditorWindow window, UnityObject[] panelSelection = null)
        {
            if (!AssetFinderSelectionManager.Instance.HasSelection) return false;
            if (ConsumePingLockState()) return false;
            if (panelSelection == null || panelSelection.Length == 0) return true;
            return window != EditorWindow.focusedWindow;
        }
    }
} 