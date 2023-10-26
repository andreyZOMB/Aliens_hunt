using Unity.Mathematics;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    private float startTime;
    private float animationTime;
    private Vector3 start;
    private Vector3 way;
    private bool isMoving = false;
    public AudioClip moveAudio;
    protected AudioSource audioComponent;
    private void Start()
    {
        audioComponent = GetComponent<AudioSource>();
    }
    public void StartMove(float3 start_position, float3 end_position, float time)
    {
        start = start_position;
        way = end_position - start_position;
        startTime = Time.time;
        animationTime = time;
        isMoving = true;
        audioComponent.clip = moveAudio;
        audioComponent.Play();
    }
    void Update()
    {
        if (isMoving)
        {
            if (Time.time > startTime + animationTime)
            {
                gameObject.transform.position = start + way;
                isMoving = false;
            }
            else
            {
                float l = (Time.time - startTime) / animationTime;
                gameObject.transform.position = start + way * l;
            }
        }
    }
}
