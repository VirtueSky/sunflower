using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using VirtueSky.Hierarchy.Data;
using VirtueSky.Hierarchy;
using UnityEditor.Callbacks;
using VirtueSky.Hierarchy.Helper;
using VirtueSky.Hierarchy.phierarchy;

namespace VirtueSky.Hierarchy
{
    [InitializeOnLoad]
    public class QHierarchyInitializer
    {
        private static VHierarchy hierarchy;

        static QHierarchyInitializer()
        {
            EditorApplication.update -= update;
            EditorApplication.update += update;

            EditorApplication.hierarchyWindowItemOnGUI -= hierarchyWindowItemOnGUIHandler;
            EditorApplication.hierarchyWindowItemOnGUI += hierarchyWindowItemOnGUIHandler;

            EditorApplication.hierarchyChanged -= hierarchyWindowChanged;
            EditorApplication.hierarchyChanged += hierarchyWindowChanged;

            Undo.undoRedoPerformed -= undoRedoPerformed;
            Undo.undoRedoPerformed += undoRedoPerformed;
        }

        static void undoRedoPerformed()
        {
            EditorApplication.RepaintHierarchyWindow();
        }

        static void init()
        {
            hierarchy = new VHierarchy();
        }

        static void update()
        {
            if (hierarchy == null) init();
            HierarchyObjectListManager.getInstance().update();
        }

        static void hierarchyWindowItemOnGUIHandler(int instanceId, Rect selectionRect)
        {
            if (hierarchy == null) init();
            hierarchy.hierarchyWindowItemOnGUIHandler(instanceId, selectionRect);
        }

        static void hierarchyWindowChanged()
        {
            if (hierarchy == null) init();
            HierarchyObjectListManager.getInstance().validate();
        }
    }
}