using System.Collections.Generic;
using UnityEngine;

public class CollideScript : MonoBehaviour
{
    protected float colideTime;
    protected bool isColide = false;
    protected float animationTime;
    protected Vector3 start;
    protected float randomRange = 0.03f;      //Растояние на которое смещается объект от исходного положения при столкновении
    protected LevelController controller;
    public AudioClip collideAudio;
    protected AudioSource audioComponent;
    private void Start()
    {
        audioComponent = GetComponent<AudioSource>();
        controller = FindAnyObjectByType<LevelController>();
        animationTime = controller.collideTime;
        randomRange = controller.randomRange;
    }
    public virtual void Collide(List<ObjectControl> other)
    {
        StartAnimation();
        audioComponent.clip = collideAudio;
        audioComponent.Play();
    }
    void Update()
    {
        Animation();
    }
    internal void StartAnimation()
    {
        start = transform.position;
        colideTime = Time.time;
        isColide = true;
    }
    internal void Animation()
    {
        if (isColide)
        {
            if (Time.time > colideTime + animationTime)
            {
                gameObject.transform.position = start;
                isColide = false;
            }
            else
            {
                gameObject.transform.position = start + new Vector3(Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange), Random.Range(-randomRange, randomRange));
            }
        }
    }
}
