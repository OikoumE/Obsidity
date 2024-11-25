using Editor;
using UnityEditor;
using UnityEngine;

namespace Obsidity.Scripts.Editor
{
    public class ObsidityWelcomeEditorWindow : EditorWindow
    {
        private string _vaultName = "";

        public void OnGUI()
        {
            GUILayout.Label("Welcome to Obsidity", EditorStyles.boldLabel);
            if (!ObsidityMain.IsInitialized())
                InitializeVault();
            else
                VaultIsInitialized();
        }

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
                if (GUILayout.Button("Initialize Vault"))
                    ObsidityMain.CreateVault(_vaultName);
#if UNITY_EDITOR
                // Refresh the AssetDatabase to ensure the new folder appears in the Project window
                AssetDatabase.Refresh();
#endif
            }

            if (GUILayout.Button("Reset to Defaults"))
                SetDefaultNameAndPath();
        }

        private void SetDefaultNameAndPath()
        {
            GUI.FocusControl(null);
            _vaultName = "";
            // _vaultPath = ObsidityStrings.DefaultPath;
        }

        private void VaultIsInitialized()
        {
            var fullPath = ObsidityPlayerPrefs.GetString(ObsidityPlayerPrefsKeys.FullPath);
            var vaultName = ObsidityPlayerPrefs.GetString(ObsidityPlayerPrefsKeys.VaultName);

            EditorGUILayout.HelpBox(ObsidityStrings.AlreadyInitialized, MessageType.Warning);
            EditorGUILayout.HelpBox($"Vault: {vaultName} at location: {fullPath}", MessageType.Info);

            // info about where to open editor
            var wordWrapStyle = new GUIStyle(GUI.skin.label);
            wordWrapStyle.wordWrap = true;
            GUILayout.Label(ObsidityStrings.FindObsidityWindow, wordWrapStyle);
            GUILayout.Box(GUIContent.none, GUILayout.ExpandWidth(true), GUILayout.Height(1));

            // open editor button (makes above pointless :)
            if (GUILayout.Button("Open Obsidity Editor"))
                ObsidityEditorWindow.ShowWindow();

            // if not correct
            GUILayout.Box(GUIContent.none, GUILayout.ExpandWidth(true), GUILayout.Height(1));
            EditorGUILayout.HelpBox(ObsidityStrings.CheckForVault, MessageType.Info);
            if (GUILayout.Button("Check for existing Obsidity vault"))
                ObsidityMain.CheckForVault();
        }


        [MenuItem("Window/Obsidity/Obsidity Intro")]
        public static void ShowWindow()
        {
            GetWindow<ObsidityWelcomeEditorWindow>();
        }
    }
}