using System.IO;
using UnityEditor;
using UnityEngine;

namespace Library.PackageCache.com.oikoume.obsidity
{
    public static class FolderCreator
    {
        private static void CreateFolder(string absoluteFolderPath)
        {
            // Ensure the parent directory exists
            var parentDirectory = Path.GetDirectoryName(absoluteFolderPath);
            if (parentDirectory == null)
            {
                ObsidityLogger.LogErr("Something went wrong when trying to create a folder, " +
                                      "could not get parent directory.");
                return;
            }

            if (!Directory.Exists(parentDirectory))
                Directory.CreateDirectory(parentDirectory);

            // Create the folder
            if (!Directory.Exists(absoluteFolderPath))
            {
                Directory.CreateDirectory(absoluteFolderPath);
                ObsidityLogger.Log($"Folder created at: {absoluteFolderPath}");
            }
            else
            {
                ObsidityLogger.LogWrn($"Folder already exists at: {absoluteFolderPath}");
            }
#if UNITY_EDITOR
            // Refresh the AssetDatabase to ensure the new folder appears in the Project window
            AssetDatabase.Refresh();
#endif
        }

        public static void CreateVaultFolders(string vaultName)
        {
            // Combine paths to get the full path
            var obsidianHiddenFolder =
                Path.Combine(Application.dataPath, "Obsidity", vaultName, ".obsidian").Replace("\\", "/");
            CreateFolder(obsidianHiddenFolder);
            // creates app.json inside .obsidian folder
            File.WriteAllText(Path.Combine(obsidianHiddenFolder, "app.json"), ObsidityStrings.AppJson);
            // create folder for obsidian notes
            // (automatically assigns obsidian to create files to this folder via app.json)
            var obsidianNotesFolder = Path.Combine(Application.dataPath, "Obsidity", vaultName, "obsidianNotes")
                .Replace("\\", "/");
            CreateFolder(obsidianNotesFolder);
            // create folder for Obsidity notes. save it to playerPrefs
            var obsidityNotesFolder = Path.Combine(Application.dataPath, "Obsidity", vaultName, "ObsidityNotes")
                .Replace("\\", "/");
            ObsidityPlayerPrefs.SaveStringKey(ObsidityPlayerPrefsKeys.ObsidityNotesFolder, obsidityNotesFolder);
            CreateFolder(obsidityNotesFolder);
        }
    }
}