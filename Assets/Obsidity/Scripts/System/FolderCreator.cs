using System.IO;
using UnityEditor;
using UnityEngine;

public static class FolderCreator
{
    public static void CreateHiddenFolder(string vaultName)
    {
        // Combine paths to get the full path
        var fullPath = Path.Combine(Application.dataPath, "Obsidity", vaultName, ".obsidian").Replace("\\", "/");

        // Ensure the parent directory exists
        var parentDirectory = Path.GetDirectoryName(fullPath);
        if (!Directory.Exists(parentDirectory))
            Directory.CreateDirectory(parentDirectory);

        // Create the hidden folder
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
            Debug.Log($"Folder created at: {fullPath}");
        }
        else
        {
            Debug.LogWarning($"Folder already exists at: {fullPath}");
        }
#if UNITY_EDITOR
        // Refresh the AssetDatabase to ensure the new folder appears in the Project window
        AssetDatabase.Refresh();
#endif
    }
}