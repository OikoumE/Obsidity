using UnityEngine;

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