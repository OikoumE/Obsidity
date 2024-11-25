using System;
using UnityEngine;

public enum ObsidityPlayerPrefsKeys
{
    IsInitialized,
    FullPath,
    VaultName,
    FileNameIndex
}

public static class ObsidityPlayerPrefs
{
    public const string IsInitializedKey = "IsInitialized";
    public const string FullPathKey = "ObsidityVaultFullPath";
    public const string VaultNameKey = "ObsidityVaultName";
    public const string FileNameIndexKey = "FileNameIndex";


    private static string GetStringKeyFromEnum(ObsidityPlayerPrefsKeys key)
    {
        return key switch
        {
            ObsidityPlayerPrefsKeys.IsInitialized => IsInitializedKey,
            ObsidityPlayerPrefsKeys.FullPath => FullPathKey,
            ObsidityPlayerPrefsKeys.VaultName => VaultNameKey,
            ObsidityPlayerPrefsKeys.FileNameIndex => FileNameIndexKey,
            _ => throw new ArgumentOutOfRangeException(nameof(key), key, null)
        };
    }

    public static string GetString(ObsidityPlayerPrefsKeys key)
    {
        var sKey = GetStringKeyFromEnum(key);
        return PlayerPrefs.GetString(sKey);
    }

    public static int GetInt(ObsidityPlayerPrefsKeys key)
    {
        var sKey = GetStringKeyFromEnum(key);
        return PlayerPrefs.GetInt(sKey);
    }

    public static void SaveStringKey(ObsidityPlayerPrefsKeys key, string value)
    {
        var sKey = GetStringKeyFromEnum(key);
        PlayerPrefs.SetString(sKey, value);
        PlayerPrefs.Save();
    }

    public static void SaveIntKey(ObsidityPlayerPrefsKeys key, int value)
    {
        var sKey = GetStringKeyFromEnum(key);
        PlayerPrefs.SetInt(sKey, value);
        PlayerPrefs.Save();
    }
}