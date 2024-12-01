using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class ObsiditySettings
    {
        private static bool _isExpanded;
        private static bool _linkTitle;
        private static bool _capitalizeTitle;
        private static int _fontSize;
        private static readonly Dictionary<ObsidityPlayerPrefsKeys, int> ObsidityIntValues;

        static ObsiditySettings()
        {
            TrySetDefaults();

            ObsidityIntValues = new Dictionary<ObsidityPlayerPrefsKeys, int>
            {
                {
                    ObsidityPlayerPrefsKeys.LinkTitle,
                    ObsidityPlayerPrefs.GetInt(ObsidityPlayerPrefsKeys.LinkTitle)
                },
                {
                    ObsidityPlayerPrefsKeys.CapitalizeTitle,
                    ObsidityPlayerPrefs.GetInt(ObsidityPlayerPrefsKeys.CapitalizeTitle)
                },
                {
                    ObsidityPlayerPrefsKeys.SaveShiftReturn,
                    ObsidityPlayerPrefs.GetInt(ObsidityPlayerPrefsKeys.SaveShiftReturn)
                },
                {
                    ObsidityPlayerPrefsKeys.FontSize,
                    ObsidityPlayerPrefs.GetInt(ObsidityPlayerPrefsKeys.FontSize)
                }
            };
        }

        private static void TrySetDefaults()
        {
            if (!ObsidityPlayerPrefs.HasKey(ObsidityPlayerPrefsKeys.LinkTitle))
                ObsidityPlayerPrefs.SaveIntKey(ObsidityPlayerPrefsKeys.LinkTitle, 1);
            if (!ObsidityPlayerPrefs.HasKey(ObsidityPlayerPrefsKeys.CapitalizeTitle))
                ObsidityPlayerPrefs.SaveIntKey(ObsidityPlayerPrefsKeys.CapitalizeTitle, 1);
            if (!ObsidityPlayerPrefs.HasKey(ObsidityPlayerPrefsKeys.SaveShiftReturn))
                ObsidityPlayerPrefs.SaveIntKey(ObsidityPlayerPrefsKeys.SaveShiftReturn, 1);
            if (!ObsidityPlayerPrefs.HasKey(ObsidityPlayerPrefsKeys.FontSize))
                ObsidityPlayerPrefs.SaveIntKey(ObsidityPlayerPrefsKeys.FontSize, 15);
        }

        private static string KeyToSettingString(ObsidityPlayerPrefsKeys key)
        {
            return key switch
            {
                ObsidityPlayerPrefsKeys.LinkTitle => "Save title with [[Link]]",
                ObsidityPlayerPrefsKeys.CapitalizeTitle => "Capitalize title",
                ObsidityPlayerPrefsKeys.SaveShiftReturn => "Save with shift return",
                ObsidityPlayerPrefsKeys.FontSize => "Font size",
                _ => throw new ArgumentException("Invalid key: not a setting-key: " + key)
            };
        }

        public static void DrawSettings()
        {
            GUILayout.Label("Settings:", EditorStyles.boldLabel);
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.BeginVertical(EditorStyles.helpBox);
            _isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(_isExpanded, "Obsidity Settings");
            GUILayout.EndVertical();

            // If the foldout is not expanded, exit early.
            if (_isExpanded)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                HandleAllKeys();
                GUILayout.EndVertical();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            GUILayout.EndVertical();
        }

        private static void HandleAllKeys()
        {
            const int indentation = 15;


            foreach (var key in ObsidityIntValues.Keys.ToArray())
                switch (key)
                {
                    case ObsidityPlayerPrefsKeys.FontSize: //int
                        HandleInt(key, indentation);
                        break;
                    case ObsidityPlayerPrefsKeys.CapitalizeTitle: //bool
                    case ObsidityPlayerPrefsKeys.LinkTitle: //bool
                    case ObsidityPlayerPrefsKeys.SaveShiftReturn: //bool
                        HandleBool(key, indentation);
                        break;
                }
        }

        private static void HandleInt(ObsidityPlayerPrefsKeys key, int indent)
        {
            var title = KeyToSettingString(key);
            var currVal = ObsidityIntValues[key];
            var newInt = ObsidityEditorHelper.DrawIntSlider(title, currVal, indent);
            if (newInt == currVal) return;
            ObsidityIntValues[key] = newInt;
            ObsidityPlayerPrefs.SaveIntKey(key, newInt);
        }

        private static void HandleBool(ObsidityPlayerPrefsKeys key, int indent)
        {
            var title = KeyToSettingString(key);
            var currVal = ObsidityIntValues[key];
            var currBool = currVal == 1;
            var newBool = ObsidityEditorHelper.DrawToggle(title, currBool, indent);
            if (currBool == newBool) return;
            ObsidityIntValues[key] = newBool ? 1 : 0;
            ObsidityPlayerPrefs.SaveIntKey(key, newBool);
        }

        public static int Get(ObsidityPlayerPrefsKeys key)
        {
            ObsidityIntValues.TryGetValue(key, out var value);
            return value;
        }
    }
}