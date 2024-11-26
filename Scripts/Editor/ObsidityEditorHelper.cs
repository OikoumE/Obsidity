using UnityEditor;
using UnityEngine;

namespace Obsidity.Scripts.Editor
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

        public static string DrawTextField(string label, string textContent)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.25f));
            textContent =
                EditorGUILayout.TextField(textContent, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.75f));
            GUILayout.EndHorizontal();
            return textContent;
        }

        public static string DrawTextField(GUIContent label, string textContent)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.25f));
            textContent =
                EditorGUILayout.TextField(textContent, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.75f));
            GUILayout.EndHorizontal();
            return textContent;
        }
    }
}