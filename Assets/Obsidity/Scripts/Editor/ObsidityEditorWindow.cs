using System;
using Obsidity.Scripts.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ObsidityEditorWindow : EditorWindow
    {
        public void OnGUI()
        {
            ShowWarnings();
            using (new EditorGUI.DisabledScope(!ObsidityMain.IsInitialized()))
            {
                GUILayout.Label("Obsidity Editor", EditorStyles.boldLabel);
                _textTitle = ObsidityEditorHelper.DrawTextField("Title:", _textTitle);
                _textTags = ObsidityEditorHelper.DrawTextField("Tags:", _textTags);
                GUILayout.Label("Date:" + DateTime.Now.ToString("dd/MM/yy HH:mm"));

                GUILayout.Label("Text Area:");
                _textContent =
                    EditorGUILayout.TextArea(_textContent, GUILayout.Height(50), GUILayout.ExpandHeight(true));
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Save")) SaveAndResetForm();
                if (GUILayout.Button("Clear")) ResetInputForm();
                GUILayout.EndHorizontal();
            }

            return;

            void ShowWarnings()
            {
                if (!ObsidityMain.IsInitialized())
                    EditorGUILayout.HelpBox(ObsidityStrings.NotInitializedError, MessageType.Error);
                if (_showEmptyError)
                    EditorGUILayout.HelpBox(ObsidityStrings.EmptyError, MessageType.Warning);
                if (_showSaveError)
                    EditorGUILayout.HelpBox(ObsidityStrings.SaveError, MessageType.Warning);
            }
            // serializedObject.Update();
            // EditorUtility.SetDirty(_target);
            // serializedObject.ApplyModifiedProperties();
        }


        [MenuItem("Window/Obsidity/Obsidity Editor")]
        public static void ShowWindow()
        {
            GetWindow<ObsidityEditorWindow>();
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
            if (!_saveSuccess)
                _showSaveError = true;
            var dt = DateTime.Now.ToString("dd/MM/yy HH:mm");
            var data = new ObsidityData(_textContent, dt, _textTags, _textTitle);
            //TODO save
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
#pragma warning disable CS0414
        private readonly bool _saveSuccess = false;
        private bool _showEmptyError;
        private bool _showSaveError;
        private string _textContent = "";
        private string _textDate = "";
        private string _textTags = "";
        private string _textTitle = "";
#pragma warning restore CS0414
    }
}