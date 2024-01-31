using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPScriptingDefineSymbolsDrawer
    {
        public static void OnDrawScriptingDefineSymbols()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("SCRIPTING DEFINE SYMBOLS", EditorStyles.boldLabel);
            GUILayout.Space(10);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_ADS);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.ADS_APPLOVIN);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.ADS_ADMOB);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_ADJUST);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_FIREBASE);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_FIREBASE_ANALYTIC);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_FIREBASE_REMOTECONFIG);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_IAP);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_RATING);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_NOTIFICATION);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_APPSFLYER);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.PRIME_TWEEN_DOTWEEN_ADAPTER);
            GUILayout.EndVertical();
        }
    }
}