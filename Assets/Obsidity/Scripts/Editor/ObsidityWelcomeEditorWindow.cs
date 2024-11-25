using Editor;
using UnityEditor;
using UnityEngine;

namespace Obsidity.Scripts.Editor
{
    public class ObsidityWelcomeEditorWindow : EditorWindow
    {
        private string _vaultName = "";
        private string _vaultPath = ObsidityStrings.DefaultPath;

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
            _vaultName = ObsidityEditorHelper.DrawTextField("Vault Name", _vaultName);
            if (_vaultName.Length == 0)
                EditorGUILayout.HelpBox(ObsidityStrings.SelectVaultName, MessageType.Warning);

            _vaultPath = ObsidityEditorHelper.DrawTextField("Path", _vaultPath);
            if (_vaultPath.Length == 0)
                EditorGUILayout.HelpBox(ObsidityStrings.SelectVaultPath, MessageType.Warning);

            using (new EditorGUI.DisabledScope(_vaultName.Length == 0))
            {
                if (GUILayout.Button("Initialize Vault"))
                    ObsidityMain.CreateVault(_vaultName, _vaultPath);
            }

            if (GUILayout.Button("Reset to Defaults"))
                SetDefaultNameAndPath();
        }

        private void SetDefaultNameAndPath()
        {
            GUI.FocusControl(null);
            _vaultName = "";
            _vaultPath = ObsidityStrings.DefaultPath;
        }

        private void VaultIsInitialized()
        {
            var fullPath = PlayerPrefs.GetString(ObsidityMain.ObsidityVaultFullPath);
            EditorGUILayout.HelpBox(ObsidityStrings.AlreadyInitialized + fullPath, MessageType.Warning);
            var wordWrapStyle = new GUIStyle(GUI.skin.label);
            wordWrapStyle.wordWrap = true;
            GUILayout.Label(ObsidityStrings.FindObsidityWindow, wordWrapStyle);
            GUILayout.Box(GUIContent.none, GUILayout.ExpandWidth(true), GUILayout.Height(1));

            if (GUILayout.Button("Open Obsidity Editor"))
                ObsidityEditorWindow.ShowWindow();
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