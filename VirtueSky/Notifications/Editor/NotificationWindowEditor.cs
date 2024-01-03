using UnityEditor;
using VirtueSky.Notifications;
using VirtueSky.UtilsEditor;


public class NotificationWindowEditor : EditorWindow
{
    [MenuItem("Sunflower/Notification Channel")]
    public static void CreateNotificationChannel()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<NotificationVariable>("/Notifications");
    }
}