using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Inspector;

namespace VirtueSky.UIButton
{
    [RequireComponent(typeof(Image))]
    [EditorIcon("icon_button")]
    public class ButtonUI_TMP : ButtonCustom
    {
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            var label = new GameObject("Label");
            label.transform.SetParent(this.transform);
            var text = label.AddComponent<TextMeshProUGUI>();
            text.color = new Color(0, 0, 0, 255);
            text.text = "Label Button";
            text.horizontalAlignment = HorizontalAlignmentOptions.Center;
            text.verticalAlignment = VerticalAlignmentOptions.Middle;
            var rectTransform = label.GetComponent<RectTransform>();
            var rectTransformParent = GetComponent<RectTransform>();
            rectTransform.localScale = rectTransformParent.localScale;
            rectTransform.position = rectTransformParent.position;
        }
#endif
    }
}