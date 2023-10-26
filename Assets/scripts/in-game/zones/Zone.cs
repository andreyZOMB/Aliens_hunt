using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public List<int2> cellArray = new();
    [NonSerialized]
    public ObjectControl parrent;
    public string zoneName = "win";
    protected float moveTime;
    [SerializeField]
    protected AudioClip audioClip;
    protected virtual void Start()
    {
        parrent = GetComponent<ObjectControl>();
        cellArray = CoordsRotator.Orient(cellArray, transform.rotation.eulerAngles.y);
        moveTime = FindObjectOfType<LevelController>().moveTime;
    }
    public bool IsInZone(int2 pos)
    {
        return cellArray.Contains(pos - parrent.centerPos);
    }
    public virtual void Worked()
    {
        PlayAudio();
    }
    public virtual void PlayAudio()
    {
        if (audioClip != null)
        {
            AudioSource source = GetComponent<AudioSource>();
            source.clip = audioClip;
            source.Play();
        }
    }
}
