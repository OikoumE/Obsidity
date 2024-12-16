using UnityEngine;

namespace Editor
{
    public static class ObsidityLogger
    {
        /// <summary>
        /// simple helperclass for rich text on log outputs from Obsidity
        /// </summary>
        private static readonly string Pre = "[OBSIDITY]: ";

        public static void LogErr(string err, Object ctx = null)
        {
            Debug.LogError($"<color=red>{Pre}</color>" + err, ctx);
        }

        public static void LogWrn(string wrn, Object ctx = null)
        {
            Debug.LogWarning($"<color=yellow>{Pre}</color>" + wrn, ctx);
        }

        public static void Log(string s, Object ctx = null)
        {
            Debug.Log($"<color=green>{Pre}</color>" + s, ctx);
        }
    }
}