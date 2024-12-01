using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ObsidityWelcomeEditorWindow : EditorWindow
    {
        private string _vaultName = "";

        public void OnGUI()
        {
            // Shows window content depending on state
            GUILayout.Label("Welcome to Obsidity", EditorStyles.boldLabel);
            if (!ObsidityMain.IsInitialized())
                InitializeVault();
            else
                VaultIsInitialized();

            ObsiditySettings.DrawSettings();
        }

        /// <summary>
        ///     displayed in editor if vault is not initialized
        /// </summary>
        /// <returns></returns>
        private void InitializeVault()
        {
            GUILayout.Label("Please initialize the Obsidian Vault.");
            // name input
            var fontSize = ObsiditySettings.Get(ObsidityPlayerPrefsKeys.FontSize);
            _vaultName = ObsidityEditorHelper.DrawTextField("Vault Name", _vaultName, fontSize);
            if (_vaultName.Length == 0)
                EditorGUILayout.HelpBox(ObsidityStrings.SelectVaultName, MessageType.Warning);
            // disable button if name is empty
            using (new EditorGUI.DisabledScope(_vaultName.Length == 0))
            {
                // create new vault
                if (GUILayout.Button("Initialize Vault"))
                {
                    ObsidityMain.CreateVault(_vaultName);
                    // Refresh the AssetDatabase to ensure the new folder appears in the Project window
                    AssetDatabase.Refresh();
                    // open obsidity editor
                    ReplaceWelcomeWindowWithEditorWindow();
                }
            }

            if (GUILayout.Button("Reset to Defaults"))
                SetDefaultNameAndPath();
        }

        private void SetDefaultNameAndPath()
        {
            GUI.FocusControl(null);
            _vaultName = "";
        }

        /// <summary>
        ///     displayed in editor if vault is initialized
        /// </summary>
        /// <returns></returns>
        private void VaultIsInitialized()
        {
            var fullPath = ObsidityPlayerPrefs.GetString(ObsidityPlayerPrefsKeys.FullPath);
            var vaultName = ObsidityPlayerPrefs.GetString(ObsidityPlayerPrefsKeys.VaultName);
            DrawInitialInfo();

            void DrawInitialInfo()
            {
                // info about initialized state and vault name+location
                GUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.HelpBox(ObsidityStrings.AlreadyInitialized, MessageType.Warning);
                EditorGUILayout.HelpBox($"Vault Name: {vaultName}\nVault Location: {fullPath}", MessageType.Info);
                GUILayout.EndVertical();
            }

            DrawCheckForVault();

            void DrawCheckForVault()
            {
                // if not correct
                GUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.Box(GUIContent.none, GUILayout.ExpandWidth(true), GUILayout.Height(1));
                EditorGUILayout.HelpBox(ObsidityStrings.CheckForVault, MessageType.Info);
                if (GUILayout.Button("Check for existing Obsidity vault"))
                    ObsidityMain.CheckForVault();
                GUILayout.EndVertical();
            }


            GUILayout.BeginVertical(EditorStyles.helpBox);
            // info about where to open editor
            var wordWrapStyle = new GUIStyle(GUI.skin.label)
                { wordWrap = true };
            GUILayout.Label(ObsidityStrings.FindObsidityWindow, wordWrapStyle);
            GUILayout.Box(GUIContent.none, GUILayout.ExpandWidth(true), GUILayout.Height(1));

            // open editor button (makes above pointless :)
            if (GUILayout.Button("Open Obsidity Editor"))
                ReplaceWelcomeWindowWithEditorWindow();
            GUILayout.EndVertical();
        }

        private static void ReplaceWelcomeWindowWithEditorWindow()
        {
            var editorWindow = ObsidityEditorWindow.ShowAndGetWindow();
            var thisWindow = GetWindow<ObsidityWelcomeEditorWindow>();
            editorWindow.position = new Rect(new Vector2(thisWindow.position.width + 350, thisWindow.position.height),
                thisWindow.position.size);
            //   CloseWindow();
            // closing window was annoying after adding settings.
            // settings does not show before vault is initialized.
            //? change^?
        }

        [MenuItem("Window/Obsidity/Obsidity Intro")]
        public static void ShowWindow()
        {
            GetWindow<ObsidityWelcomeEditorWindow>();
        }

        public static void CloseWindow()
        {
            var window = GetWindow<ObsidityWelcomeEditorWindow>();
            if (window) window.Close();
        }
    }
}