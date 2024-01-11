using UnityEditor;
using UnityEngine;
using VirtueSky.Rating;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPInAppReviewDrawer
    {
        public static void OnDrawInAppReview()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("IN APP REVIEW", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Create In App Review"))
            {
                RatingWindowEditor.CreateInAppReview();
            }

            GUILayout.EndVertical();
        }
    }
}