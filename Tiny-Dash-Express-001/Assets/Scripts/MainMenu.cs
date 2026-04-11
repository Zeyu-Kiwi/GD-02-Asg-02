using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        float savedScore = PlayerPrefs.GetFloat("BestTime");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("BestTime", savedScore);

        Application.Quit();
    }

    public void RestartGame()
    {
        float savedScore = PlayerPrefs.GetFloat("BestTime");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("BestTime", savedScore);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
