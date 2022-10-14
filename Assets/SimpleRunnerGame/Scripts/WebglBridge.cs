using System.Runtime.InteropServices;

public static class WebglBridge
{
    [DllImport("__Internal")]
    private static extern void OnGameOver();
    [DllImport("__Internal")]
    private static extern void OnScoreUpdate(int score);
    [DllImport("__Internal")]
    private static extern void OnFloatingText(float x, float y, string text);
    
    public static void GameOver()
    {
#if UNITY_WEBGL && ! UNITY_EDITOR
        OnGameOver();
#endif
    }

    public static void UpdateScore(int score)
    {
#if UNITY_WEBGL && ! UNITY_EDITOR
        OnScoreUpdate(score);
#endif
    }

    public static void SetFloatingText(float x, float y, string text)
    {
#if UNITY_WEBGL && ! UNITY_EDITOR
        OnFloatingText(x, y, text);
#endif
    }
}