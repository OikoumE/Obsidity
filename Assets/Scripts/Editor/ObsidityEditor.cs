using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ObsidityEditor : EditorWindow
    {
        private readonly string _emptyError = "Please fill out all the fields.";
        private readonly string _saveError = "lol couldnt save";
        private readonly bool _saveSuccess = false;
        private bool _showEmptyError;
        private bool _showSaveError;
        private string _textContent = "";
        private string _textDate = "";
        private string _textTags = "";
        private string _textTitle = "";

        public void OnGUI()
        {
            GUILayout.Label("Obsidity Editor", EditorStyles.boldLabel);
            _textTitle = DrawTextField("Title:", _textTitle);
            _textTags = DrawTextField("Tags:", _textTags);
            GUILayout.Label("Date:" + DateTime.Now.ToString("dd/MM/yy HH:mm"));

            GUILayout.Label("Text Area:");
            _textContent = EditorGUILayout.TextArea(_textContent, GUILayout.Height(50), GUILayout.ExpandHeight(true));
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save")) SaveAndResetForm();
            if (GUILayout.Button("Clear")) ResetInputForm();
            GUILayout.EndHorizontal();
            ShowWarning();
            return;

            void ShowWarning()
            {
                var warningStyle = new GUIStyle(GUI.skin.label);
                warningStyle.normal.textColor = Color.red;
                warningStyle.fontStyle = FontStyle.Bold;

                if (_showEmptyError)
                    GUILayout.Label(_emptyError, warningStyle);
                if (_showSaveError)
                    GUILayout.Label(_saveError, warningStyle);
            }


            // serializedObject.Update();
            // EditorUtility.SetDirty(_target);
            // serializedObject.ApplyModifiedProperties();
        }

        private string DrawTextField(string label, string textContent)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.25f));
            textContent =
                EditorGUILayout.TextField(textContent, GUILayout.Width(EditorGUIUtility.currentViewWidth * 0.75f));
            GUILayout.EndHorizontal();
            return textContent;
        }

        [MenuItem("Window/Obsidity/Obsidity Editor")]
        public static void ShowWindow()
        {
            GetWindow<ObsidityEditor>();
        }

        private void SaveAndResetForm()
        {
            if (_textContent.Length == 0 || _textTags.Length == 0 || _textTitle.Length == 0)
            {
                _showEmptyError = true;
                return;
            }

            SaveAsMd();
            ResetInputForm();
        }

        private void SaveAsMd()
        {
            if (_saveSuccess)
                _showSaveError = true;
        }

        public void ResetInputForm()
        {
            _textContent = "";
            _textTags = "";
            _textTitle = "";
            _showEmptyError = false;
            GUI.FocusControl(null);
            Repaint();
        }
    }
}