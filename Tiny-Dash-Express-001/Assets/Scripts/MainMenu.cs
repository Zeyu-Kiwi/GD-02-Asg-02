using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        ResetSavedData();
        Application.Quit();
    }

    public void RestartGame()
    {
        ResetSavedData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ResetSavedData()
    {
        if (PlayerPrefs.HasKey("BestTime"))
        {
            //Debug.Log("Found BestTime save");
            float savedScore = PlayerPrefs.GetFloat("BestTime");
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetFloat("BestTime", savedScore);
        }
        else
        {
            //Debug.Log("No BestTime save");
            PlayerPrefs.DeleteAll();
        }
    }
}
