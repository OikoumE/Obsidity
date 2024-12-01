using System;
using System.Collections.Generic;
using UnityEditor;

namespace Editor
{
    public static class ObsiditySettings
    {
        private static bool _isFolded;
        private static bool _linkTitle;
        private static bool _capitalizeTitle;
        private static readonly Dictionary<ObsidityPlayerPrefsKeys, bool> ObsiditySettingValues;

        static ObsiditySettings()
        {
            ObsiditySettingValues = new Dictionary<ObsidityPlayerPrefsKeys, bool>
            {
                {
                    ObsidityPlayerPrefsKeys.LinkTitle,
                    ObsidityPlayerPrefs.GetPrefAsBool(ObsidityPlayerPrefsKeys.LinkTitle)
                },
                {
                    ObsidityPlayerPrefsKeys.CapitalizeTitle,
                    ObsidityPlayerPrefs.GetPrefAsBool(ObsidityPlayerPrefsKeys.CapitalizeTitle)
                }
            };
        }


        private static string KeyToSettingString(ObsidityPlayerPrefsKeys key)
        {
            return key switch
            {
                ObsidityPlayerPrefsKeys.LinkTitle => "Save title with [[Link]]",
                ObsidityPlayerPrefsKeys.CapitalizeTitle => "Capitalize title",
                _ => throw new ArgumentException("Invalid key: not a setting-key: " + key)
            };
        }

        public static void DrawSettings()
        {
            _isFolded = EditorGUILayout.BeginFoldoutHeaderGroup(_isFolded, "Obsidity Settings");
            // If the foldout is not expanded, exit early.
            if (_isFolded)
            {
                EditorGUI.indentLevel++;
                foreach (var key in ObsiditySettingValues.Keys)
                {
                    var title = KeyToSettingString(key);
                    var currVal = ObsiditySettingValues[key];
                    var newValue = ObsidityEditorHelper.DrawToggle(title, currVal);
                    if (currVal == newValue)
                        continue;
                    ObsiditySettingValues[key] = newValue;
                    ObsidityPlayerPrefs.SetPrefAsBool(key, newValue);
                }
            }

            EditorGUI.indentLevel--; // Reset indentation
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
}