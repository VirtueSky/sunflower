using UnityEngine;
using UnityEditor;


namespace VirtueSky.Attributes
{
    [CustomEditor(typeof(Object), true)]
    public class CustomizeDrawInspector : Editor
    {
        public ButtonDrawer mButtonDrawer;
        public ButtonShowIfDrawer buttonShowIfDrawer;

        public void OnEnable()
        {
            mButtonDrawer = new ButtonDrawer(target);
            buttonShowIfDrawer = new ButtonShowIfDrawer(target);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (mButtonDrawer != null)
            {
                mButtonDrawer.Draw();
            }

            if (buttonShowIfDrawer != null)
            {
                buttonShowIfDrawer.Draw();
            }
        }
    }
}