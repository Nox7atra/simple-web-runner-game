using UnityEngine;
using UnityEngine.SceneManagement;

public class ReactEventsHandler : MonoBehaviour
{
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
