using System;
using UnityEngine;

namespace Library.PackageCache.com.oikoume.obsidity
{
    /// <summary>
    ///     helper enum for gettin playerPrefsKeys
    /// </summary>
    public enum ObsidityPlayerPrefsKeys
    {
        IsInitialized,
        FullPath,
        VaultName,
        FileNameIndex,
        ObsidityNotesFolder,
        CapitalizeTitle,
        LinkTitle
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
        public const string CapitalizeTitleKey = "CapitalizeTitle";
        public const string LinkTitleKey = "LinkTitle";


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
                ObsidityPlayerPrefsKeys.CapitalizeTitle => CapitalizeTitleKey,
                ObsidityPlayerPrefsKeys.LinkTitle => LinkTitleKey,
                _ => throw new ArgumentOutOfRangeException(nameof(key), key, null)
            };
        }

        public static void DeleteObsidityPlayerPrefs()
        {
            ObsidityLogger.LogWrn("Resetting stores playerPref keys");
            foreach (ObsidityPlayerPrefsKeys key in Enum.GetValues(typeof(ObsidityPlayerPrefsKeys)))
            {
                var keyString = GetStringKeyFromEnum(key);
                ObsidityLogger.Log("Resetting stores playerPrefs key: " + keyString);
                PlayerPrefs.DeleteKey(keyString);
            }
        }

        internal static bool GetPrefAsBool(ObsidityPlayerPrefsKeys key)
        {
            return GetInt(key) == 1;
        }

        internal static void SetPrefAsBool(ObsidityPlayerPrefsKeys key, bool value)
        {
            var i = value ? 1 : 0;
            SaveIntKey(key, i);
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
}