using System;
using Obsidity.Scripts.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ObsidityEditorWindow : EditorWindow
    {
        private const float SuccessDisplayTime = 5.0f;
        private static double _startTime;

        public void OnGUI()
        {
            ShowInformation();
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
                if (GUILayout.Button("Save"))
                    SaveAndResetForm();
                if (GUILayout.Button("Clear"))
                    ResetInputForm();
                GUILayout.EndHorizontal();
            }

            return;

            void ShowInformation()
            {
                var anyConditionTrue = false;
                if (!ObsidityMain.IsInitialized())
                {
                    anyConditionTrue = true;
                    EditorGUILayout.HelpBox(ObsidityStrings.NotInitializedError, MessageType.Error);
                }

                if (_showEmptyError)
                {
                    anyConditionTrue = true;
                    EditorGUILayout.HelpBox(ObsidityStrings.EmptyError, MessageType.Warning);
                }

                if (_showSaveError)
                {
                    anyConditionTrue = true;
                    EditorGUILayout.HelpBox(ObsidityStrings.SaveError, MessageType.Warning);
                }

                if (_showSaveSuccess)
                {
                    anyConditionTrue = true;
                    var vaultName = ObsidityPlayerPrefs.GetString(ObsidityPlayerPrefsKeys.VaultName);
                    var index = ObsidityPlayerPrefs.GetInt(ObsidityPlayerPrefsKeys.FileNameIndex);
                    EditorGUILayout.HelpBox(ObsidityStrings.SaveSuccess + $"{vaultName}_{index:D5}.md",
                        MessageType.Info);
                }

                if (anyConditionTrue)
                    RemoveHelpBoxTimer();
            }
        }

        private static void RemoveHelpBoxTimer()
        {
            _startTime = EditorApplication.timeSinceStartup;
            EditorApplication.update += UpdateTimer;
        }


        private static void UpdateTimer()
        {
            var elapsed = EditorApplication.timeSinceStartup - _startTime;
            if (elapsed >= SuccessDisplayTime)
            {
                _showEmptyError = false;
                _showSaveError = false;
                _showSaveSuccess = false;
                EditorApplication.update -= UpdateTimer;
            }
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


            var dt = DateTime.Now.ToString("yyyy-MM-dd");
            var data = new ObsidityData(_textContent, dt, _textTags, _textTitle);
            var success = ObsidityMain.SaveMarkdownFile(data);
            if (!success)
                _showSaveError = true;
            else
                _showSaveSuccess = true;
            Repaint();
        }


        public void ResetInputForm()
        {
            _textContent = "";
            _textTags = "";
            _textTitle = "";
            _showEmptyError = false;
            _showSaveError = false;
            _showSaveSuccess = false;

            GUI.FocusControl(null);
            Repaint();
        }
#pragma warning disable CS0414
        private bool _saveSuccess;
        private static bool _showEmptyError;
        private static bool _showSaveError;
        private static bool _showSaveSuccess;
        private string _textContent = "";
        private string _textDate = "";
        private string _textTags = "";
        private string _textTitle = "";
#pragma warning restore CS0414
    }
}