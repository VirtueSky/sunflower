using UnityEngine;
using UnityEditor;


namespace VirtueSky.Attributes
{
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class MonoBehaviourInspector : Editor
    {
        public ButtonDrawer mButtonDrawer;

        public void OnEnable()
        {
            mButtonDrawer = new ButtonDrawer(target);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            mButtonDrawer.Draw();
        }
    }
}