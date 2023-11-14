using System;
using System.Reflection;
using UnityEditor;

namespace VirtueSky.UtilsEditor
{
#if UNITY_EDITOR
    public class GameViewUtils
    {
        public const int DefaultSizeCount = 18;

        public static readonly Resolution[] resolutions =
        {
            new Resolution("iPhone 4", 640, 960),
            new Resolution("iPhone 5", 640, 1136),
            new Resolution("iPhone 6", 750, 1334),
            new Resolution("iPhone 8+", 1242, 2208),
            new Resolution("iPhone X", 1125, 2436),
            new Resolution("iPhone Xs Max", 1242, 2688),
            new Resolution("iPhone XR ", 828, 1792),
            new Resolution("HD", 1080, 1920),
            new Resolution("iPad Retina", 1536, 2048),
            new Resolution("iPad Pro 10.5", 1668, 2224),
            new Resolution("iPad Pro 12.9", 2048, 2732),
            new Resolution("iPhone 11 Pro", 1125, 2436),
            new Resolution("iPhone 11 Pro Max", 1242, 2688)
        };

        static readonly object gameViewSizesInstance;
        static readonly MethodInfo getGroup;

        static GameViewUtils()
        {
            var sizesType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
            var singleType = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
            var instanceProp = singleType.GetProperty("instance");
            getGroup = sizesType.GetMethod("GetGroup");
            gameViewSizesInstance = instanceProp.GetValue(null, null);
        }

        public static void AddCustomSize()
        {
            foreach (var resolution in resolutions)
                AddCustomSize(GameViewSizeGroupType.Android, resolution.width, resolution.height, resolution.name);
        }

        static void AddCustomSize(GameViewSizeGroupType sizeGroupType, int width, int height, string text)
        {
            var group = GetGroup(sizeGroupType);
            var addCustomSize = getGroup.ReturnType.GetMethod("AddCustomSize");
            var gvsType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GameViewSize");
            var ctor = gvsType.GetConstructor(new[]
            {
                typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GameViewSizeType"), typeof(int), typeof(int),
                typeof(string)
            });
            var newSize = ctor.Invoke(new object[] { 1, width, height, text });
            addCustomSize.Invoke(group, new[] { newSize });
        }

        public static void SetSize(int index)
        {
            var gvWndType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
            var gvWnd = EditorWindow.GetWindow(gvWndType);
            var sizeSelectionCallback = gvWndType.GetMethod("SizeSelectionCallback",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            sizeSelectionCallback.Invoke(gvWnd, new object[] { index, null });
        }

        static object GetGroup(GameViewSizeGroupType type)
        {
            return getGroup.Invoke(gameViewSizesInstance, new object[] { (int)type });
        }

        public static int GetViewListSize()
        {
            var group = GetGroup(GameViewSizeGroupType.Android);
            var getDisplayTexts = group.GetType().GetMethod("GetDisplayTexts");
            return (getDisplayTexts.Invoke(group, null) as string[]).Length;
        }
    }
#endif


    [Serializable]
    public class Resolution
    {
        public string name;
        public int height;
        public int width;

        public Resolution(string name, int width, int height)
        {
            this.name = name;
            this.width = width;
            this.height = height;
        }
    }
}