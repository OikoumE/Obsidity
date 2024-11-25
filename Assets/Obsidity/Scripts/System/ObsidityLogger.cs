using UnityEngine;

public static class ObsidityLogger
{
    private static readonly string _pre = "[OBSIDITY]: ";

    public static void LogErr(string err, Object ctx = null)
    {
        Debug.LogError($"<color=red>{_pre}</color>" + err, ctx);
    }

    public static void LogWrn(string wrn, Object ctx = null)
    {
        Debug.LogWarning($"<color=yellow>{_pre}</color>" + wrn, ctx);
    }

    public static void Log(string s, Object ctx = null)
    {
        Debug.Log($"<color=green>{_pre}</color>" + s, ctx);
    }
}