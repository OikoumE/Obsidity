using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    //TODO scan for existing .md's inside Assets/Obsidity/ to restore vaults/add multiple
    //TODO add settings to intro window (editorFontSize, saveOnShiftReturn, etc)
    public static class ObsidityMain
    {
        /// <summary>
        ///     Creates meta tag, appends title and content with (minor) formatting
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string CreateString(ObsidityData data)
        {
            var title = data.textTitle;

            // setting: toggle, capitalizeTitle 
            //TODO get setting from ObsiditySettings
            var doCapital = ObsidityPlayerPrefs.GetPrefAsBool(ObsidityPlayerPrefsKeys.CapitalizeTitle);
            if (doCapital) title = title.FirstCharToUpper(); //setting to add

            // setting: toggle, [[Link]] titles
            //TODO get setting from ObsiditySettings
            var doLink = ObsidityPlayerPrefs.GetPrefAsBool(ObsidityPlayerPrefsKeys.CapitalizeTitle);
            if (doLink) title = $"[[{title}]]";

            var parseContent = ShouldParse();
            var content = data.textContent;
            if (parseContent) content = ParseContentString(data.textContent);

            return $"{CreateMetaString(data)}" +
                   $"\n# {title}" +
                   "\n---" +
                   $"\n{content}" +
                   "\n\n---";
        }

        private static bool ShouldParse()
        {
            return true;
            //TODO set/get prefs (settings) 
            //ObsidityPlayerPrefs.GetPrefAsBool(ObsidityPlayerPrefsKeys.CapitalizeTitle);
        }

        private static string ParseContentString(string dataTextContent)
        {
            //TODO settings: replaceTodoWithMetaTodo
            dataTextContent = dataTextContent.Replace("//TODO", "\n- [ ] ");
            dataTextContent = dataTextContent.Replace("//todo", "\n- [ ] ");
            dataTextContent = dataTextContent.Replace("//Todo", "\n- [ ] ");


            return dataTextContent;
        }

        /// <summary>
        ///     space separated string with tags
        /// </summary>
        /// <param name="data"></param>
        /// <returns>string formatted according to meta field requirements of obsisidan</returns>
        private static string CreateMetaString(ObsidityData data)
        {
            // space separated string with tags
            const string pre = "---\ntags:";
            var tags = data.textTags.Split(" ")
                .Aggregate("", (current, tag) => current + $"\n - {tag}");
            var post = $"\nCreated: {data.textDate}\n---";
            return pre + tags + post;
        }

        /// <summary>
        ///     Save a markdown file to the Vault
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool SaveMarkdownFile(ObsidityData data)
        {
            try
            {
                var vaultName = ObsidityPlayerPrefs.GetString(ObsidityPlayerPrefsKeys.VaultName);
                var vaultFullPath = ObsidityPlayerPrefs.GetString(ObsidityPlayerPrefsKeys.ObsidityNotesFolder);
                var index = ObsidityPlayerPrefs.GetInt(ObsidityPlayerPrefsKeys.FileNameIndex);
                var newIndex = index + 1;
                // assign filename
                var fileName = $"{vaultName}_{newIndex:D5}.md";
                ObsidityLogger.Log($"Saved file {fileName}");
                ObsidityPlayerPrefs.SaveIntKey(ObsidityPlayerPrefsKeys.FileNameIndex, newIndex);

                var fullFileNamePath = Path.Combine(vaultFullPath, fileName).Replace("\\", "/");
                // assign meta+content
                var stringData = CreateString(data);
                // write file
                File.WriteAllText(fullFileNamePath, stringData);
#if UNITY_EDITOR
                // refresh project folder to immediatly show the new files/folders
                AssetDatabase.Refresh();
#endif
                return true;
            }
            catch (Exception e)
            {
                ObsidityLogger.LogErr("Error encountered when saving file: " + e);
                return false;
            }
        }


        /// <summary>
        ///     checks if Obsidity is initialized
        /// </summary>
        /// <returns>false if we dont have path, vaultname or isInitialzied = 0</returns>
        public static bool IsInitialized()
        {
            var path = ObsidityPlayerPrefs.GetString(ObsidityPlayerPrefsKeys.FullPath);
            var vaultName = ObsidityPlayerPrefs.GetString(ObsidityPlayerPrefsKeys.VaultName);

            var pathIsInvalid = string.IsNullOrEmpty(path);
            var nameIsInvalid = string.IsNullOrEmpty(vaultName);
            if (nameIsInvalid || pathIsInvalid) return false;

            return ObsidityPlayerPrefs.GetInt(ObsidityPlayerPrefsKeys.IsInitialized) == 1;
        }

        /// <summary>
        ///     simple check of playerPrefs to see if we have stored vaultName and path,
        ///     checks the path and vaultname is valid
        ///     checks that directory exsists - resets isInitialized to false if no directory
        /// </summary>
        /// <returns></returns>
        public static void CheckForVault()
        {
            if (!IsInitialized())
            {
                ObsidityLogger.LogErr("Obsidity Vault Not Initialized");
                return;
            }

            // validate vaultName and path
            var fullPath = ObsidityPlayerPrefs.GetString(ObsidityPlayerPrefsKeys.FullPath);
            var vaultName = ObsidityPlayerPrefs.GetString(ObsidityPlayerPrefsKeys.VaultName);
            if (string.IsNullOrEmpty(vaultName))
                ObsidityLogger.LogErr("Invalid VaultName: " + vaultName);
            if (string.IsNullOrEmpty(fullPath))
                ObsidityLogger.LogErr("Invalid VaultPath: " + fullPath);
            // check that path exists
            if (!Directory.Exists(fullPath))
            {
                // reset initialized state
                ObsidityLogger.LogErr("Stored Obsidity Vault Folder Location Not Found. Path: " + fullPath);
                SetIsInitialized(false);

                PlayerPrefs.Save();
                ObsidityLogger.LogErr("Obsidity Reset, please Initialize via Window/Obsidity/Obsidity Welcome!");
            }
            else
            {
                ObsidityLogger.Log("Stored Obsidity Vault Folder Location Found. Path: " + fullPath);
                // ensure initialized state
                SetIsInitialized(true, vaultName, fullPath);
                ObsidityLogger.LogWrn("Obsidity Vault path updated to: " + fullPath);
            }
        }

        /// <summary>
        ///     constructor for vault
        /// </summary>
        /// <param name="vaultName"></param>
        /// <returns></returns>
        public static void CreateVault(string vaultName)
        {
            // checking if initialized
            if (IsInitialized())
            {
                ObsidityLogger.LogErr("ObsidityMain.CreateVault() Already Initialized");
                return;
            }

            try
            {
                //try to create folders and files
                var vaultPath = Path.Combine(Application.dataPath, "Obsidity", vaultName).Replace("\\", "/");
                ObsidityFolderCreator.CreateVaultFolders(vaultName);
                var readmeContent = $"New vault: {vaultName}\n";
                File.WriteAllText(Path.Combine(vaultPath, "README.md"), readmeContent);
                ObsidityLogger.Log($"Vault '{vaultName}' created successfully at: {vaultPath}");
                SetIsInitialized(true, vaultName, vaultPath);
#if UNTIY_EDITOR
            // refresh project folder to display newly created files/folders
            AssetDatabase.Refresh();
#endif
            }
            catch (Exception e)
            {
                SetIsInitialized(false);
                ObsidityLogger.LogErr($"An error occured: {e}");
            }
        }


        /// <summary>
        ///     helper method for assigning IsInitialized State
        /// </summary>
        /// <param name="isInitialized">state to set</param>
        /// <param name="vaultName">name of vault</param>
        /// <param name="fullPath">path to vault</param>
        /// <returns></returns>
        private static void SetIsInitialized(bool isInitialized, string vaultName = null, string fullPath = null)
        {
            var nameIsInvalid = string.IsNullOrEmpty(vaultName);
            var pathIsInvalid = string.IsNullOrEmpty(fullPath);
            // check if path and name is valid if we are setting initialzied=true
            if (isInitialized && (nameIsInvalid || pathIsInvalid))
            {
                ObsidityLogger.LogErr($"is Initialized, but path is null or empty. current path: {fullPath}");
                return;
            }

            // assign playerPrefs if included
            if (!nameIsInvalid)
                ObsidityPlayerPrefs.SaveStringKey(ObsidityPlayerPrefsKeys.VaultName, vaultName);
            // assign playerPrefs if included
            if (!pathIsInvalid)
                ObsidityPlayerPrefs.SaveStringKey(ObsidityPlayerPrefsKeys.FullPath, fullPath);
            // set isInitialized
            ObsidityPlayerPrefs.SaveIntKey(ObsidityPlayerPrefsKeys.IsInitialized, isInitialized ? 1 : 0);
            // reset note-index 
            if (!isInitialized)
                ObsidityPlayerPrefs.SaveIntKey(ObsidityPlayerPrefsKeys.FileNameIndex, 0);
        }
    }
}