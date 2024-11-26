using UnityEditor;

namespace Obsidity.Scripts.Editor
{
    public class ObsidityImportProcessor : AssetPostprocessor
    {
        
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            // checks if Obsidity is added, moved or removed, and triggers initialization 
            foreach (var asset in importedAssets)
                if (asset.EndsWith(".cs") && asset.Contains("Obsidity"))
                {
                    ObsidityLogger.Log("Obsidity package imported. Running initialization script...");
                    RunInitializationScript();
                    break;
                }
        }

        private static void RunInitializationScript()
        {
            // initialization logic here
            ObsidityLogger.Log("Initialization script executed!");
            ObsidityWelcomeEditorWindow.ShowWindow();
        }
    }
}