using UnityEngine;
using UnityEngine.SceneManagement;

public class ReactEventsHandler : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = -1;
        Pause();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }
}
