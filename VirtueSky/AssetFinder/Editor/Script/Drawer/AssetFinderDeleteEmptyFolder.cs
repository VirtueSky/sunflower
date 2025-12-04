using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetFinderDeleteEmptyFolder : IRefDraw
    {
        private readonly IWindow _window;
        private bool _isProcessing;
        private float _progress;
        private List<string> _foldersToDelete = new List<string>();
        private List<string> _deletedFolders = new List<string>();
        private string _reportTitle;
        private int _currentIndex;

        internal AssetFinderDeleteEmptyFolder(IWindow window, Func<AssetFinderRefDrawer.Sort> getSort, Func<AssetFinderRefDrawer.Mode> getGroup)
        {
            _window = window;
        }

        public IWindow window => _window;
        public int ElementCount() => _deletedFolders.Count;

        public bool Draw(Rect rect)
        {
            GUI.BeginClip(rect);
            GUILayout.BeginArea(new Rect(0, 0, rect.width, rect.height));
            bool result = DrawLayout();
            GUILayout.EndArea();
            GUI.EndClip();
            return result;
        }

        public bool DrawLayout()
        {
            GUILayout.BeginVertical();
            if (_isProcessing)
            {
                DrawProgressBar();
            }
            else
            {
                if (GUILayout.Button("Delete All Empty Folders", AssetFinderTheme.Current.ActionButtonHeight))
                {
                    StartProcessing();
                }
                if (!string.IsNullOrEmpty(_reportTitle))
                {
                    EditorGUILayout.HelpBox(_reportTitle, MessageType.Info);
                }
                if (_deletedFolders.Count > 0)
                {
                    GUILayout.Space(10);
                    EditorGUILayout.LabelField("Deleted Folders:", EditorStyles.boldLabel);
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    foreach (var folder in _deletedFolders.OrderBy(f => f))
                    {
                        EditorGUILayout.LabelField(folder, EditorStyles.miniLabel);
                    }
                    GUILayout.EndVertical();
                }
            }
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            return true;
        }

        private void DrawProgressBar()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Deleting empty folders...", EditorStyles.boldLabel);
            Rect rect = AssetFinderTheme.Current.GetProgressBarRect();
            EditorGUI.ProgressBar(rect, _progress, _currentIndex < _foldersToDelete.Count ? _foldersToDelete[_currentIndex] : "Done");
            EditorGUILayout.Space();
            if (GUILayout.Button("Cancel", AssetFinderTheme.Current.CancelButtonHeight))
            {
                CancelProcessing();
            }
        }

        private void StartProcessing()
        {
            _isProcessing = true;
            _progress = 0f;
            _foldersToDelete.Clear();
            _deletedFolders.Clear();
            _reportTitle = null;
            _currentIndex = 0;
            FindAllEmptyFolders("Assets");
            _foldersToDelete = _foldersToDelete.Distinct().OrderByDescending(f => f.Length).ToList();
            EditorApplication.update -= ProcessNextFolder;
            EditorApplication.update += ProcessNextFolder;
        }

        private void CancelProcessing()
        {
            _isProcessing = false;
            EditorApplication.update -= ProcessNextFolder;
            _window.Repaint();
        }

        private void ProcessNextFolder()
        {
            if (!_isProcessing)
            {
                EditorApplication.update -= ProcessNextFolder;
                return;
            }
            if (_currentIndex >= _foldersToDelete.Count)
            {
                EditorApplication.update -= ProcessNextFolder;
                _isProcessing = false;
                GenerateReport();
                _window.Repaint();
                return;
            }
            string folder = _foldersToDelete[_currentIndex];
            _progress = _currentIndex / (float)_foldersToDelete.Count;
            if (IsFolderEmpty(folder))
            {
                AssetDatabase.DeleteAsset(folder);
                _deletedFolders.Add(folder);
            }
            _currentIndex++;
            _window.Repaint();
        }

        private void FindAllEmptyFolders(string root)
        {
            foreach (var dir in Directory.GetDirectories(root))
            {
                var dirName = Path.GetFileName(dir);
                if (dirName.StartsWith(".")) continue; // skip hidden folders
                FindAllEmptyFolders(dir.Replace("\\", "/"));
                if (IsUnityFolderEmpty(dir.Replace("\\", "/")))
                {
                    _foldersToDelete.Add(dir.Replace("\\", "/"));
                }
            }
        }

        private bool IsUnityFolderEmpty(string folder)
        {
            var files = Directory.GetFiles(folder);
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                if (fileName == null) continue;
                if (fileName.StartsWith(".")) return false; // hidden file
                if (!fileName.EndsWith(".meta", StringComparison.OrdinalIgnoreCase)) return false; // any non-meta file
            }
            var dirs = Directory.GetDirectories(folder);
            foreach (var dir in dirs)
            {
                var dirName = Path.GetFileName(dir);
                if (dirName.StartsWith(".")) return false; // hidden subfolder
                if (!IsUnityFolderEmpty(dir.Replace("\\", "/"))) return false;
            }
            return true;
        }

        private bool IsFolderEmpty(string folder)
        {
            var files = Directory.GetFiles(folder).Where(f => !f.EndsWith(".meta")).ToArray();
            var dirs = Directory.GetDirectories(folder);
            return files.Length == 0 && dirs.All(d => !AssetDatabase.IsValidFolder(d) || IsFolderEmpty(d.Replace("\\", "/")));
        }

        private void GenerateReport()
        {
            if (_deletedFolders.Count > 0)
            {
                _reportTitle = $"Deleted {_deletedFolders.Count} empty folder(s).";
            }
            else
            {
                _reportTitle = "No empty folders found.";
            }
        }

        // Public static method to delete all empty folders recursively from a root
        public static void DeleteAllEmptyFoldersRecursive(string root)
        {
            foreach (var dir in Directory.GetDirectories(root))
            {
                var dirName = Path.GetFileName(dir);
                if (dirName.StartsWith(".")) continue; // skip hidden folders
                DeleteAllEmptyFoldersRecursive(dir.Replace("\\", "/"));
                if (IsUnityFolderEmptyStatic(dir.Replace("\\", "/")))
                {
                    string relPath = dir.Replace("\\", "/");
                    if (relPath.StartsWith(Application.dataPath))
                    {
                        relPath = "Assets" + relPath.Substring(Application.dataPath.Length);
                    }
                    // Remove any double 'Assets' prefix
                    while (relPath.StartsWith("AssetsAssets/"))
                    {
                        relPath = relPath.Substring("Assets".Length);
                    }
                    // Ensure single 'Assets/' at the start
                    if (!relPath.StartsWith("Assets/"))
                    {
                        relPath = "Assets/" + relPath.TrimStart('/');
                    }
                    // File IO delete: delete folder and .meta file
                    string absPath = Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - 6), relPath); // Remove 'Assets' from Application.dataPath
                    if (Directory.Exists(absPath))
                    {
                        Directory.Delete(absPath, true);
                    }
                    string metaPath = absPath + ".meta";
                    if (File.Exists(metaPath))
                    {
                        File.Delete(metaPath);
                    }
                }
            }
        }
        private static bool IsUnityFolderEmptyStatic(string folder)
        {
            var files = Directory.GetFiles(folder);
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                if (fileName == null) continue;
                if (fileName.StartsWith(".")) return false; // hidden file
                if (!fileName.EndsWith(".meta", StringComparison.OrdinalIgnoreCase)) return false; // any non-meta file
            }
            var dirs = Directory.GetDirectories(folder);
            foreach (var dir in dirs)
            {
                var dirName = Path.GetFileName(dir);
                if (dirName.StartsWith(".")) return false; // hidden subfolder
                if (!IsUnityFolderEmptyStatic(dir.Replace("\\", "/"))) return false;
            }
            return true;
        }
    }
}