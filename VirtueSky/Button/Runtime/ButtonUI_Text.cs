using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Inspector;

namespace VirtueSky.UIButton
{
    [RequireComponent(typeof(Image))]
    [EditorIcon("icon_button")]
    public class ButtonUI_Text : ButtonCustom
    {
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            var labelObj = new GameObject("Label");
            labelObj.transform.SetParent(transform);
            var label = labelObj.AddComponent<Text>();
            label.color = new Color(0, 0, 0, 255);
            label.text = "Button_Text";
            label.fontSize = 50;
            label.alignment = TextAnchor.MiddleCenter;
            var rect = labelObj.GetComponent<RectTransform>();
            var rectParent = GetComponent<RectTransform>();
            rect.localScale = rectParent.localScale;
            rect.position = rectParent.position;
        }
#endif
    }
}