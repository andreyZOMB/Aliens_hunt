using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;


public class LevelController : MonoBehaviour
{


    public float CellSize = 30f;
    public int xLength = 10;
    public int yLength = 10;
    public float moveTime = 0.3f;
    public float collideTime = 0.5f;
    public float randomRange = 0.03f;
    public ObjectControl[,] cellArray;
    [SerializeField]
    private GameObject winCanvas;
    [SerializeField]
    private GameObject loseCanvas;
    [SerializeField]
    private GameObject dialogCanvas;
    [SerializeField]
    private GameObject winDialogCanvas;
    [SerializeField]
    private Text stepsLeftField;
    public int stepCount;
    [SerializeField]
    private GameObject adsController;
    [SerializeField]
    private HintSystem hintSystem;
    private bool gameEnded = false;
    private string filePath;
    private SaveProgress progress;

    private void Awake()
    {
        cellArray = new ObjectControl[xLength, yLength];
        filePath = Application.persistentDataPath + "/progress.save";
    }
    private void Start()
    {
        UpdateTextFields();
        LoadProgress();
        if (!progress.dialog[SceneManager.GetActiveScene().buildIndex])
        {
            if (dialogCanvas != null)
            {
                dialogCanvas.SetActive(true);
                GetComponent<PlayerControl>().Disable();
            }
            UpdateDialogs();
        }
    }
    public void LoadProgress()
    {
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new();
            using FileStream stream = new(filePath, FileMode.Open);
            progress = formatter.Deserialize(stream) as SaveProgress;
        }
        else
        {
            Debug.LogError("Progress save not found");
        }

    }
    public bool ValidIndex(int2 index)
    {
        if (index.x < xLength && index.y < yLength && index.x > 0 && index.y > 0) return true;
        return false;
    }
    public int2 ClampIndex(int2 index)
    {
        if (index.x > xLength) index.x = xLength;
        else if (index.x < 0) index.x = 0;
        if (index.y > yLength) index.y = yLength;
        else if (index.y < 0) index.y = 0;
        return index;
    }
    private void UpdateProgress()
    {

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new();
                using FileStream stream = new(filePath, FileMode.Open);
            progress.progress[SceneManager.GetActiveScene().buildIndex] = stepCount;
            formatter.Serialize(stream, progress);
        }
        else
        {
            Debug.LogError("Progress save not found");
        }
    }
    private void UpdateDialogs()
    {

        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new();
            using FileStream stream = new(filePath, FileMode.Open);
            progress.dialog[SceneManager.GetActiveScene().buildIndex] = true;
            formatter.Serialize(stream, progress);
        }
        else
        {
            Debug.LogError("Progress save not found");
        }
    }
    public void UpdateTextFields()
    {
        stepsLeftField.text = stepCount.ToString();
    }
    public void Check()
    {
        stepCount--;
        UpdateTextFields();
        ObjectControl[] objects = FindObjectsOfType<ObjectControl>();
        Zone[] zones = FindObjectsOfType<Zone>();
        for (int i = 0; i < zones.Length; i++)
        {
            if (zones[i] != null)
                for (int j = 0; j < objects.Length; j++)
                {
                    if (objects[j] != null)
                        for (int k = 0; k < objects[j].cellArray.Count; k++)
                        {
                            if (zones[i].IsInZone(objects[j].centerPos + objects[j].cellArray[k]))
                            {
                                switch (zones[i].zoneName)
                                {
                                    case "win":
                                        {
                                            if (objects[j].GetComponentInChildren<Human>() != null)
                                            {
                                                Destroy(objects[j].GetComponentInChildren<Human>().gameObject);
                                                if (FindObjectsOfType<Human>().Length < 2)
                                                {
                                                    Win();
                                                }
                                            }
                                            break;
                                        }
                                    case "lose":
                                        {
                                            Human human = objects[j].GetComponentInChildren<Human>();
                                            if (human != null)
                                            {
                                                zones[i].Worked();
                                                Lose();
                                            }
                                            break;
                                        }
                                    case "disintegration":
                                        {
                                            Destroy(objects[j].gameObject);
                                            zones[i].Worked();
                                            Destroy(zones[i]);
                                            Human human = objects[j].GetComponentInChildren<Human>();
                                            if (human != null)
                                            {
                                                human.gameObject.transform.parent = null;
                                                human.WakeUp();

                                                Invoke(nameof(Lose), collideTime);
                                            }
                                            break;
                                        }
                                }
                            }
                        }
                }
        }
        if (stepCount <= 0)
        {
            Lose();
        }
        hintSystem.ClearHints();
        if (!gameEnded)
        {
            hintSystem.ShowHint();
        }
    }
    public void ShowAd()
    {
        adsController.GetComponent<Interstition>().ShowAd();
    }
    public void Win()
    {
        if (!gameEnded)
        {
            if (winDialogCanvas != null)
            {
                winDialogCanvas.SetActive(true);
            }
            else
            {
                winCanvas.SetActive(true);
            }
            Destroy(gameObject.GetComponent<PlayerControl>());
            gameEnded = true;
            UpdateProgress();
            ShowAd();
        }
    }
    public void Lose()
    {
        if (!gameEnded)
        {
            Human[] humans = FindObjectsOfType<Human>();
            for (int i = 0; i < humans.Length; i++)
            {
                humans[i].WakeUp();
            }
            loseCanvas.SetActive(true);
            Destroy(gameObject.GetComponent<PlayerControl>());
            gameEnded = true;
            ShowAd();
        }
    }
}
