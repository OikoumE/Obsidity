using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ObsidityWelcome : EditorWindow
    {
        private string _vaultName = "";
        private string _vaultPath = "Assets/Obsidity/";


        public void OnGUI()
        {
            GUILayout.Label("Welcome to Obsidity", EditorStyles.boldLabel);
            if (!ObsidityMain.IsInitialized())
            {
                GUILayout.Label("Please initialize the Obsidian Vault.");
                _vaultName = ObsidityEditorHelper.DrawTextField("Vault Name", _vaultName);
                _vaultPath = ObsidityEditorHelper.DrawTextField("Path", _vaultPath);
                using (new EditorGUI.DisabledScope(_vaultName.Length == 0))
                {
                    if (GUILayout.Button("Initialize Vault"))
                        ObsidityMain.CreateVault(_vaultName, _vaultPath);
                }
            }
            else
            {
                ObsidityEditorHelper.ShowWarning(
                    "Obsidity is already initialized.",
                    ObsidityEditorHelper.ErrorStyle);
            }
        }

        [MenuItem("Window/Obsidity/Obsidity Intro")]
        public static void ShowWindow()
        {
            GetWindow<ObsidityWelcome>();
        }
    }
}