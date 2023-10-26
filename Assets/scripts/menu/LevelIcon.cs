using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class LevelIcon : MonoBehaviour
{
    public List<GameObject> levels = new();
    public SplineContainer spline;
    public GameObject pathSpline;
    public float knotDistance = 0.75f;
    public int stepsLeft = -1;
    public bool orAnd = false;
    public Sprite complete;
    public Sprite available;

    public void Init()

    {
        Image image_component = gameObject.GetComponent<Image>();
        if (stepsLeft>=0)
            image_component.sprite = complete;
        else
            image_component.sprite = available;

        int length = levels.Count;
        bool active;
        if (orAnd)
        {
           active = true;
            for (int i = 0; i < length; i++)
            {
                if (levels[i] != null)
                {
                    LevelIcon icon = levels[i].GetComponent<LevelIcon>();
                    if (icon.stepsLeft < 0)
                    {
                        active = false;
                    }
                }
            }
        }
        else
        {
            active = false;
            for (int i = 0; i < length; i++)
            {
                if (levels[i] != null)
                {
                    LevelIcon icon = levels[i].GetComponent<LevelIcon>();
                    if (icon.stepsLeft >= 0)
                    {
                        active = true;
                    }
                }
            }
        }
        gameObject.SetActive(active);
        if (active)
        {
            for (int i = 0; i < length; i++)
            {
                if (levels[i] != null)
                {
                    LevelIcon icon = levels[i].GetComponent<LevelIcon>();
                    if (icon.stepsLeft >= 0)
                    {
                        pathSpline = Instantiate(pathSpline, new Vector3(0, 0, 0.01f), Quaternion.identity);
                        spline = pathSpline.GetComponent<SplineContainer>();
                        float distance = Mathf.Abs(gameObject.transform.position.x - levels[i].transform.position.x);
                        var knot0 = new BezierKnot
                        {
                            Position = gameObject.transform.position,

                            TangentOut = Vector3.left * (distance * knotDistance)
                        };
                        spline.Spline.SetKnot(0, knot0);
                        var knot1 = new BezierKnot
                        {
                            Position = levels[i].transform.position,

                            TangentOut = Vector3.left * (distance * knotDistance)
                        };

                        spline.Spline.SetKnot(1, knot1);
                    }
                }
            }
        }
    }
}



