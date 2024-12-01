using System;
using UnityEditor;
using UnityEngine;

namespace Library.PackageCache.com.oikoume.obsidity
{
    public class ObsidityEditorWindow : EditorWindow
    {
        private const float SuccessDisplayTime = 5.0f;

        private const string InputFieldControlName = "MyInputField";
        private static double _startTime;

        private bool _allowKeyboardSave = true;

        // unity event
        public void OnGUI()
        {
            // show any information if required
            ShowInformation();
            // disable editor input if not initialzied
            using (new EditorGUI.DisabledScope(!ObsidityMain.IsInitialized()))
            {
                GUILayout.Label("Obsidity", EditorStyles.boldLabel);

                InputEventHandler();

                // title and tag input
                var tContent = new GUIContent("Title: ", "This is the title at the top of your note.");
                var tagsContent = new GUIContent("Tags: ", "Space separated list of tags.");
                _textTitle = ObsidityEditorHelper.DrawTextField(tContent, _textTitle);
                _textTags = ObsidityEditorHelper.DrawTextField(tagsContent, _textTags);
                //display date and text area
                GUILayout.Label("Date:" + DateTime.Now.ToString("yyyy-MM-dd "));
                // content window
                DrawTextContentArea();
                // save/clear buttons
                SaveClearButtons();
            }

            return;

            void DrawTextContentArea()
            {
                GUILayout.Label("Text Area:");
                var textAreaStyle = new GUIStyle(GUI.skin.textArea)
                {
                    fontSize = 25, // Set desired font size
                    wordWrap = true // Enable word wrap
                };
                GUI.SetNextControlName(InputFieldControlName);
                _textContent =
                    EditorGUILayout.TextArea(_textContent, textAreaStyle, GUILayout.Height(50),
                        GUILayout.ExpandHeight(true));
            }

            void SaveClearButtons()
            {
                GUILayout.BeginHorizontal();
                // quick inversion lol
                var canSave = _textTitle.Length > 0 &&
                              _textTags.Length > 0 &&
                              _textContent.Length > 0;
                using (new EditorGUI.DisabledScope(!canSave))
                {
                    if (GUILayout.Button("Save"))
                        SaveAndResetForm();
                }

                if (GUILayout.Button("Clear"))
                    ResetInputForm();

                GUILayout.EndHorizontal();
            }

            void ShowInformation()
            {
                // if any of the conditions below are true,
                // trigger a timer that removes the information
                // box after a set time
                var anyConditionTrue = false;
                if (!ObsidityMain.IsInitialized())
                {
                    // info about initialization state
                    anyConditionTrue = true;
                    EditorGUILayout.HelpBox(ObsidityStrings.NotInitializedError, MessageType.Error);
                }

                if (_showEmptyError)
                {
                    // info about empty input fields
                    anyConditionTrue = true;
                    EditorGUILayout.HelpBox(ObsidityStrings.EmptyError, MessageType.Warning);
                }

                if (_showSaveError)
                {
                    // save error
                    anyConditionTrue = true;
                    EditorGUILayout.HelpBox(ObsidityStrings.SaveError, MessageType.Warning);
                }

                if (_showSaveSuccess)
                {
                    // show saved filename
                    anyConditionTrue = true;
                    var vaultName = ObsidityPlayerPrefs.GetString(ObsidityPlayerPrefsKeys.VaultName);
                    var index = ObsidityPlayerPrefs.GetInt(ObsidityPlayerPrefsKeys.FileNameIndex);
                    EditorGUILayout.HelpBox(ObsidityStrings.SaveSuccess + $"{vaultName}_{index:D5}.md",
                        MessageType.Info);
                }

                // trigger timer
                if (anyConditionTrue)
                    RemoveHelpBoxTimer();
            }
        }

        /// <summary>
        ///     listens for shift+enter if the editor content window has focus
        /// </summary>
        private void InputEventHandler()
        {
            _allowKeyboardSave = GUILayout.Toggle(_allowKeyboardSave, "Save on Shift+Enter");
            if (!_allowKeyboardSave)
                return;
            // listens for shift+enter to save
            var e = Event.current;
            if (GUI.GetNameOfFocusedControl() != InputFieldControlName)
                return;
            if (e.type != EventType.KeyDown)
                return;
            if (Event.current.keyCode != KeyCode.Return || !Event.current.shift) return;
            Event.current.Use();
            SaveAndResetForm();
        }

        private static void RemoveHelpBoxTimer()
        {
            // assigns UpdateTimer to unityEditor.update loop
            _startTime = EditorApplication.timeSinceStartup;
            EditorApplication.update += UpdateTimer;
        }


        // timer
        private static void UpdateTimer()
        {
            var elapsed = EditorApplication.timeSinceStartup - _startTime;
            if (elapsed >= SuccessDisplayTime)
            {
                // reset state on info
                _showEmptyError = false;
                _showSaveError = false;
                _showSaveSuccess = false;
                // Queue a GUI update without repainting immediately
                if (EditorApplication.isFocused)
                    EditorApplication.delayCall += () => GetWindow<ObsidityEditorWindow>().Repaint();
                // unReg from update loop
                EditorApplication.update -= UpdateTimer;
            }
        }

        [MenuItem("Window/Obsidity/Obsidity Editor")]
        public static void ShowWindow()
        {
            // shows the editorWindow  (unity event)
            GetWindow<ObsidityEditorWindow>();
        }

        public static ObsidityEditorWindow ShowAndGetWindow()
        {
            //shows the window and returns a ref to it
            ShowWindow();
            return GetWindow<ObsidityEditorWindow>();
        }


        private void SaveAndResetForm()
        {
            // make sure input are not empty
            if (_textContent.Length == 0 || _textTags.Length == 0 || _textTitle.Length == 0)
            {
                _showEmptyError = true;
                return;
            }

            var dt = DateTime.Now.ToString("yyyy-MM-dd");
            // create data
            var data = new ObsidityData(_textContent, dt, _textTags, _textTitle);
            var success = ObsidityMain.SaveMarkdownFile(data);
            // show save state info/error
            if (!success)
            {
                _showSaveError = true;
            }
            else
            {
                _showSaveSuccess = true;
                ResetInputForm();
            }

            // refresh UI
            Repaint();
        }


        public void ResetInputForm()
        {
            // clears out all input and removes focus from panel then repaints
            _textContent = "";
            _textTags = "";
            _textTitle = "";
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