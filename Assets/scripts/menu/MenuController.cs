using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

//Один экземпляр на сцену
public class MenuController : MonoBehaviour
{
    private LevelIcon[] levels;
    private void Start()
    {
        levels = FindObjectsOfType<LevelIcon>();
        Debug.Log(levels.Length);
        LoadProgress();
    }

    //Записывает в LevelIcon.stepsLeft значения из сохранения
    private void LoadProgress()
    {
        SaveProgress progress;
        string path = Application.persistentDataPath + "/progress.save";
        BinaryFormatter formatter = new();
        FileStream stream = null;
        try
        {
            if (File.Exists(path))
            {
                stream = new(path, FileMode.Open);
                progress = formatter.Deserialize(stream) as SaveProgress;
            }
            else
            {
                progress = new SaveProgress(SceneManager.sceneCountInBuildSettings);
                stream = new(path, FileMode.Create);
                formatter.Serialize(stream, progress);
            }
            Debug.Log("! "+SceneManager.sceneCountInBuildSettings);
        }
        finally
        {
            stream?.Close();
        }
        Debug.Log(levels.Length);
        foreach(LevelIcon level in levels)
        {
            level.stepsLeft = progress.progress[level.GetComponent<LevelStart>().levelIndex];
        }
        foreach (LevelIcon level in levels)
        {
            level.Init();
        }
    }
}
