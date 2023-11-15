using UnityEditor;
using System.Collections.Generic;

namespace VirtueSky.Attributes
{
    public static class EButtonDrawerExtension
    {
        private static Dictionary<Editor, ButtonDrawer> s_Drawers = new Dictionary<Editor, ButtonDrawer>();

        private static ButtonDrawer GetDrawer(Editor editor)
        {
            ButtonDrawer drawer;
            if (!s_Drawers.TryGetValue(editor, out drawer))
            {
                drawer = new ButtonDrawer(editor.target);
                s_Drawers.Add(editor, drawer);
            }

            return drawer;
        }

        public static void DrawEButtons(this Editor editor)
        {
            GetDrawer(editor).Draw();
        }
    }
}