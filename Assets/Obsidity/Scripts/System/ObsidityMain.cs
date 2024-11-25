using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class ObsidityMain
{
    public static bool SaveMarkdownFile(ObsidityData data)
    {
        try
        {
            var vaultName = ObsidityPlayerPrefs.GetString(ObsidityPlayerPrefsKeys.VaultName);
            var vaultFullPath = ObsidityPlayerPrefs.GetString(ObsidityPlayerPrefsKeys.FullPath);
            var index = ObsidityPlayerPrefs.GetInt(ObsidityPlayerPrefsKeys.FileNameIndex);
            Debug.Log(vaultFullPath);

            var newIndex = index + 1;
            var fileName = $"{vaultName}_{newIndex:D5}.md";
            ObsidityPlayerPrefs.SaveIntKey(ObsidityPlayerPrefsKeys.FileNameIndex, newIndex);
            var fullFileNamePath = Path.Combine(vaultFullPath, fileName).Replace("\\", "/");
            Debug.Log(fullFileNamePath);
            var stringData =
                "# Title:" + data.textTitle +
                "\n ---" +
                "\n tags:" + IterateTags(data) +
                "\n Created: " + data.textDate +
                "\n ---\n" + data.textContent;

            File.WriteAllText(fullFileNamePath, stringData);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
            return true;
        }
        catch (Exception e)
        {
            ObsidityLogger.LogErr("Error encountered when creating file: " + e);
            return false;
        }
    }

    private static string IterateTags(ObsidityData data)
    {
        return data.textTags.Split(",").Aggregate("", (current, tag) => current + $"\n - {tag}");
    }

    public static bool IsInitialized()
    {
        var path = ObsidityPlayerPrefs.GetString(ObsidityPlayerPrefsKeys.FullPath);
        var vaultName = ObsidityPlayerPrefs.GetString(ObsidityPlayerPrefsKeys.VaultName);

        var pathIsInvalid = string.IsNullOrEmpty(path);
        var nameIsInvalid = string.IsNullOrEmpty(vaultName);
        if (nameIsInvalid || pathIsInvalid) return false;

        return ObsidityPlayerPrefs.GetInt(ObsidityPlayerPrefsKeys.IsInitialized) == 1;
    }

    public static void CheckForVault()
    {
        if (!IsInitialized())
        {
            ObsidityLogger.LogErr("Obsidity Vault Not Initialized");
            return;
        }

        var fullPath = ObsidityPlayerPrefs.GetString(ObsidityPlayerPrefsKeys.FullPath);
        var vaultName = ObsidityPlayerPrefs.GetString(ObsidityPlayerPrefsKeys.VaultName);
        if (string.IsNullOrEmpty(vaultName))
            ObsidityLogger.LogErr("Invalid VaultName: " + vaultName);


        if (!Directory.Exists(fullPath))
        {
            ObsidityLogger.LogErr("Stored Obsidity Vault Folder Location Not Found. Path: " + fullPath);
            SetIsInitialized(false);

            PlayerPrefs.Save();
            ObsidityLogger.LogErr("Obsidity Reset, please Initialize via Window/Obsidity/Obsidity Welcome!");
        }
        else
        {
            ObsidityLogger.Log("Stored Obsidity Vault Folder Location Found. Path: " + fullPath);

            SetIsInitialized(true, vaultName, fullPath);
            ObsidityLogger.LogWrn("Obsidity Vault path updated to: " + fullPath);
        }
    }

    public static void CreateVault(string vaultName)
    {
        if (IsInitialized())
        {
            ObsidityLogger.LogErr("ObsidityMain.CreateVault() Already Initialized");
            return;
        }

        try
        {
            // if (!AssetDatabase.IsValidFolder($"Assets/Obsidity/{vaultName}"))
            //     pathGuid = AssetDatabase.CreateFolder("Assets/Obsidity", vaultName);
            //
            //

            var vaultPath = Path.Combine(Application.dataPath, "Obsidity", vaultName).Replace("\\", "/");
            FolderCreator.CreateHiddenFolder(vaultName);
            var readmeContent = $"New vault: {vaultName}\n";
            File.WriteAllText(Path.Combine(vaultPath, "README.md"), readmeContent);
            ObsidityLogger.Log($"Vault '{vaultName}' created successfully at: {vaultPath}");
            SetIsInitialized(true, vaultName, vaultPath);
#if UNTIY_EDITOR
            ObsidityEditorWindow.ShowWindow();
#endif
        }
        catch (Exception e)
        {
            SetIsInitialized(false);
            ObsidityLogger.LogErr($"An error occured: {e}");
        }
    }


    private static void SetIsInitialized(bool isInitialized, string vaultName = null, string fullPath = null)
    {
        var nameIsInvalid = string.IsNullOrEmpty(vaultName);
        var pathIsInvalid = string.IsNullOrEmpty(fullPath);

        if (isInitialized && (nameIsInvalid || pathIsInvalid))
        {
            ObsidityLogger.LogErr($"is Initialized, but path is null or empty. current path: {fullPath}");
            return;
        }

        if (!nameIsInvalid)
            ObsidityPlayerPrefs.SaveStringKey(ObsidityPlayerPrefsKeys.VaultName, vaultName);
        if (!pathIsInvalid)
            ObsidityPlayerPrefs.SaveStringKey(ObsidityPlayerPrefsKeys.FullPath, fullPath);

        ObsidityPlayerPrefs.SaveIntKey(ObsidityPlayerPrefsKeys.IsInitialized, isInitialized ? 1 : 0);
        if (!isInitialized)
            ObsidityPlayerPrefs.SaveIntKey(ObsidityPlayerPrefsKeys.FileNameIndex, 0);
    }
}