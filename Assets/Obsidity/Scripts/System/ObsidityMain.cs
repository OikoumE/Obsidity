using System;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;


public static class ObsidityMain
{
    private const string IsInitializedKey = "IsInitialized";

    public static bool IsInitialized()
    {
        return PlayerPrefs.GetInt(IsInitializedKey, 0) == 1;
    }

    public static void CreateVault(string vaultName, string vaultLocation)
    {
        Debug.Log("TEST");
        return;
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
                ObsidityLogger.Log($"Vault '{vaultName}' created successfully!");
                SetIsInitialized(true);
            }
            else
            {
                SetIsInitialized(true);
                ObsidityLogger.LogWrn("Directory already exists, please choose another directory");
            }
        }
        catch (Exception e)
        {
            SetIsInitialized(false);
            ObsidityLogger.LogErr($"An error occured: {e}");
        }
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