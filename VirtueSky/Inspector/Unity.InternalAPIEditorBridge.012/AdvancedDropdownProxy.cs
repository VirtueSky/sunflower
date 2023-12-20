using UnityEditor.IMGUI.Controls;

namespace VirtueSky.InspectorUnityInternalBridge
{
    public class AdvancedDropdownProxy
    {
        public static void SetShowHeader(AdvancedDropdown dropdown, bool showHeader)
        {
            dropdown.m_WindowInstance.showHeader = showHeader;
        }
    }
}