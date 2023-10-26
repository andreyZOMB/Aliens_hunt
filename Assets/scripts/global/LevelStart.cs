using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStart : MonoBehaviour
{
    public int levelIndex = 0;
    public void StartLevel()
    {
        
        SceneManager.LoadScene(levelIndex);
    }
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
