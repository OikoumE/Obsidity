using UnityEditor;

namespace Editor
{
    public class ObsidityWelcome : EditorWindow
    {
        [MenuItem("Window/Obsidity/Obsidity Editor")]
        public static void ShowWindow()
        {
            GetWindow<ObsidityWelcome>();
        }
    }
}