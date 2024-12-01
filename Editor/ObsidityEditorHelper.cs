using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class ObsidityEditorHelper
    {
        public static GUIStyle WarningStyle = new(GUI.skin.label)
        {
            normal =
            {
                textColor = Color.yellow
            },
            fontStyle = FontStyle.Bold
        };
        

        public static GUIStyle ErrorStyle = new(GUI.skin.label)
        {
            normal =
            {
                textColor = Color.red
            },
            fontStyle = FontStyle.Bold
        };

        // helper method for drawing textField
        public static string DrawTextField(string label, string textContent)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.25f));
            textContent =
                EditorGUILayout.TextField(textContent, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.75f));
            GUILayout.EndHorizontal();
            return textContent;
        }

        // override helper method for drawing textField
        public static string DrawTextField(GUIContent label, string textContent)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.25f));
            textContent =
                EditorGUILayout.TextField(textContent, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();
            return textContent;
        }

        public static bool DrawToggle(string label, bool toggle)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.25f));
            GUILayout.FlexibleSpace(); // Pushes following elements to the right
            var t = GUILayout.Toggle(toggle, GUIContent.none, GUILayout.Width(20));
            GUILayout.EndHorizontal();
            return t;
        }
    }

    public static class ObsidityExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            return input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1).ToArray())
            };
        }
    }
}