using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine.SceneManagement;
using Unity.Mathematics;

public class HintSystem : MonoBehaviour
{
    public List<int4> correctPath = new();
    public List<int4> playerPath = new();
    [SerializeField]
    private LevelController controller;
    [SerializeField]
    private GameObject hintObjPrefab;
    [SerializeField]
    private bool writeHints = false;
    [SerializeField]
    private int hintsAmmount = 5;
    private GameObject hintObject;
    private int hintsAvailable = 0;


    public void WriteStep(int4 step)
    {
        if (writeHints)
        {
            correctPath.Add(step);
        }
        else
        {
            playerPath.Add(step);
        }
    }
    public void ShowHint()
    {
        if (hintsAvailable > 0)
        {
            hintsAvailable--;
            int index = playerPath.Count;
            if (index==0|| playerPath[index-1].Equals(correctPath[index - 1]))
            {
                ObjectControl obj = controller.cellArray[correctPath[index].x, correctPath[index].y];
                Debug.Log(correctPath);
                Quaternion stepDirection = Quaternion.LookRotation(new Vector3(correctPath[index].z, 0, correctPath[index].w), Vector3.up);
                hintObject = Instantiate(hintObjPrefab, obj.transform.position + Vector3.up, stepDirection);
            }
        }
    }
    public void ClearHints()
    {
        Destroy(hintObject);
    }
    public void AddHints()
    {
        hintsAvailable = hintsAmmount;
    }
}
