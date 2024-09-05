using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Hierarchy
{
    [ExecuteInEditMode]
    [EditorIcon("icon_hierarchy"), HideMonoScript]
    public class HeaderHierarchy : MonoBehaviour
    {
        #region Variables

        public enum TextAlignment
        {
            Left,
            Center
        }

        [TitleColor("Text", CustomColor.Gold, CustomColor.Aqua)] [SerializeField]
        public bool customText;

        [ShowIf(nameof(customText)), SerializeField]
        public Color32 textColor = InspectorUtility.textNormalColor;

        [Space] [SerializeField] public FontStyle textStyle = FontStyle.BoldAndItalic;

        [SerializeField] public TextAlignment textAlignment = TextAlignment.Left;

        [TitleColor("Highlight", CustomColor.Coral, CustomColor.Lime)] [Space, SerializeField]
        public bool customHighlight;

        [ShowIf(nameof(customHighlight)), SerializeField]
        public Color32 highlightColor = InspectorUtility.cyanColor;

        #endregion
    } // class end
}