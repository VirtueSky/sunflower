using UnityEngine;

namespace VirtueSky.Inspector
{
    public static class ColorExtensions
    {
        // Convert the TitleColor enum to an actual Color32
        public static Color32 ToColor(this CustomColor color)
        {
            switch (color)
            {
                case CustomColor.Aqua: return new Color32(127, 219, 255, 255);
                case CustomColor.Beige: return new Color32(245, 245, 220, 255);
                case CustomColor.Black: return new Color32(0, 0, 0, 255);
                case CustomColor.Blue: return new Color32(31, 133, 221, 255);
                case CustomColor.BlueVariant: return new Color32(67, 110, 238, 255);
                case CustomColor.DarkBlue: return new Color32(41, 41, 225, 255);
                case CustomColor.Bright: return new Color32(196, 196, 196, 255);
                case CustomColor.Brown: return new Color32(148, 96, 59, 255);
                case CustomColor.Cyan: return new Color32(0, 255, 255, 255);
                case CustomColor.DarkGray: return new Color32(36, 36, 36, 255);
                case CustomColor.Fuchsia: return new Color32(240, 18, 190, 255);
                case CustomColor.Gray: return new Color32(88, 88, 88, 255);
                case CustomColor.Green: return new Color32(98, 200, 79, 255);
                case CustomColor.Indigo: return new Color32(75, 0, 130, 255);
                case CustomColor.LightGray: return new Color32(128, 128, 128, 255);
                case CustomColor.Lime: return new Color32(1, 255, 112, 255);
                case CustomColor.Navy: return new Color32(15, 35, 86, 255);
                case CustomColor.Olive: return new Color32(61, 153, 112, 255);
                case CustomColor.DarkOlive: return new Color32(47, 79, 79, 255);
                case CustomColor.Orange: return new Color32(255, 128, 0, 255);
                case CustomColor.OrangeVariant: return new Color32(255, 135, 62, 255);
                case CustomColor.Pink: return new Color32(255, 152, 203, 255);
                case CustomColor.Red: return new Color32(234, 42, 42, 255);
                case CustomColor.LightRed: return new Color32(217, 71, 71, 255);
                case CustomColor.RedVariant: return new Color32(232, 10, 10, 255);
                case CustomColor.DarkRed: return new Color32(144, 20, 39, 255);
                case CustomColor.Tan: return new Color32(210, 180, 140, 255);
                case CustomColor.Teal: return new Color32(27, 126, 126, 255);
                case CustomColor.Violet: return new Color32(181, 93, 237, 255);
                case CustomColor.White: return new Color32(255, 255, 255, 255);
                case CustomColor.Yellow: return new Color32(255, 211, 0, 255);
                case CustomColor.Aquamarine: return new Color32(127, 255, 212, 255);
                case CustomColor.Crimson: return new Color32(220, 20, 60, 255);
                case CustomColor.LightGreen: return new Color32(144, 238, 144, 255);
                case CustomColor.SkyBlue: return new Color32(135, 206, 235, 255);
                default: return new Color32(0, 0, 0, 0);
            }
        }
    }
}