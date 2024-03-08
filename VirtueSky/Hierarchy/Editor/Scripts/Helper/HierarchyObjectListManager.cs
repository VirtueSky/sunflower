using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VirtueSky.Hierarchy.Data;
#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
#endif

namespace VirtueSky.Hierarchy.Helper
{
    public class HierarchyObjectListManager
    {
        // CONST
        private const string HierarchyObjectListName = "QHierarchyObjectList";

        // SINGLETON
        private static HierarchyObjectListManager instance;
        public static HierarchyObjectListManager getInstance()
        {
            if (instance == null) instance = new HierarchyObjectListManager();
            return instance;
        }

        // PRIVATE
        private bool showObjectList;
        private bool preventSelectionOfLockedObjects;
        private bool preventSelectionOfLockedObjectsDuringPlayMode;
        private GameObject lastSelectionGameObject = null;
        private int lastSelectionCount = 0;

        // CONSTRUCTOR
        private HierarchyObjectListManager()
        {
            HierarchySettings.getInstance().addEventListener(HierarchySetting.AdditionalShowHiddenQHierarchyObjectList , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.LockPreventSelectionOfLockedObjects, settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.LockShow              , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.LockShowDuringPlayMode, settingsChanged);
            settingsChanged();
        }

        private void settingsChanged()
        {
            showObjectList = HierarchySettings.getInstance().get<bool>(HierarchySetting.AdditionalShowHiddenQHierarchyObjectList);
            preventSelectionOfLockedObjects = HierarchySettings.getInstance().get<bool>(HierarchySetting.LockShow) && HierarchySettings.getInstance().get<bool>(HierarchySetting.LockPreventSelectionOfLockedObjects);
            preventSelectionOfLockedObjectsDuringPlayMode = preventSelectionOfLockedObjects && HierarchySettings.getInstance().get<bool>(HierarchySetting.LockShowDuringPlayMode);
        }

        private bool isSelectionChanged()
        {
            if (lastSelectionGameObject != Selection.activeGameObject || lastSelectionCount  != Selection.gameObjects.Length)
            {
                lastSelectionGameObject = Selection.activeGameObject;
                lastSelectionCount = Selection.gameObjects.Length;
                return true;
            }
            return false;
        }

        public void validate()
        {
            ObjectList.instances.RemoveAll(item => item == null);
            foreach (ObjectList objectList in ObjectList.instances)
                objectList.checkIntegrity();
            #if UNITY_5_3_OR_NEWER
            objectListDictionary.Clear();
            foreach (ObjectList objectList in ObjectList.instances)            
                objectListDictionary.Add(objectList.gameObject.scene, objectList);
            #endif
        }

        #if UNITY_5_3_OR_NEWER
        private Dictionary<Scene, ObjectList> objectListDictionary = new Dictionary<Scene, ObjectList>();
        private Scene lastActiveScene;
        private int lastSceneCount = 0;

        public void update()
        {
            try
            {     
                List<ObjectList> objectListList = ObjectList.instances;
                int objectListCount = objectListList.Count;
                if (objectListCount > 0) 
                {
                    for (int i = objectListCount - 1; i >= 0; i--)
                    {
                        ObjectList objectList = objectListList[i];
                        Scene objectListScene = objectList.gameObject.scene;
						
						if (objectListDictionary.ContainsKey(objectListScene) && objectListDictionary[objectListScene] == null)
                            objectListDictionary.Remove(objectListScene);
							
                        if (objectListDictionary.ContainsKey(objectListScene))
                        {
                            if (objectListDictionary[objectListScene] != objectList)
                            {
                                objectListDictionary[objectListScene].merge(objectList);
                                GameObject.DestroyImmediate(objectList.gameObject);
                            }
                        }
                        else
                        {
                            objectListDictionary.Add(objectListScene, objectList);
                        }
                    }

                    foreach (KeyValuePair<Scene, ObjectList> objectListKeyValue in objectListDictionary)
                    {
                        ObjectList objectList = objectListKeyValue.Value;
                        setupObjectList(objectList);
                        if (( showObjectList && ((objectList.gameObject.hideFlags & HideFlags.HideInHierarchy)  > 0)) ||
                            (!showObjectList && ((objectList.gameObject.hideFlags & HideFlags.HideInHierarchy) == 0)))
                        {
                            objectList.gameObject.hideFlags ^= HideFlags.HideInHierarchy;      
                            EditorApplication.DirtyHierarchyWindowSorting();
                        }
                    }
                    
                    if ((!Application.isPlaying && preventSelectionOfLockedObjects) || 
                        ((Application.isPlaying && preventSelectionOfLockedObjectsDuringPlayMode)) && 
                        isSelectionChanged())
                    {
                        GameObject[] selections = Selection.gameObjects;
                        List<GameObject> actual = new List<GameObject>(selections.Length);
                        bool found = false;
                        for (int i = selections.Length - 1; i >= 0; i--)
                        {
                            GameObject gameObject = selections[i];
                            
                            if (objectListDictionary.ContainsKey(gameObject.scene))
                            {
                                bool isLock = objectListDictionary[gameObject.scene].lockedObjects.Contains(selections[i]);
                                if (!isLock) actual.Add(selections[i]);
                                else found = true;
                            }
                        }
                        if (found) Selection.objects = actual.ToArray();
                    }   

                    lastActiveScene = EditorSceneManager.GetActiveScene();
                    lastSceneCount = SceneManager.loadedSceneCount;
                }
            }
            catch 
            {
            }
        }

        public ObjectList getObjectList(GameObject gameObject, bool createIfNotExist = true)
        { 
            ObjectList objectList = null;
            objectListDictionary.TryGetValue(gameObject.scene, out objectList);
            
            if (objectList == null && createIfNotExist)
            {         
                objectList = createObjectList(gameObject);
                if (gameObject.scene != objectList.gameObject.scene) EditorSceneManager.MoveGameObjectToScene(objectList.gameObject, gameObject.scene);
                objectListDictionary.Add(gameObject.scene, objectList);
            }

            return objectList;
        }

        public bool isSceneChanged()
        {
            if (lastActiveScene != EditorSceneManager.GetActiveScene() || lastSceneCount != SceneManager.loadedSceneCount)
                return true;
            else 
                return false;
        }

        #else

        public void update()
        {
            try
            {  
                List<QObjectList> objectListList = QObjectList.instances;
                int objectListCount = objectListList.Count;
                if (objectListCount > 0) 
                {
                    if (objectListCount > 1)
                    {
                        for (int i = objectListCount - 1; i > 0; i--)
                        {
                            objectListList[0].merge(objectListList[i]);
                            GameObject.DestroyImmediate(objectListList[i].gameObject);
                        }
                    }

                    QObjectList objectList = QObjectList.instances[0];
                    setupObjectList(objectList);

                    if (( showObjectList && ((objectList.gameObject.hideFlags & HideFlags.HideInHierarchy)  > 0)) ||
                        (!showObjectList && ((objectList.gameObject.hideFlags & HideFlags.HideInHierarchy) == 0)))
                    {
                        objectList.gameObject.hideFlags ^= HideFlags.HideInHierarchy; 
                        EditorApplication.DirtyHierarchyWindowSorting();
                    }

                    if ((!Application.isPlaying && preventSelectionOfLockedObjects) || 
                        ((Application.isPlaying && preventSelectionOfLockedObjectsDuringPlayMode))
                        && isSelectionChanged())
                    {
                        GameObject[] selections = Selection.gameObjects;
                        List<GameObject> actual = new List<GameObject>(selections.Length);
                        bool found = false;
                        for (int i = selections.Length - 1; i >= 0; i--)
                        {
                            GameObject gameObject = selections[i];
                            
                            bool isLock = objectList.lockedObjects.Contains(gameObject);                        
                            if (!isLock) actual.Add(selections[i]);
                            else found = true;
                        }
                        if (found) Selection.objects = actual.ToArray();
                    }   
                }
            }
            catch 
            {
            }
        }

        public QObjectList getObjectList(GameObject gameObject, bool createIfNotExists = false)
        { 
            List<QObjectList> objectListList = QObjectList.instances;
            int objectListCount = objectListList.Count;
            if (objectListCount != 1)
            {
                if (objectListCount == 0) 
                {
                    if (createIfNotExists)
                    {
                        createObjectList(gameObject);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
                
            return QObjectList.instances[0];
        }

        #endif

        private ObjectList createObjectList(GameObject gameObject)
        {
            GameObject gameObjectList = new GameObject();
            gameObjectList.name = HierarchyObjectListName;
            ObjectList objectList = gameObjectList.AddComponent<ObjectList>();
            setupObjectList(objectList);
            return objectList;
        }

        private void setupObjectList(ObjectList objectList)
        {
            if (objectList.tag == "EditorOnly") objectList.tag = "Untagged";
            MonoScript monoScript = MonoScript.FromMonoBehaviour(objectList);
            if (MonoImporter.GetExecutionOrder(monoScript) != -10000)                    
                MonoImporter.SetExecutionOrder(monoScript, -10000);
        }
    }
}

