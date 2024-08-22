using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPScriptingDefineSymbolsDrawer
    {
        private static Vector2 scroll = Vector2.zero;

        public static void OnDrawScriptingDefineSymbols()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            CPUtility.DrawHeaderIcon(StatePanelControl.ScriptDefineSymbols, "Scripting Define Symbols");
            GUILayout.Space(10);
            scroll = EditorGUILayout.BeginScrollView(scroll);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_ADS);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_APPLOVIN);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_ADMOB);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_IRONSOURCE);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_ADJUST);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_FIREBASE);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_FIREBASE_ANALYTIC);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_FIREBASE_REMOTECONFIG);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_IAP);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_RATING);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_NOTIFICATION);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_APPSFLYER);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.PRIME_TWEEN_DOTWEEN_ADAPTER);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_APPLE_AUTH);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_GPGS);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_SKELETON);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_ANIMANCER);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.UNITASK_ADDRESSABLE_SUPPORT);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.UNITASK_DOTWEEN_SUPPORT);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.UNITASK_TEXTMESHPRO_SUPPORT);
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}