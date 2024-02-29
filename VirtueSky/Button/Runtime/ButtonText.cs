using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Inspector;

namespace VirtueSky.UIButton
{
    [RequireComponent(typeof(Text))]
    [EditorIcon("icon_button")]
    public class ButtonText : ButtonCustom
    {
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            var label = GetComponent<Text>();
            label.color = new Color(255, 255, 255, 255);
            label.text = "Button_Text";
            label.fontSize = 50;
            label.alignment = TextAnchor.MiddleCenter;
            var rect = GetComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.position = Vector3.zero;
            rect.sizeDelta = new Vector2(300, 80);
        }
#endif
    }
}