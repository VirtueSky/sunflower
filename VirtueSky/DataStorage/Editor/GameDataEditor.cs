using UnityEditor;
using VirtueSky.Inspector;

public partial class GameDataEditor
{
    public static CustomColor ColorContentWindowSunflower
    {
        get => (CustomColor)EditorPrefs.GetInt("ColorContent_Windows_Sunflower",
            (int)CustomColor.Bright);
        set => EditorPrefs.SetInt("ColorContent_Windows_Sunflower", (int)value);
    }

    public static CustomColor ColorTextContentWindowSunflower
    {
        get => (CustomColor)EditorPrefs.GetInt("ColorTextContent_Windows_Sunflower",
            (int)CustomColor.Gold);
        set => EditorPrefs.SetInt("ColorTextContent_Windows_Sunflower", (int)value);
    }

    public static CustomColor ColorBackgroundRectWindowSunflower
    {
        get => (CustomColor)EditorPrefs.GetInt("ColorBackground_Windows_Sunflower",
            (int)CustomColor.DarkSlateGray);
        set => EditorPrefs.SetInt("ColorBackground_Windows_Sunflower", (int)value);
    }
}