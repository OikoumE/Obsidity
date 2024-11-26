using System;
using UnityEngine;

/// <summary>
///     helper enum for gettin playerPrefsKeys
/// </summary>
public enum ObsidityPlayerPrefsKeys
{
    IsInitialized,
    FullPath,
    VaultName,
    FileNameIndex,
    ObsidityNotesFolder
}

/// <summary>
///     helper class for handling playerPref keys
/// </summary>
public static class ObsidityPlayerPrefs
{
    public const string IsInitializedKey = "IsInitialized";
    public const string FullPathKey = "ObsidityVaultFullPath";
    public const string VaultNameKey = "ObsidityVaultName";
    public const string FileNameIndexKey = "FileNameIndex";
    public const string ObsidityNotesFolderKey = "ObsidityNotesFolder";


    private static string GetStringKeyFromEnum(ObsidityPlayerPrefsKeys key)
    {
        // does what it says on the tin!
        return key switch
        {
            ObsidityPlayerPrefsKeys.IsInitialized => IsInitializedKey,
            ObsidityPlayerPrefsKeys.FullPath => FullPathKey,
            ObsidityPlayerPrefsKeys.VaultName => VaultNameKey,
            ObsidityPlayerPrefsKeys.FileNameIndex => FileNameIndexKey,
            ObsidityPlayerPrefsKeys.ObsidityNotesFolder => ObsidityNotesFolderKey,
            _ => throw new ArgumentOutOfRangeException(nameof(key), key, null)
        };
    }

    public static string GetString(ObsidityPlayerPrefsKeys key)
    {
        // does what it says on the tin!
        var sKey = GetStringKeyFromEnum(key);
        return PlayerPrefs.GetString(sKey);
    }

    public static int GetInt(ObsidityPlayerPrefsKeys key)
    {
        // does what it says on the tin!
        var sKey = GetStringKeyFromEnum(key);
        return PlayerPrefs.GetInt(sKey);
    }

    public static void SaveStringKey(ObsidityPlayerPrefsKeys key, string value)
    {
        // does what it says on the tin!
        var sKey = GetStringKeyFromEnum(key);
        PlayerPrefs.SetString(sKey, value);
        PlayerPrefs.Save();
    }

    public static void SaveIntKey(ObsidityPlayerPrefsKeys key, int value)
    {
        // does what it says on the tin!
        var sKey = GetStringKeyFromEnum(key);
        PlayerPrefs.SetInt(sKey, value);
        PlayerPrefs.Save();
    }
}