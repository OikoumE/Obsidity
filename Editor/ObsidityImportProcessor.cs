using System.Linq;
using UnityEditor;

namespace Library.PackageCache.com.oikoume.obsidity
{
    public class ObsidityImportProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            // checks if Obsidity is added, moved or removed, and triggers initialization 
            HandleObsidityImport(importedAssets);
        }


        private static void HandleObsidityImport(string[] assets)
        {
            if (FoundObsidity(assets))
            {
                if (!ObsidityMain.IsInitialized())
                {
                    ObsidityLogger.Log("Obsidity package imported. Running initialization script...");
                    RunInitializationScript();
                }
                else
                {
                    ObsidityLogger.Log("Obsidity package already initialized.");
                }
            }
        }


        private static bool FoundObsidity(string[] assets)
        {
            return assets.Any(asset => asset.EndsWith(".cs") && asset.Contains("Obsidity"));
        }


        private static void RunInitializationScript()
        {
            // initialization logic here
            ObsidityLogger.Log("Initialization script executed!");
            ObsidityWelcomeEditorWindow.ShowWindow();
        }
    }
}