using System;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;


public static class ObsidityMain
{
    public const string IsInitializedKey = "IsInitialized";
    public const string ObsidityVaultFullPath = "ObsidityVaultFullPath";


    public static bool IsInitialized()
    {
        var path = PlayerPrefs.GetString(ObsidityVaultFullPath);
        var pathIsInvalid = string.IsNullOrEmpty(path);
        if (pathIsInvalid) return false;
        return PlayerPrefs.GetInt(IsInitializedKey, 0) == 1;
    }

    public static void CheckForVault()
    {
        if (!IsInitialized())
        {
            ObsidityLogger.LogErr("Obsidity Vault Not Initialized");
            return;
        }

        var fullPath = PlayerPrefs.GetString(ObsidityVaultFullPath);
        if (string.IsNullOrEmpty(fullPath))
            fullPath = ObsidityStrings.DefaultPath;
        if (!Directory.Exists(fullPath))
        {
            ObsidityLogger.LogErr("Stored Obsidity Vault Folder Location Not Found. Path: " + fullPath);
            PlayerPrefs.SetInt(IsInitializedKey, 0);
            PlayerPrefs.SetString(ObsidityVaultFullPath, null);
            PlayerPrefs.Save();
            ObsidityLogger.LogErr("Obsidity Reset, please Initialize via Window/Obsidity/Obsidity Welcome!");
        }
        else
        {
            ObsidityLogger.Log("Stored Obsidity Vault Folder Location Found. Path: " + fullPath);
            PlayerPrefs.SetInt(IsInitializedKey, 1);
            PlayerPrefs.SetString(ObsidityVaultFullPath, fullPath);
            PlayerPrefs.Save();
            ObsidityLogger.LogWrn("Obsidity Vault path updated to: " + fullPath);
        }
    }

    public static void CreateVault(string vaultName, string vaultLocation)
    {
        if (IsInitialized())
        {
            ObsidityLogger.LogErr("ObsidityMain.CreateVault() Already Initialized");
            return;
        }

        try
        {
            var fullPath = Path.Combine(vaultLocation, vaultName);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
                Directory.CreateDirectory(Path.Combine(fullPath, ".obsidian"));

                var readmeContent = $"New vault: {vaultName}\n";
                File.WriteAllText(Path.Combine(fullPath, "README.md"), readmeContent);
                ObsidityLogger.Log($"Vault '{vaultName}' created successfully at: {fullPath}");
                PlayerPrefs.SetString(ObsidityVaultFullPath, fullPath);
                PlayerPrefs.Save();
                SetIsInitialized(true, fullPath);
            }
            else
            {
                ObsidityLogger.LogWrn("Directory already exists");
            }

            SetIsInitialized(true, fullPath);
        }
        catch (Exception e)
        {
            SetIsInitialized(false);
            ObsidityLogger.LogErr($"An error occured: {e}");
        }
    }


    private static void SetIsInitialized(bool isInitialized, string fullPath)
    {
        if (isInitialized && string.IsNullOrEmpty(fullPath))
            ObsidityLogger.LogErr($"isInitialized{isInitialized}, but path is null or empty. path: {fullPath}");
        PlayerPrefs.SetString(ObsidityVaultFullPath, fullPath);
        SetIsInitialized(isInitialized);
    }

    private static void SetIsInitialized(bool isInitialized)
    {
        PlayerPrefs.SetInt(IsInitializedKey, isInitialized ? 1 : 0);
        PlayerPrefs.Save();
    }
}

public static class ObsidityLogger
{
    private static readonly string _pre = "[OBSIDITY]: ";

    public static void LogErr(string err, Object ctx = null)
    {
        Debug.LogError(_pre + err, ctx);
    }

    public static void LogWrn(string wrn, Object ctx = null)
    {
        Debug.LogWarning(_pre + wrn, ctx);
    }

    public static void Log(string s, Object ctx = null)
    {
        Debug.Log(_pre + s, ctx);
    }
}


//
// Obsidity
//
// Obsidian.md + Unity
// ---
// tags:
//   - Literary
// DocType: Literary Note
// Created: "{{date}}"
// Type:
// ---
// collectAll undone tasks
// ```dataview
// TASK
// from !"Templates"
// where !completed
// ```
//
//
// ---
//
// kanban-plugin: board
//
// ---
//
// ## scascaczxc
//
// - [ ] zxczx
// - [ ] zxc
// - [ ] zc
// - [ ] zxc
// - [ ] z
// - [ ] z
// - [ ] z
// - [ ] zz
//
//
// ## zxczx
//
//
//
// ## zxc
//
//
//
// ## zxc
//
//
//
// ## zxc
//
// **Complete**
// - [x] cx
// - [x] czz
// - [x] z
// - [x] zx
//
//
//
//
// %% kanban:settings
// ```
// {"kanban-plugin":"board","list-collapse":[true,true,false,false,false]}
// ```
// %%