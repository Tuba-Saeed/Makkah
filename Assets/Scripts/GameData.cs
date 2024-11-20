using UnityEngine;

public static class GameData
{
    // This will hold the current level, either "Hajj" or "Umrah"
    public static string CurrentMainLevel;

    // Ensure the GameData persists between scene loads
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnRuntimeMethodLoad()
    {
        // If CurrentMainLevel is not set yet, set it to a default level (Hajj in this case)
        if (string.IsNullOrEmpty(CurrentMainLevel))
        {
            CurrentMainLevel = "Hajj"; // Default level
        }
    }
}
