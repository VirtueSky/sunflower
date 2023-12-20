using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using System.Reflection;
using System.Linq;

namespace VirtueSky.Inspector
{
    [CustomPropertyDrawer(typeof(ExtendEnumAttribute))]
    public class ExtendEnumDrawer : PropertyDrawer
    {
        //Some static values to pass back and forth with the popup
        static string newValueText = "";
        static SerializedProperty currentProperty = null;
        static bool showWindow = false;
        static List<string> enumNames;
        static int popupWidth = 150;
        static int popupHeight = 90;

        //Our class to make the popup
        public class NewValuePopup : PopupWindowContent
        {
            public override void OnGUI(Rect position)
            {
                EditorGUILayout.LabelField("New Value", EditorStyles.wordWrappedLabel);

                newValueText = GUILayout.TextField(newValueText).Trim();

                float exitSize = 18;
                if (GUI.Button(new Rect(position.width - exitSize, 0, exitSize, exitSize), "X"))
                {
                    this.editorWindow.Close();
                }

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Confirm"))
                {
                    List<string> names = enumNames;
                    for (int i = 0; i < names.Count; i++)
                    {
                        names[i] = names[i].ToLower();
                    }

                    if (!names.Contains(newValueText.ToLower()))
                    {
                        //This sends our enum to go get created. Be safe little enum.
                        FindClassFile(GetEnumName(currentProperty), newValueText);
                        this.editorWindow.Close();
                    }
                    else
                    {
                        newValueText = newValueText + "_Copy";
                    }
                }

                if (GUILayout.Button("Cancel"))
                {
                    this.editorWindow.Close();
                }

                GUILayout.EndHorizontal();
                GUIStyle small = new GUIStyle(EditorStyles.wordWrappedLabel);
                small.fontSize = 9;
                EditorGUILayout.LabelField("(To add multiple, separate with commas)", small);
            }

            public override void OnOpen()
            {
                showWindow = true;
                newValueText = "";
                base.OnOpen();
            }

            public override void OnClose()
            {
                showWindow = false;
                base.OnClose();
            }

            public override Vector2 GetWindowSize()
            {
                return new Vector2(popupWidth, popupHeight);
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            currentProperty = property;
            ExtendEnumAttribute source = (ExtendEnumAttribute)attribute;
            System.Enum enumVal = GetBaseProperty<System.Enum>(property);

            enumNames = (property.enumDisplayNames).OfType<string>().ToList();
            if (source.display)
            {
                int[] enumValues = (int[])(System.Enum.GetValues(enumVal.GetType()));
                for (int i = 0; i < enumNames.Count; i++)
                {
                    enumNames[i] += " | " + enumValues[i];
                }
            }

            EditorGUI.BeginProperty(position, label, property);
            if (!showWindow)
            {
                if (enumNames.Count != 0)
                {
                    enumNames.Add("Add New...");
                    int newValue = EditorGUI.Popup(position, property.displayName, property.intValue, enumNames.ToArray());

                    if (newValue == enumNames.Count - 1)
                    {
                        NewValuePopup popup = new NewValuePopup();
                        PopupWindow.Show(new Rect(Screen.width / 2 - popupWidth / 2, position.y - popupHeight / 2, 0, 0), popup);

                        newValueText = "";
                    }
                    else
                    {
                        property.intValue = newValue;
                    }
                }
                else
                {
                    EditorGUI.LabelField(position, "Extendable Enums needs at least one value in your declared enum.");
                }
            }
            else
            {
                EditorGUI.LabelField(position, "Waiting for new value input.");
            }

            EditorGUI.EndProperty();
        }

        //I know this is pretty much the same as GetBaseProperty, I was lazy, bite me.
        static string GetEnumName(SerializedProperty prop)
        {
            string[] separatedPaths = prop.propertyPath.Split('.');
            System.Object reflectionTarget = prop.serializedObject.targetObject as object;

            foreach (var path in separatedPaths)
            {
                FieldInfo fieldInfo = reflectionTarget.GetType().GetField(path, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                string name = fieldInfo.FieldType.Name;
                return name;
            }

            return "Null";
        }

        static T GetBaseProperty<T>(SerializedProperty prop)
        {
            string[] separatedPaths = prop.propertyPath.Split('.');
            System.Object reflectionTarget = prop.serializedObject.targetObject as object;

            foreach (var path in separatedPaths)
            {
                FieldInfo fieldInfo = reflectionTarget.GetType().GetField(path, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                reflectionTarget = fieldInfo.GetValue(reflectionTarget);
            }

            return (T)reflectionTarget;
        }

        static void FindClassFile(string enumName, string newEnum)
        {
            KeyValuePair<string, string> codeFile = FindAllScriptFiles(Application.dataPath, "enum " + enumName);
            if (codeFile.Key != "NOPE")
                AddNewEnum(codeFile.Key, codeFile.Value, enumName, newEnum);
            else
                Debug.LogError("Could not find enum class");
        }

        //Formatting is a little weird on this depending. It works pretty well with a single line enum format, but other than that it can do weird shit.
        //But it still works, so it's fine... for now.
        static void AddNewEnum(string classFile, string path, string enumName, string newEnum)
        {
            string[] originalSplit = classFile.Split(new[] { "enum " + enumName }, System.StringSplitOptions.RemoveEmptyEntries);
            string newHalf = originalSplit[1];
            string enumSection = newHalf.Split('}')[0];
            string[] commas = enumSection.Split(',');
            if (commas.Length == 0 && enumSection.Split('{')[0].Trim().Length == 0) //They've left the enum empty... for some reason.
            {
                Debug.Log("Uhh idk yet");
                newHalf = newHalf.Replace(enumSection, enumSection + newEnum);
            }
            else
            {
                bool commaAfter = commas[commas.Length - 1].Trim().Length == 0; //This should check if the weirdo added a comma after their last enum value.

                if (commaAfter)
                {
                    newHalf = newHalf.Replace(enumSection, enumSection + newEnum + ", ");
                }
                else
                {
                    while (enumSection.Length > 0 && enumSection[enumSection.Length - 1] == ' ')
                        enumSection = enumSection.Substring(0, enumSection.Length - 1);
                    newHalf = newHalf.Replace(enumSection, enumSection + ", " + newEnum);
                }
            }

            string result = classFile.Replace(originalSplit[1], newHalf);
            using (var file = File.Open(path, FileMode.Create))
            {
                using (var writer = new StreamWriter(file))
                {
                    writer.Write(result);
                }
            }

            AssetDatabase.Refresh();
        }

        static KeyValuePair<string, string> FindAllScriptFiles(string startDir, string enumToFind)
        {
            try
            {
                foreach (string file in Directory.GetFiles(startDir))
                {
                    if ((file.Contains(".cs") || file.Contains(".js")) && !file.Contains(".meta"))
                    {
                        string current = File.ReadAllText(file);
                        string currentTrimmed = current.Replace(" ", "").Replace("\n", "").Replace("\t", "").Replace("\r", "");
                        if (currentTrimmed.Contains(enumToFind.Replace(" ", "") + "{"))
                            return new KeyValuePair<string, string>(current, file);
                    }
                }

                foreach (string dir in Directory.GetDirectories(startDir))
                {
                    KeyValuePair<string, string> result = FindAllScriptFiles(dir, enumToFind);
                    if (result.Key != "NOPE")
                        return result;
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }

            return new KeyValuePair<string, string>("NOPE", "NOPE");
        }
    }
}