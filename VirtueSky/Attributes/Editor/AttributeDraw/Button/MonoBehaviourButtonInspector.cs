using UnityEngine;
using UnityEditor;


namespace VirtueSky.Attributes
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MonoBehaviourButtonInspector : Editor
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
            mButtonDrawer.Draw();
            buttonShowIfDrawer.Draw();
        }
    }
}