using Obsidity.Scripts.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ObsidityWelcomeEditorWindow : EditorWindow
    {
        private string _vaultName = "";

        public void OnGUI()
        {
            // Shows window content dependant on state
            GUILayout.Label("Welcome to Obsidity", EditorStyles.boldLabel);
            if (!ObsidityMain.IsInitialized())
                InitializeVault();
            else
                VaultIsInitialized();
        }

        /// <summary>
        ///     displayed in editor if vault is not initialized
        /// </summary>
        /// <returns></returns>
        private void InitializeVault()
        {
            GUILayout.Label("Please initialize the Obsidian Vault.");
            // name input
            _vaultName = ObsidityEditorHelper.DrawTextField("Vault Name", _vaultName);
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

            // info about initialized state and vault name+location
            EditorGUILayout.HelpBox(ObsidityStrings.AlreadyInitialized, MessageType.Warning);
            EditorGUILayout.HelpBox($"Vault Name: {vaultName}\nVault Location: {fullPath}", MessageType.Info);

            // info about where to open editor
            var wordWrapStyle = new GUIStyle(GUI.skin.label);
            wordWrapStyle.wordWrap = true;
            GUILayout.Label(ObsidityStrings.FindObsidityWindow, wordWrapStyle);
            GUILayout.Box(GUIContent.none, GUILayout.ExpandWidth(true), GUILayout.Height(1));

            // open editor button (makes above pointless :)
            if (GUILayout.Button("Open Obsidity Editor"))
                ReplaceWelcomeWindowWithEditorWindow();

            // if not correct
            GUILayout.Box(GUIContent.none, GUILayout.ExpandWidth(true), GUILayout.Height(1));
            EditorGUILayout.HelpBox(ObsidityStrings.CheckForVault, MessageType.Info);
            if (GUILayout.Button("Check for existing Obsidity vault"))
                ObsidityMain.CheckForVault();
        }

        private static void ReplaceWelcomeWindowWithEditorWindow()
        {
            var editorWindow = ObsidityEditorWindow.ShowAndGetWindow();
            var thisWindow = GetWindow<ObsidityWelcomeEditorWindow>();
            editorWindow.position = thisWindow.position;
            CloseWindow();
        }

        [MenuItem("Window/Obsidity/Obsidity Intro")]
        public static void ShowWindow()
        {
            GetWindow<ObsidityWelcomeEditorWindow>();
        }

        public static void CloseWindow()
        {
            var window = GetWindow<ObsidityWelcomeEditorWindow>();
            if (window != null) window.Close();
        }
    }
}