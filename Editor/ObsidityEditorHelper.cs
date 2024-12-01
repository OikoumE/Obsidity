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
                { textColor = Color.yellow },
            fontStyle = FontStyle.Bold
        };


        public static GUIStyle ErrorStyle = new(GUI.skin.label)
        {
            normal =
                { textColor = Color.red },
            fontStyle = FontStyle.Bold
        };

        // helper method for drawing textField
        public static string DrawTextField(string label, string textContent, int fontSize)
        {
            var textLabelStyle = new GUIStyle(GUI.skin.label) { fontSize = fontSize };
            var textAreaStyle = new GUIStyle(GUI.skin.textField) { fontSize = fontSize };
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, textLabelStyle, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.25f));
            textContent =
                EditorGUILayout.TextField(textContent, textAreaStyle,
                    GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.75f));
            GUILayout.EndHorizontal();
            return textContent;
        }

        // override helper method for drawing textField
        public static string DrawTextField(GUIContent label, string textContent, int fontSize)
        {
            var textLabelStyle = new GUIStyle(GUI.skin.label) { fontSize = fontSize };
            var textAreaStyle = new GUIStyle(GUI.skin.textField)
                { fontSize = fontSize, fixedHeight = textLabelStyle.lineHeight + textLabelStyle.padding.vertical };
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, textLabelStyle);
            GUILayout.FlexibleSpace(); // Pushes following elements to the right
            textContent =
                EditorGUILayout.TextField(textContent, textAreaStyle, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();
            return textContent;
        }

        public static bool DrawToggle(string label, bool toggle, int indent = 0)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(indent);
            GUILayout.Label(label);
            GUILayout.FlexibleSpace(); // Pushes following elements to the right
            var t = GUILayout.Toggle(toggle, GUIContent.none, GUILayout.Width(20));
            GUILayout.EndHorizontal();
            return t;
        }

        public static int DrawIntSlider(string label, int currVal, int indent)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(indent);
            GUILayout.Label(label);
            var newInt = EditorGUILayout.IntSlider(currVal, 12, 35, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();

            return newInt;
        }
    }

    public static class ObsidityExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
            var charArr = input.ToCharArray();
            charArr[0] = char.ToUpper(charArr[0]);
            return new string(charArr);
        }
    }
}